namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open FrameworkACL
open Building

type BuildingsController
  (buildingsProvider : IBuildingsProvider, 
   singleBuilder : IBuildingResponseBuilder) =
  inherit Controller()

  [<HttpGet>]
  [<Route("buildings")>]
  member this.Get() =
    match buildingsProvider.Get() with
    | Ok(buildings) -> buildings
                      |> Seq.map singleBuilder.Build
                      |> this.Ok :> IActionResult
    | Error(_) -> this.StatusCode(500) :> IActionResult

  [<HttpGet>]
  [<Route("buildings/{id}")>]
  member this.GetSingle(id : int) : IActionResult = 
    match buildingsProvider.GetSingle(BuildingId id) with
    | Ok(building) -> building 
                      |> singleBuilder.Build 
                      |> this.Ok :> IActionResult
    | Error(NotFound) -> this.NotFound() :> IActionResult
    | Error(FoundDuplicate)
    | Error(Panic) -> this.StatusCode(500) :> IActionResult