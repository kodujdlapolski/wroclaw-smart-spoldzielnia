module DependencyResolution

open Microsoft.Extensions.DependencyInjection
open FrameworkACL
open BuildingsWebObject
open DataAccess
open Domain
open ConnectionString

let registerServices (kernel: IServiceCollection) = 
  let getBuilding = getBuilding (getBuildingById connectionString)
  let getBuildings = getBuildings connectionString

  let provider = 
    { new IBuildingsProvider 
      with 
        member __.Get() = getBuildings()
        member __.GetSingle id = getBuilding id
    }
  let collectionAffordancesBuilder = 
    { new ICollectionBuildingAffordanceBuilder 
      with member __.Build r b = collectionBuildingAffordances urlProvider r b 
    }

  let singleAffordancesBuilder = 
    { new ISingleBuildingAffordanceBuilder 
      with member __.Build r b = singleBuildingAffordances urlProvider r b 
    }  

  kernel.AddTransient<IBuildingsProvider>(fun _ -> provider) |> ignore
  kernel.AddTransient<ICollectionBuildingAffordanceBuilder>(fun _ -> collectionAffordancesBuilder) |> ignore
  kernel.AddTransient<ISingleBuildingAffordanceBuilder>(fun _ -> singleAffordancesBuilder) |> ignore