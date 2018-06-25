module Links
open Newtonsoft.Json
open Newtonsoft.Json.Serialization

[<JsonObject(NamingStrategyType = typeof<SnakeCaseNamingStrategy>)>]
type Link = {Href : string; Templated : bool;}

type Links = Map<string,Link>