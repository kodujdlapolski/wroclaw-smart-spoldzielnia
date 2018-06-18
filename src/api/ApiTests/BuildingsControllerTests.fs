module BuildingsControllerTests

open Utils
open Api.Controllers
open Building
open BuildingsWebObject
open FrameworkACL
open Microsoft.AspNetCore.Mvc

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

  let mockProvider buildings = 
    let singleton (b : Building list) = 
      match b with
      | [] -> Error NotFound
      | h::_ -> Ok h

    { new IBuildingsProvider 
      with 
        member __.Get() = buildings |> Seq.ofList |> Ok
        member __.GetSingle _ = buildings |> singleton }

  let mockWebObjectBuilder webObject = 
    {new IBuildingResponseBuilder with member __.Build _ = webObject } 

  let webObjectBuilderStub = 
    {new IBuildingResponseBuilder 
      with member __.Build b = 
            { Name = b.Name; 
              Description = b.Description;
              Id = string b.Id;
              Links = Map.empty}
    }

  "BuildingsController" =>? 
  [
    
    "Collection of buildings" =>? [

      "Should return all found buildings" ->? fun _ ->
        let count = 10
        let providerDouble =
          dummyBuilding |> List.replicate count |> mockProvider
        let jsonBuilderDouble = mockWebObjectBuilder buildingWebObject
        let controller = 
          new BuildingsController(providerDouble, jsonBuilderDouble)
        let result = 
          controller.Get() |> getOkResponseBody<BuildingWebObject seq>

        test <@ result |> Seq.length = count @> 

      "Should return json representations" ->? fun _ ->
        let providerDouble = [dummyBuilding] |> mockProvider
        let jsonBuilderDouble = 
          mockWebObjectBuilder { buildingWebObject 
                                 with Name = "this is building" }
        let controller = 
          new BuildingsController(providerDouble, jsonBuilderDouble)
        let result = 
          controller.Get() |> getOkResponseBody<BuildingWebObject seq>

        test <@ let x = result |> Seq.head
                x.Name = "this is building" @>
    ]

    "Single building" =>? 
    [
      "Should return json representation" ->? fun _ ->
        let providerDouble = mockProvider [dummyBuilding]
        let jsonBuilding = 
          { buildingWebObject
            with 
              Name = "single building Name";
              Description = "single building Description";
              Id = "777"
          }
        let jsonBuilderDouble = jsonBuilding |> mockWebObjectBuilder
        let controller =
          new BuildingsController(providerDouble, jsonBuilderDouble)

        let result = controller.GetSingle(0) :?> OkObjectResult

        test <@ result.Value :?> BuildingWebObject = jsonBuilding @>

      "When building is not found should return 404 response" ->? fun _ ->
        
        let providerDouble = mockProvider []
        let dummyJsonBuilder = mockWebObjectBuilder buildingWebObject
        let controller =
          new BuildingsController(providerDouble, dummyJsonBuilder)
        
        let result = controller.GetSingle(0)

        test <@ result :? NotFoundResult @>

      "When two buildings are found should return 500 response" ->? fun _ ->
        let providerDouble = 
          {new IBuildingsProvider with
            member __.GetSingle(_) = Error FoundDuplicate 
            member __.Get() = failwith ""
          }
        let dummyJsonBuilder = mockWebObjectBuilder buildingWebObject
        let controller =
          new BuildingsController(providerDouble, dummyJsonBuilder)
        
        let result = controller.GetSingle(0) :?> StatusCodeResult

        test <@ result.StatusCode = 500 @>

      "When provider panics should return 500 response" ->? fun _ ->
        let providerDouble = 
          {new IBuildingsProvider with
            member __.GetSingle(_) = Error Panic 
            member __.Get() = failwith ""
          }
        let dummyJsonBuilder = mockWebObjectBuilder buildingWebObject
        let controller =
          new BuildingsController(providerDouble, dummyJsonBuilder)
        
        let result = controller.GetSingle(0) :?> StatusCodeResult

        test <@ result.StatusCode = 500 @>

      "Should build json representation from retrieved building" =>?
        
        let retrievedBuilding = 
          { dummyBuilding
            with 
              Name = "retrieved building Name";
              Description = "retrieved building Description";
              Id = BuildingId(123)
          }
        let providerDouble = mockProvider [retrievedBuilding]    
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