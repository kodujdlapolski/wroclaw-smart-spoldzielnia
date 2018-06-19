module BuildingWebObject
open Newtonsoft.Json
open Newtonsoft.Json.Serialization
open Links

[<JsonObject(NamingStrategyType = typeof<SnakeCaseNamingStrategy>)>]
type BuildingWebObject = 
  {
    Name : string
    Description : string
    Id : string 
    [<JsonProperty("_links")>]
    Links : Links
  }