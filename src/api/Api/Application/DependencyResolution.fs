module DependencyResolution

open Microsoft.Extensions.DependencyInjection
open FrameworkACL
open BuildingsWebObject
open DataAccess.Buildings
open DataAccess.Services
open Building
open ConnectionString
open Affordances

let registerServices (kernel: IServiceCollection) = 
  let getBuilding = getBuilding (getBuildingById connectionString)
  let getBuildings = getAllBuildings (getBuildings connectionString)

  let provider = 
    { new IBuildingsProvider 
      with 
        member __.Get() = getBuildings()
        member __.GetSingle id = getBuilding id
    }

  let singleAffordancesBuilder = 
    { new IBuildingResponseBuilder 
      with member __.Build b = buildWebObject buildUri b
    }  

  kernel.AddTransient<IBuildingsProvider>(fun _ -> provider) |> ignore
  kernel.AddTransient<IBuildingResponseBuilder>(fun _ -> singleAffordancesBuilder) |> ignore