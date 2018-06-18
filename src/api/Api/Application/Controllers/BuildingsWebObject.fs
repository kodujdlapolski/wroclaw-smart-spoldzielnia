module BuildingsWebObject

open Newtonsoft.Json
open Microsoft.AspNetCore.Http
open Domain
open Newtonsoft.Json.Serialization

type BaseUrlProvider = HttpRequest -> string

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

type ISingleBuildingAffordanceBuilder = 
  abstract member Build : HttpRequest -> Building -> BuildingWebObject

type ICollectionBuildingAffordanceBuilder = 
  abstract member Build : HttpRequest -> Building -> BuildingWebObject

let urlProvider (request : HttpRequest) = 
  sprintf "%s%s" 
    (request.PathBase.ToUriComponent()) 
    (request.Path.ToUriComponent())

let toWebObject name desc id links = 
  { Name = name; 
    Description = desc; 
    Id = id |> string; 
    Links = links |> Map.ofList
  }

let collectionBuildingAffordances 
  baseUrlProvider 
  request { 
          Building.Name = name; 
          Building.Description = desc; 
          Building.Id = id } 
          =
  let baseUrl = baseUrlProvider request
  let links = [
            ("self", { Href = sprintf "%s/%i" baseUrl id; 
                       Templated = false
                     })
            ]
  toWebObject name desc (string id) links

let singleBuildingAffordances 
  baseUrlProvider 
  request { 
          Building.Name = name; 
          Building.Description = desc; 
          Building.Id = id } 
          =
  let baseUrl = baseUrlProvider request
  let links = [
            ("self", { Href = baseUrl 
                       Templated = false
                     })
            ]
  toWebObject name desc (string id) links 