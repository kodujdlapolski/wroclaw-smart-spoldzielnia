namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open Providers
open JsonResultBuilder

[<Route("api/buildings")>]
type BuildingsController(buildingsProvider : IBuildingsProvider, jsonBuilder : IJsonResultBuilder) =
    inherit Controller()

    [<HttpGet>]
    member this.Get() =
        buildingsProvider.Get() |> List.map jsonBuilder.BuildJsonResult
