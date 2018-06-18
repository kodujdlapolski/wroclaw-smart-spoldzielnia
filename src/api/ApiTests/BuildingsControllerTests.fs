module BuildingsControllerTests

open Utils
open Api.Controllers
open Domain
open BuildingsWebObject
open FrameworkACL
open Microsoft.AspNetCore.Mvc

[<Tests>]
let buildingsControllerTests = 

  let dummyBuilding = 
    { 
      Id = 1; 
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
    let singleton = function
    | [] -> None
    | h::_ -> Some(h)

    { new IBuildingsProvider 
      with 
        member __.Get() = buildings
        member __.GetSingle _ = buildings |> singleton }

  let mockWebObjectBuilder webObject = 
    {new ICollectionBuildingAffordanceBuilder with member __.Build _ _ = webObject } 

  let webObjectBuilderStub = 
    {new ISingleBuildingAffordanceBuilder 
      with member __.Build _ b = 
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
          new BuildingsController(providerDouble, jsonBuilderDouble, webObjectBuilderStub)
        let result = 
          controller.Get().Value :?> BuildingWebObject list

        test <@ result |> List.length = count @> 

      "Should return json representations" ->? fun _ ->
        let providerDouble = [dummyBuilding] |> mockProvider
        let jsonBuilderDouble = 
          mockWebObjectBuilder { buildingWebObject 
                                 with Name = "this is building" }
        let controller = 
          new BuildingsController(providerDouble, jsonBuilderDouble, webObjectBuilderStub)
        let result = 
          controller.Get().Value :?> BuildingWebObject list

        test <@ let x = result |> List.head
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
          new BuildingsController(providerDouble, jsonBuilderDouble, webObjectBuilderStub)

        let result = controller.GetSingle(0) :?> OkObjectResult

        test <@ result.Value :?> BuildingWebObject = jsonBuilding @>

      "When building is not found should return 404 response" ->? fun _ ->
        
        let providerDouble = mockProvider []
        let dummyJsonBuilder = mockWebObjectBuilder buildingWebObject
        let controller =
          new BuildingsController(providerDouble, dummyJsonBuilder, webObjectBuilderStub)
        
        let result = controller.GetSingle(0)

        test <@ result :? NotFoundResult @>

      "Should build json representation from retrieved building" =>?
        
        let retrievedBuilding = 
          { dummyBuilding
            with 
              Name = "retrieved building Name";
              Description = "retrieved building Description";
              Id = 123
          }
        let providerDouble = mockProvider [retrievedBuilding]    
        let x = mockWebObjectBuilder buildingWebObject    
        let controller = 
          new BuildingsController(providerDouble, x, webObjectBuilderStub)
        let result = 
          (controller.GetSingle(0) :?> OkObjectResult).Value 
          :?> BuildingWebObject

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