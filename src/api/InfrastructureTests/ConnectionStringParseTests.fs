module ConnectionStringParseTests

open ConnectionString.Parser
open Utils
open FParsec

type Either<'a> = 
  | OK of 'a
  | Error of string

let runParser parser str = 
  match run parser str with
  | Success(result,_,_) -> OK result
  | Failure(msg,_,_) -> Error msg

[<Tests>]
let parsingPostgresStyleConnectionString = 
  "should be able to parse postgres connection string" =>?
    [
      "should be able to parse user name" ->? fun _ ->
        let parsed = runParser userParser "postgres://user:"
        test <@ parsed = OK("user") @>

      "should be able to parse password" ->? fun _ ->
        let parsed = runParser passwordParser ":pass@server"
        test <@ parsed = OK("pass") @>

      "should be able to parse server" ->? fun _ ->
        let parsed = runParser serverParser "@server:5432"
        test <@ parsed = OK("server") @>

      "should be able to parse port" ->? fun _ ->
        let parsed = runParser portParser ":5432/dbName"
        test <@ parsed = OK("5432") @>

      "should be able to db name" ->? fun _ ->
        let parsed = runParser dbNameParser "/dbName"
        test <@ parsed = OK("dbName") @>

      "should be able to parse all" ->? fun _ ->
        let parsed = 
          "postgres://user:pass@server:5432/dbName" |> 
          runParser postgresConnectionStringParser 
          
        test <@ parsed = OK("user", "pass", "server", "5432", "dbName") @> 

      "should be able to parse real life example" ->? fun _ ->
        let parsed = 
          "postgres://zbmsaqqyysivwsk:\
          efd316c219402b7c7c1901b3ff69f79ae94b1fbb006da8cd7322339a7064@\
          ec2-84-247-161-18.eu-west-1.compute.amazonaws.com:\
          5432/dni0jptol2gcf" |>
          runParser postgresConnectionStringParser 
            
        test <@ parsed = OK(
                  "zbmsaqqyysivwsk", 
                  "efd316c219402b7c7c1901b3ff69f79ae94b1fbb006da8cd7322339a7064",
                  "ec2-84-247-161-18.eu-west-1.compute.amazonaws.com",
                  "5432",
                  "dni0jptol2gcf") @> 
    ]

