module FrameworkACL
open Domain
open BuildingsWebObject

//Types here exist for the sole purpose to be registered in IServiceCollection
//These types should not be used anywhere else than controller

type IBuildingResponseBuilder = 
  abstract member Build : Building -> BuildingWebObject

type IBuildingsProvider = 
  abstract member Get : unit -> Building list
  abstract member GetSingle : int -> Building option