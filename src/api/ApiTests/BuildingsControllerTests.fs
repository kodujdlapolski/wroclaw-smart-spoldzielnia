module BuildingsControllerTests

open Utils
open Api.Controllers
open Domain
open BuildingsWebObject
open FrameworkACL

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
      Links = []
    }

  let mockProvider buildings = 
    { new IBuildingsProvider 
      with 
        member __.Get() = buildings
        member __.Get(_) = buildings |> Seq.head }

  let mockWebObjectBuilder webObject = 
    {new IResponseBuilder with member __.Build _ _ = webObject } 

  let webObjectBuilderStub = 
    {new IResponseBuilder 
      with member __.Build _ b = 
            { Name = b.Name; 
              Description = b.Description;
              Id = string b.Id;
              Links = []}
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

        test <@ controller.Get() |> List.length = count @> 

      "Should return json representations" ->? fun _ ->
        let providerDouble = [dummyBuilding] |> mockProvider
        let jsonBuilderDouble = 
          mockWebObjectBuilder { buildingWebObject 
                                 with Name = "this is building" }
        let controller = 
          new BuildingsController(providerDouble, jsonBuilderDouble)

        test <@ let x = controller.Get() |> List.head
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

        test <@ controller.GetSingle(0) = jsonBuilding @>

      "Should build json representation from retrieved building" =>?
        
        let retrievedBuilding = 
          { dummyBuilding
            with 
              Name = "retrieved building Name";
              Description = "retrieved building Description";
              Id = 123
          }
        let providerDouble = mockProvider [retrievedBuilding]        
        let controller = 
          new BuildingsController(providerDouble, webObjectBuilderStub)

        [
          "Should map Description" ->? fun _ ->
            let result = controller.GetSingle(7)

            test <@ result.Description = retrievedBuilding.Description @>

          "Should map Name" ->? fun _ ->
            let result = controller.GetSingle(7)

            test <@ result.Name = retrievedBuilding.Name @>

          "Should map Id" ->? fun _ ->
            let result = controller.GetSingle(7)

            test <@ result.Name = retrievedBuilding.Name @>          
        ]
      
    ]
  ]