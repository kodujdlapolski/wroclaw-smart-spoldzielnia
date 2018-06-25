module ServicesControllerTests

open Utils
open Building
open Service
open Api.Controllers
open FrameworkACL
open ServiceWebObject
open Microsoft.AspNetCore.Mvc
open Microsoft.FSharp.Linq.NullableOperators

let getOkResponseBody<'a> (resp : IActionResult) = 
  let okResp = resp :?> OkObjectResult
  okResp.Value :?> 'a

let mockResponseBuilder webObject = 
  { new IServiceResponseBuilder with 
    member __.Success _ = webObject
    member __.CollectionError _ _ = failwith "" }

let mockServiceProvider result = 
  { new IBuildingServiceProvider with 
    member __.Get _ = result }  

let stubResponseBuilderWithError code msg = 
  {new IServiceResponseBuilder with 
   member __.Success _ = failwith ""
   member __.CollectionError _ _ = code, msg } 

[<Tests>]
let servicesControllerTests = 

  let dummyServiceWebObject = 
    { Name = "name"; 
      Description = "Description"; 
      Id = "zzz"; 
      Links = [] |> Map.ofList }

  let dummyService = 
    { Name = "name"; 
      Description = "Description";
      Id = ServiceId(2);
      Building = { Name = "building";
                   Description = "building desc";
                   Id = BuildingId(777)}}

  "Retrieving collection of services for building" =>?
  [
    "Should return all services" ->? fun _ ->
      let count = 10
      let providerDouble = 
        { new IBuildingServiceProvider with 
          member __.Get _ = dummyService |> List.replicate count |> Ok }
      let builder = { new IServiceResponseBuilder with 
                      member __.Success _ = dummyServiceWebObject
                      member __.CollectionError _ _ = failwith "" }

      let controller = new ServicesController(providerDouble, builder)

      let result = 
        controller.Get(0) 
        |> getOkResponseBody<ServiceWebObject list>

      test <@ List.length result = count @>

    "When an error occurs should return error response" ->? fun _ ->
        let providerDouble = 
          { new IBuildingServiceProvider with 
            member __.Get _ = Error Panic }
        let responseBuilderDouble = 
          stubResponseBuilderWithError 777 "error message"   
        let controller = 
          new ServicesController(providerDouble, responseBuilderDouble)
          
        let result = controller.Get(0) :?> ObjectResult
        
        test <@ result.StatusCode ?= 777 
             && string result.Value = "error message" @>
  ]