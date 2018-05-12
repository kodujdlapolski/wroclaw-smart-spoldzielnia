module BuildingsWebObject

open Microsoft.AspNetCore.Mvc
open Newtonsoft.Json
open Microsoft.AspNetCore.Http
open System.Net.Http
open Models

type Link = {Href : string}

type BuildingWebObject = 
  { Name: string; Description : string; Id: string; Links: Link list }

type IResponseBuilder = 
  abstract member Build : Building -> BuildingWebObject

let toWebObject { Building.Name = name; Building.Description = desc; Building.Id = id } =
  {Name = name; Description = desc; Id = id |> string; Links = []}