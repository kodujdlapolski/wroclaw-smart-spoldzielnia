module BuildingsWebObject

open Newtonsoft.Json
open Domain
open Affordances
open Newtonsoft.Json.Serialization

[<JsonObject(NamingStrategyType = typeof<SnakeCaseNamingStrategy>)>]
type Link = {Href : string; Templated : bool;}

type Links = Map<string,Link>

[<JsonObject(NamingStrategyType = typeof<SnakeCaseNamingStrategy>)>]
type BuildingWebObject = 
  {
    Name : string
    Description : string
    Id : string 
    [<JsonProperty("_links")>]
    Links : Links
  }

let buildWebObject 
  uriBuilder
  { Building.Name = name; Building.Description = desc; Building.Id = id } =

  let self = Building(BuildingId id)
  let services = BuildingServices(BuildingId id, CollectionOfServices)
  let toWebObject name desc id links = 
    { Name = name; 
      Description = desc; 
      Id = id |> string; 
      Links = links |> Map.ofList
    }
  let links = [
            ("self", { Href = self |> uriBuilder; Templated = false });
            ("services", {Href = services |> uriBuilder; Templated = false})                   
            ]
  toWebObject name desc (string id) links