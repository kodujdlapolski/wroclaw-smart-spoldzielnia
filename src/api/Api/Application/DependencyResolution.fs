module DependencyResolution
open Microsoft.Extensions.DependencyInjection
open FrameworkACL
open DataAccess.Buildings
open DataAccess.Services
open Building
open ConnectionString
open Affordances
open Service

let addToKernel<'a when 'a : not struct> 
  (kernel: IServiceCollection) (instance : 'a) = 
  kernel.AddTransient<'a>(fun _ -> instance)

let registerServices (kernel: IServiceCollection) = 
  let getBuilding = getBuilding (fetchBuildingById connectionString)
  let getBuildings = getAllBuildings (fetchBuildings connectionString)
  let getServices = 
    getServicesForBuilding getBuilding (fetchServicesForBuilding connectionString)

  let buildingsProvider = 
    { new IBuildingsProvider with 
      member __.Get() = getBuildings()
      member __.GetSingle id = getBuilding id }

  let servicesProvider = 
    { new IBuildingServiceProvider with 
      member __.Get(id) = getServices id }  

  let buildingResponseBuilder = 
    { new IBuildingResponseBuilder with
      member __.Build b = BuildingsWebObject.buildWebObject buildUri b
    }

  let serviceResponseBuilder = 
    { new IServiceResponseBuilder with
      member __.Build s = ServiceWebObject.buildWebObject buildUri s
     }  

  addToKernel<IBuildingsProvider> kernel buildingsProvider |> ignore
  addToKernel<IBuildingResponseBuilder> kernel buildingResponseBuilder |> ignore
  addToKernel<IBuildingServiceProvider> kernel servicesProvider |> ignore
  addToKernel<IServiceResponseBuilder> kernel serviceResponseBuilder |> ignore