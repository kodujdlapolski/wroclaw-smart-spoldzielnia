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
  let builder = 
    { new IResponseBuilder 
      with member __.Build r b = toWebObject urlProvider r b 
    }

  kernel.AddTransient<IBuildingsProvider>(fun _ -> provider) |> ignore
  kernel.AddTransient<IResponseBuilder>(fun _ -> builder) |> ignore