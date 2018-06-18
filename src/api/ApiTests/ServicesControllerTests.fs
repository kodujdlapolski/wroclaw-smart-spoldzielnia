module ServicesControllerTests

open Utils
open Building
open Service
open Api.Controllers
open FrameworkACL
open ServiceWebObject
open Microsoft.AspNetCore.Mvc
open System


let getOkResponseBody<'a> (resp : IActionResult) = 
  let okResp = resp :?> OkObjectResult
  okResp.Value :?> 'a

let mockResponseBuilder webObject = 
  { new IServiceResponseBuilder with 
    member __.Build _ = webObject }

let mockServiceProvider result = 
  { new IBuildingServiceProvider with 
    member __.Get _ = result }  

[<Tests>]
let servicesControllerTests = 

  let dummyServiceWebObject = 
    {Name = "name"; Description = "Description"; Id = "zzz"; Links = [] |> Map.ofList}

  let dummyService = 
    { Name = "name"; 
      Description = "Description";
      Id = ServiceId(2);
      Building = { Name = "building";
                   Description = "building desc";
                   Id = BuildingId(777)}}

  "Should provide collection of services for building" =>?
  [
    "Should return all services" ->? fun _ ->
      let count = 10
      let providerDouble = 
        dummyService 
        |> List.replicate count 
        |> Ok 
        |> mockServiceProvider
      let builder = mockResponseBuilder dummyServiceWebObject
      let controller = 
        new ServicesController(providerDouble, builder)

      let result = 
        controller.Get(0) 
        |> getOkResponseBody<ServiceWebObject list>

      test <@ List.length result = count @>      


     

  ]