namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open FrameworkACL
open BuildingsWebObject

type BuildingsController
  (buildingsProvider : IBuildingsProvider, 
   responseBuilder : IResponseBuilder) =
  inherit Controller()

  [<HttpGet>]
  [<Route("api/buildings")>]
  member this.Get() =
    buildingsProvider.Get() 
    |> List.map (responseBuilder.Build this.Request)
    |> this.Ok

  [<HttpGet>]
  [<Route("api/buildings/{id}")>]
  member this.GetSingle(id : int) : IActionResult = 
    buildingsProvider.GetSingle(id) 
    |> this.BuildResult (responseBuilder.Build this.Request)

  member private this.BuildResult projection result : IActionResult = 
    match result with 
    | Some (r) -> this.Ok(r |> projection) :> IActionResult
    | None -> this.NotFound() :> IActionResult  
