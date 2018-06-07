module DataAccess
open Domain
open FSharp.Data.Sql
open ConnectionString

module private Internal = 

    [<Literal>]
    let CompiletimeConnectionString = "Server=localhost;Port=5432;Database=spoldzielnia;User Id=Dev;Password=dev;"

    [<Literal>]
    let ResolutionPath = @".\libraries"

    type Db = 
        SqlDataProvider<Common.DatabaseProviderTypes.POSTGRESQL,ConnectionString = CompiletimeConnectionString,ResolutionPath = ResolutionPath>

let getBuildings connectionString () =
    let dataContext = Internal.Db.GetDataContext (connectionString |> value)
    dataContext.Public.Buildings    
    |> Seq.map (fun b -> { Id = b.Id; Name = b.Name; Description = b.Description })
    |> List.ofSeq