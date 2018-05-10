module DataAccess
open Models
open FSharp.Data.Sql

module private Internal = 

    [<Literal>]
    let CompiletimeConnectionString = "Server=localhost;Port=5432;Database=spoldzielnia;User Id=Dev;Password=dev;"

    [<Literal>]
    let ResolutionPath = @".\libraries"

    type Db = 
        SqlDataProvider<Common.DatabaseProviderTypes.POSTGRESQL,ConnectionString = CompiletimeConnectionString,ResolutionPath = ResolutionPath>

let getBuildings (connectionString : string) =
    let dataContext = Internal.Db.GetDataContext connectionString
    dataContext.Public.Buildings    
    |> Seq.map (fun b -> {Id = b.Id; Name = b.Name; Description = b.Description})
