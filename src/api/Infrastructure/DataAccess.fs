module DataAccess
open Building
open FSharp.Data.Sql
open ConnectionString

module private Internal = 

  [<Literal>]
  let CompiletimeConnectionString = 
    "Server=localhost;Port=5432;Database=spoldzielnia;User Id=Dev;Password=dev;"

  [<Literal>]
  let ResolutionPath = @".\libraries"

  type Db = 
    SqlDataProvider<
      Common.DatabaseProviderTypes.POSTGRESQL,
      ConnectionString = CompiletimeConnectionString,
      ResolutionPath = ResolutionPath>

module Buildings = 

  let fetchBuildings connectionString () =
    try
      let dataContext = Internal.Db.GetDataContext (connectionString |> value)
      dataContext.Public.Buildings    
      |> Seq.map (
          fun b -> { Id = BuildingId(b.Id); 
                     Name = b.Name; 
                     Description = b.Description 
                   })
      |> Some
    with _ -> None 

  let fetchBuildingById connectionString (BuildingId id) = 
    try
      let dataContext = Internal.Db.GetDataContext (connectionString |> value)
      query {
        for building in dataContext.Public.Buildings do
          where (building.Id = id)
          select building
      } 
      |> Seq.map (fun row -> { Id = BuildingId(row.Id);
                                 Name = row.Name; 
                                 Description = row.Description })
      |> Some   
    with _ -> None 

module Services = 
  open Service

  let fetchServicesForBuilding connectionString (BuildingId id) = 
    try
      let dataContext = Internal.Db.GetDataContext (connectionString |> value)
      query {
        for service in dataContext.Public.Services do
          where (service.Buildingid = id)
          select service      
      } 
      |> Seq.map (fun row -> { ServiceId = row.Id;
                               ServiceName = row.Name;
                               ServiceDescription = row.Description })
      |> Some                           
    with _ -> None
  