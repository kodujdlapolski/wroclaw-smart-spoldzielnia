namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open DataAccess
open ConnectionString

[<Route("api/buildings")>]
type BuildingsController () =
    inherit Controller()

    [<HttpGet>]
    member this.Get() =
        getBuildings connectionString |> List.ofSeq
