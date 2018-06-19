module FrameworkACL
open Building
open BuildingWebObject
open Service
open ServiceWebObject
open Microsoft.AspNetCore.Mvc

//Types here exist for the sole purpose to be registered in IServiceCollection
//These types should not be used anywhere else than controller

type IBuildingResponseBuilder = 
  abstract member Success : Building -> BuildingWebObject
  abstract member Error : BuildingId -> BuildingError -> int * string
  abstract member CollectionError : BuildingError -> int * string

type IServiceResponseBuilder = 
  abstract member Build : Service -> ServiceWebObject

type IBuildingServiceProvider = 
  abstract member Get : BuildingId -> RetrievedServices

type IBuildingsProvider = 
  abstract member Get : unit -> RetrievedBuildings
  abstract member GetSingle : BuildingId -> RetrievedBuilding

let handle 
    (makeSuccess : 'success -> 'a)
    (makeFailure : 'failure -> int * 'b) 
    (result : Result<'success,'failure>) = 
    match result with 
    | Ok(success) -> makeSuccess success |> OkObjectResult :> IActionResult
    | Error(failure) -> 
      let code, webObject = makeFailure failure
      new ObjectResult(webObject, StatusCode = new System.Nullable<int>(code))
      :> IActionResult

    

