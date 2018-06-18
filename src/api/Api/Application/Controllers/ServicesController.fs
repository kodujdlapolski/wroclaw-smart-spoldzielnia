namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open FrameworkACL
open Building

type ServicesController(
  serviceProvider : IBuildingServiceProvider,
  responseBuilder : IServiceResponseBuilder) =
  inherit Controller()

  [<HttpGet>]
  [<Route("buildings/{id}/services")>]
  member this.Get(id) =
    match serviceProvider.Get(BuildingId id) with 
    | Ok(buildings) -> buildings 
                       |> List.map responseBuilder.Build
                       |> this.Ok