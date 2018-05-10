module ConnectionString
open FParsec

type ConnectionStringParams = {
  UserName : string
  Password : string
  Port : string
  Server : string
  DbName : string
}

module Parser = 
  let userParser = pstring "postgres://" >>. many1SatisfyL isLetter "user identifier"
  let passwordParser = pchar ':' >>. many1SatisfyL (fun c -> c <> '@') "password"
  let serverParser = pchar '@' >>. many1SatisfyL (fun c -> c <> ':') "server address"
  let portParser = pchar ':' >>. many1SatisfyL isDigit "port number"
  let dbNameParser = pchar '/' >>. restOfLine true

  let postgresConnectionStringParser = tuple5 userParser passwordParser serverParser portParser dbNameParser

  let parsePostgresConnectionString str = 
    match run postgresConnectionStringParser str with
    | Success((user, pass, serv, port, db),_,_) -> Some {UserName = user; Password = pass; Server = serv; Port = port; DbName = db }
    | Failure(_) -> None
  
let connectionString = 
  let databaseUrl() = 
    match System.Environment.GetEnvironmentVariable("DATABASE_URL") with 
    | null -> None
    | s -> Some s
  let url = databaseUrl()
  match Option.bind Parser.parsePostgresConnectionString url with
  | Some(p) -> sprintf "Server=%s;Port=%s;Database=%s;User Id=%s;Password=%s;" p.Server p.Port p.DbName p.UserName p.Password
  | None -> sprintf  "unable to parse DATABASE_URL environment variable %A" url |> failwith

        