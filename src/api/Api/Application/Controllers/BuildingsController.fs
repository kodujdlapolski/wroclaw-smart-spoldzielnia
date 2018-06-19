namespace Api.Controllers
open Microsoft.AspNetCore.Mvc
open FrameworkACL
open Building

type BuildingsController
  (buildingsProvider : IBuildingsProvider, 
   response : IBuildingResponseBuilder) =
  inherit Controller()

  [<HttpGet>]
  [<Route("buildings")>]
  member __.Get() =
    buildingsProvider.Get() 
    |> handle (Seq.map response.Success) response.CollectionError
    
  [<HttpGet>]
  [<Route("buildings/{id}")>]
  member __.GetSingle(id : int) : IActionResult = 
    buildingsProvider.GetSingle(BuildingId id) 
    |> handle response.Success (response.Error (BuildingId id))