module Utils

let test =  Swensen.Unquote.Assertions.test
let (=>?) name list = Expecto.Tests.testList name list
let (->?) name t = Expecto.Tests.testCase name t
let (->???) name t = Expecto.Tests.ftestCase name t
type Tests = Expecto.TestsAttribute

let strContains (content:string) (text:string) =
  text.Contains content 

let strContainsAll (content:string list) (text:string) =
  let folder state str = state && text.Contains str
  content |> List.fold folder true