namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open FrameworkACL
open Building

type ServicesController
  (serviceProvider : IBuildingServiceProvider,
   responseBuilder : IServiceResponseBuilder) =
  inherit Controller()

  [<HttpGet>]
  [<Route("buildings/{id}/services")>]
  member __.Get(id) =
    serviceProvider.Get(BuildingId id) |> 
      handle 
        (List.map responseBuilder.Success)
        (responseBuilder.CollectionError (BuildingId id))