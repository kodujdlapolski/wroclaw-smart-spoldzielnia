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
  let getBuilding = getBuilding (buildingById connectionString)
  let getBuildings = getAllBuildings (buildings connectionString)
  let getServices = 
    getServicesForBuilding 
      getBuilding 
      mapServices 
      (servicesForBuilding connectionString)

  let buildingsProvider = 
    { new IBuildingsProvider with 
      member __.Get() = getBuildings()
      member __.GetSingle id = getBuilding id }

  let servicesProvider = 
    { new IBuildingServiceProvider with 
      member __.Get(id) = getServices id }  

  let buildingResponseBuilder = 
    { new IBuildingResponseBuilder with
      member __.Success b = BuildingResponse.toBuildingWebObject buildUri b
      member __.Error b r = BuildingResponse.toBuildingError b r
      member __.CollectionError r = BuildingResponse.toBuildingsError r
    }

  let serviceResponseBuilder = 
    { new IServiceResponseBuilder with
      member __.Build s = ServiceResponse.toServiceWebObject buildUri s
     }  

  addToKernel<IBuildingsProvider> kernel buildingsProvider |> ignore
  addToKernel<IBuildingResponseBuilder> kernel buildingResponseBuilder |> ignore
  addToKernel<IBuildingServiceProvider> kernel servicesProvider |> ignore
  addToKernel<IServiceResponseBuilder> kernel serviceResponseBuilder |> ignore