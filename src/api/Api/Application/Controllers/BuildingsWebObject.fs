module BuildingsWebObject

open Microsoft.AspNetCore.Http
open Domain

type Link = {Href : string; Relation : string}

type BaseUrlProvider = HttpRequest -> string

type BuildingWebObject = 
  { Name: string; Description : string; Id: string; Links: Link list }

type IResponseBuilder = 
  abstract member Build : HttpRequest -> Building -> BuildingWebObject

let urlProvider (request : HttpRequest) = 
  let scheme() = 
    match System.Environment.GetEnvironmentVariable("API_SCHEME") with 
    | null -> "http"
    | s -> s
  sprintf "%s://%s%s%s" (scheme()) (request.Host.ToUriComponent()) (request.PathBase.ToUriComponent()) (request.Path.ToUriComponent())

let toWebObject baseUrlProvider request { Building.Name = name; Building.Description = desc; Building.Id = id }=
  let baseUrl = baseUrlProvider request
  {Name = name; Description = desc; Id = id |> string; Links = [{Relation = "self"; Href = sprintf "%s/%i" baseUrl id}]}