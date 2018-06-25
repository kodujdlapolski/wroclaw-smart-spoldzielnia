module ServiceWebObject
open Newtonsoft.Json
open Newtonsoft.Json.Serialization
open Links

[<JsonObject(NamingStrategyType = typeof<SnakeCaseNamingStrategy>)>]
type ServiceWebObject = 
  {
    Name : string
    Description : string
    Id : string 
    [<JsonProperty("_links")>]
    Links : Links
  }