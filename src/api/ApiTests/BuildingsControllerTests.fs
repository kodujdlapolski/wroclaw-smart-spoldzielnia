module BuildingsControllerTests

open Utils
open Api.Controllers
open Building
open BuildingWebObject
open FrameworkACL
open Microsoft.AspNetCore.Mvc
open Microsoft.FSharp.Linq.NullableOperators

let getOkResponseBody<'a> (resp : IActionResult) = 
    let okResp = resp :?> OkObjectResult
    okResp.Value :?> 'a

[<Tests>]
let buildingsControllerTests = 

  let dummyBuilding = 
    { 
      Id = BuildingId(1); 
      Name = "dummyName"; 
      Description = "dummyDescription";
    }

  let buildingWebObject = 
    {
      Name = "dummyName"; 
      Description = "dummyDescription"; 
      Id = "id"; 
      Links = Map.empty
    }

  let stubProvider buildings = 
    let singleton (b : Building list) = 
      match b with
      | [] -> Error NotFound
      | h::_ -> Ok h
    { new IBuildingsProvider 
      with 
        member __.Get() = buildings |> Seq.ofList |> Ok
        member __.GetSingle _ = buildings |> singleton }

  let stubResponseBuilder webObject = 
    {new IBuildingResponseBuilder with 
     member __.Success _ = webObject
     member __.Error _ _ = failwith ""
     member __.CollectionError _ = failwith "" }

  let stubResponseBuilderWithError code msg = 
    {new IBuildingResponseBuilder with 
     member __.Success _ = failwith ""
     member __.Error _ _ = code, msg
     member __.CollectionError _ = code, msg }   

  let webObjectBuilderStub = 
    {new IBuildingResponseBuilder with 
     member __.Success b = 
            { Name = b.Name; 
              Description = b.Description;
              Id = string b.Id;
              Links = Map.empty}
     member __.Error _ _ = failwith ""
     member __.CollectionError _ = failwith ""        
    }

  "BuildingsController" =>? 
  [
    
    "Collection of buildings" =>? [

      "Should return all found buildings" ->? fun _ ->
        let count = 10
        let providerDouble =
          dummyBuilding |> List.replicate count |> stubProvider
        let responseBuilderDouble = stubResponseBuilder buildingWebObject
        let controller = 
          new BuildingsController(providerDouble, responseBuilderDouble)
        let result = 
          controller.Get() |> getOkResponseBody<BuildingWebObject seq>

        test <@ result |> Seq.length = count @> 

      "Should return building's representations" ->? fun _ ->
        let providerDouble = [dummyBuilding] |> stubProvider
        let buildingRepresentation = 
            { buildingWebObject with Name = "this is building" }
        let responseBuilderDouble = stubResponseBuilder buildingRepresentation
        let controller = 
          new BuildingsController(providerDouble, responseBuilderDouble)

        let result = 
          controller.Get() |> getOkResponseBody<BuildingWebObject seq>

        test <@ result |> Seq.head = buildingRepresentation @>

      "When an error occurs should return error response" ->? fun _ ->
        let providerDouble = 
          { new IBuildingsProvider with 
            member __.Get() = Error Panic
            member __.GetSingle(_) = Error Panic }
        let responseBuilderDouble = 
          stubResponseBuilderWithError 777 "error message"   
        let controller = 
          new BuildingsController(providerDouble, responseBuilderDouble)               

        let result = controller.Get() :?> ObjectResult
        
        test <@ result.StatusCode ?= 777 
             && string result.Value = "error message" @>

    ]

    "Single building" =>? 
    [
      "Should return json representation" ->? fun _ ->
        let providerDouble = stubProvider [dummyBuilding]
        let jsonBuilding = 
          { buildingWebObject
            with 
              Name = "single building Name";
              Description = "single building Description";
              Id = "777"
          }
        let jsonBuilderDouble = jsonBuilding |> stubResponseBuilder
        let controller =
          new BuildingsController(providerDouble, jsonBuilderDouble)

        let result = controller.GetSingle(0) :?> OkObjectResult

        test <@ result.Value :?> BuildingWebObject = jsonBuilding @>

      "When an error occurs should return error response" ->? fun _ ->
        let providerDouble = 
          { new IBuildingsProvider with 
            member __.Get() = Error Panic
            member __.GetSingle(_) = Error Panic }

        let responseBuilderDouble = 
          stubResponseBuilderWithError 888 "error message"   
        let controller = 
          new BuildingsController(providerDouble, responseBuilderDouble)               

        let result = controller.GetSingle(2) :?> ObjectResult
        
        test <@ result.StatusCode ?= 888 
             && string result.Value = "error message" @>

      // "When building is not found should return 404 response" ->? fun _ ->
        
      //   let providerDouble = stubProvider []
      //   let dummyJsonBuilder = stubResponseBuilder buildingWebObject
      //   let controller =
      //     new BuildingsController(providerDouble, dummyJsonBuilder)
        
      //   let result = controller.GetSingle(0)

      //   test <@ result :? NotFoundResult @>

      // "When two buildings are found should return 500 response" ->? fun _ ->
      //   let providerDouble = 
      //     {new IBuildingsProvider with
      //       member __.GetSingle(_) = Error FoundDuplicate 
      //       member __.Get() = failwith ""
      //     }
      //   let dummyJsonBuilder = stubResponseBuilder buildingWebObject
      //   let controller =
      //     new BuildingsController(providerDouble, dummyJsonBuilder)
        
      //   let result = controller.GetSingle(0) :?> StatusCodeResult

      //   test <@ result.StatusCode = 500 @>

      // "When provider panics should return 500 response" ->? fun _ ->
      //   let providerDouble = 
      //     {new IBuildingsProvider with
      //       member __.GetSingle(_) = Error Panic 
      //       member __.Get() = failwith ""
      //     }
      //   let dummyJsonBuilder = stubResponseBuilder buildingWebObject
      //   let controller =
      //     new BuildingsController(providerDouble, dummyJsonBuilder)
        
      //   let result = controller.GetSingle(0) :?> StatusCodeResult

      //   test <@ result.StatusCode = 500 @>

      "Should build json representation from retrieved building" =>?
        
        let retrievedBuilding = 
          { dummyBuilding
            with 
              Name = "retrieved building Name";
              Description = "retrieved building Description";
              Id = BuildingId(123)
          }
        let providerDouble = stubProvider [retrievedBuilding]    
        let controller = 
          new BuildingsController(providerDouble, webObjectBuilderStub)
        let result = 
          controller.GetSingle(0) |> getOkResponseBody<BuildingWebObject>

        [
          "Should map Description" ->? fun _ ->
            test <@ result.Description = retrievedBuilding.Description @>

          "Should map Name" ->? fun _ ->
            test <@ result.Name = retrievedBuilding.Name @>

          "Should map Id" ->? fun _ ->
            test <@ result.Name = retrievedBuilding.Name @>          
        ]
    ]
  ]