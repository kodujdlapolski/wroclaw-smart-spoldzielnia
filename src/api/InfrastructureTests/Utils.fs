module Utils

let test q =  lazy Swensen.Unquote.Assertions.test q
let (=>?) name list = Expecto.Tests.testList name list
let (->?) name (t: Lazy<unit>) = Expecto.Tests.testCase name (fun _ -> t.Value)
let (->???) name (t: Lazy<unit>) = Expecto.Tests.ftestCase name (fun _ -> t.Value)
type Tests = Expecto.TestsAttribute