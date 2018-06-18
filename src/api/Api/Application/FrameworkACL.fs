module FrameworkACL
open Building
open BuildingsWebObject

//Types here exist for the sole purpose to be registered in IServiceCollection
//These types should not be used anywhere else than controller

type IBuildingResponseBuilder = 
  abstract member Build : Building -> BuildingWebObject

type IBuildingsProvider = 
  abstract member Get : unit -> RetrievedBuildings
  abstract member GetSingle : BuildingId -> RetrievedBuilding