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
  "should be able to parse all parameters" =>?
    [
      ( "should be able to parse user name" ->?
        let parsed = runParser userParser "postgres://user:"
        test <@ parsed = OK("user") @> )

      ( "should be able to parse password" ->?
        let parsed = runParser passwordParser ":pass@server"
        test <@ parsed = OK("pass") @> )      

      ( "should be able to parse server" ->?
        let parsed = runParser serverParser "@server:5432"
        test <@ parsed = OK("server") @> )  

      ( "should be able to parse port" ->?
        let parsed = runParser portParser ":5432/dbName"
        test <@ parsed = OK("5432") @> )   

      ( "should be able to db name" ->?
        let parsed = runParser dbNameParser "/dbName"
        test <@ parsed = OK("dbName") @> )

      ( "should be able to parse all" ->?
        let parsed = runParser postgresConnectionStringParser "postgres://user:pass@server:5432/dbName"
        test <@ parsed = OK("user", "pass", "server", "5432", "dbName") @> )  

      ( "should be able to parse real life example" ->?
        let parsed = runParser postgresConnectionStringParser "postgres://zbmsaqqyysivwsk:efd316c219402b7c7c1901b3ff69f79ae94b1fbb006da8cd7322339a7064@ec2-84-247-161-18.eu-west-1.compute.amazonaws.com:5432/dni0jptol2gcf"
        test <@ parsed = OK("zbmsaqqyysivwsk", "efd316c219402b7c7c1901b3ff69f79ae94b1fbb006da8cd7322339a7064", "ec2-84-247-161-18.eu-west-1.compute.amazonaws.com", "5432", "dni0jptol2gcf") @> )  
    ]

