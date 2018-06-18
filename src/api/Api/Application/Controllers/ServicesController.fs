namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open FrameworkACL
open Building

type ServicesController() =
  inherit Controller()

  [<HttpGet>]
  [<Route("buildings/{id}/services")>]
  member this.Get() =
    "hello there"