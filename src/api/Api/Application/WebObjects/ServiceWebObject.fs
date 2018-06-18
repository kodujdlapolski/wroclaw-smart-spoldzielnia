module ServiceWebObject

open Newtonsoft.Json
open Service
open Affordances
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

let buildWebObject 
  uriBuilder 
  { Name = name;
    Description = description;
    Id = ServiceId(id);
    Building = building } =

  let self = BuildingServices(building.Id, Service(ServiceId id))
  let toWebObject links = 
    { Name = name; 
      Description = description; 
      Id = id |> string; 
      Links = links |> Map.ofList
    }
  let links = [("self", { Href = self |> uriBuilder; Templated = false });]
  toWebObject links