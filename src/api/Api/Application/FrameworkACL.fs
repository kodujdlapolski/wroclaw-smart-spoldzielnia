module FrameworkACL
open Building
open BuildingsWebObject
open Service
open ServiceWebObject

//Types here exist for the sole purpose to be registered in IServiceCollection
//These types should not be used anywhere else than controller

type IBuildingResponseBuilder = 
  abstract member Build : Building -> BuildingWebObject

type IServiceResponseBuilder = 
  abstract member Build : Service -> ServiceWebObject

type IBuildingServiceProvider = 
  abstract member Get : BuildingId -> RetrievedServices

type IBuildingsProvider = 
  abstract member Get : unit -> RetrievedBuildings
  abstract member GetSingle : BuildingId -> RetrievedBuilding

