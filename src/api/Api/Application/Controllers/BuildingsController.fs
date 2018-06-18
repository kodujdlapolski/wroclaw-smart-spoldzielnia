namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open FrameworkACL
open BuildingsWebObject

type BuildingsController
  (buildingsProvider : IBuildingsProvider, 
   collectionBuilder : ICollectionBuildingAffordanceBuilder,
   singleBuilder : ISingleBuildingAffordanceBuilder) =
  inherit Controller()

  [<HttpGet>]
  [<Route("buildings")>]
  member this.Get() =
    buildingsProvider.Get() 
    |> List.map (collectionBuilder.Build this.Request)
    |> this.Ok

  [<HttpGet>]
  [<Route("buildings/{id}")>]
  member this.GetSingle(id : int) : IActionResult = 
    buildingsProvider.GetSingle(id) 
    |> this.BuildResult (singleBuilder.Build this.Request)

  member private this.BuildResult projection result : IActionResult = 
    match result with 
    | Some (r) -> this.Ok(r |> projection) :> IActionResult
    | None -> this.NotFound() :> IActionResult  
