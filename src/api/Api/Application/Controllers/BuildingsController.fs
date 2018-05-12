namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open Providers
open BuildingsWebObject

[<Route("api/buildings")>]
type BuildingsController(buildingsProvider : IBuildingsProvider, responseBuilder : IResponseBuilder) =
    inherit Controller()

    [<HttpGet>]
    member this.Get() =
        buildingsProvider.Get() |> List.map responseBuilder.Build
