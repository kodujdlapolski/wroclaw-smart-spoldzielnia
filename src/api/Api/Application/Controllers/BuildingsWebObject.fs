module BuildingsWebObject

open Microsoft.AspNetCore.Mvc
open Newtonsoft.Json
open Microsoft.AspNetCore.Http
open System.Net.Http
open Domain

type Link = {Href : string; Relation : string}

type BaseUrlProvider = HttpRequest -> string

type BuildingWebObject = 
  { Name: string; Description : string; Id: string; Links: Link list }

type IResponseBuilder = 
  abstract member Build : HttpRequest -> Building -> BuildingWebObject

let urlProvider (request : HttpRequest) = 
  sprintf "https://%s%s%s" (request.Host.ToUriComponent()) (request.PathBase.ToUriComponent()) (request.Path.ToUriComponent())

let toWebObject baseUrlProvider request { Building.Name = name; Building.Description = desc; Building.Id = id }=
  let baseUrl = baseUrlProvider request
  {Name = name; Description = desc; Id = id |> string; Links = [{Relation = "self"; Href = sprintf "%s/%i" baseUrl id}]}