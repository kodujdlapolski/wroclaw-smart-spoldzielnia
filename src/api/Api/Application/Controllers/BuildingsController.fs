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
    buildingsProvider.Get() 
    |> List.map singleBuilder.Build
    |> this.Ok

  [<HttpGet>]
  [<Route("buildings/{id}")>]
  member this.GetSingle(id : int) : IActionResult = 
    buildingsProvider.GetSingle(BuildingId id) 
    |> this.BuildResult singleBuilder.Build

  member private this.BuildResult projection result : IActionResult = 
    match result with 
    | Some (r) -> this.Ok(r |> projection) :> IActionResult
    | None -> this.NotFound() :> IActionResult  
