namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open FrameworkACL
open BuildingsWebObject

type BuildingsController(buildingsProvider : IBuildingsProvider, responseBuilder : IResponseBuilder) =
    inherit Controller()

    [<HttpGet>]
    [<Route("api/buildings")>]
    member this.Get() =
        buildingsProvider.Get() |> List.map (responseBuilder.Build this.Request)
  
    [<HttpGet>]
    [<Route("api/buildings/{id}")>]
    member this.GetSingle(id : int) = 
        id
