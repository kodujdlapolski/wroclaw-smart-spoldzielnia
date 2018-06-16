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

  let getMockProvider buildings = 
    {new IBuildingsProvider with member __.Get() = buildings }

  let mockWebObjectBuilder webObject = 
    {new IResponseBuilder with member __.Build _ _ = webObject }  

  "BuildingsController retrieves buildings" =>? [

    ("Should return all found buildings" ->? fun _ ->
      let count = 10
      let providerDouble =
        dummyBuilding |> List.replicate count |> getMockProvider
      let jsonBuilderDouble = mockWebObjectBuilder buildingWebObject
      let controller = 
        new BuildingsController(providerDouble, jsonBuilderDouble)

      test <@ controller.Get() |> List.length = count @> )

    ("Should return json representations" ->? fun _ ->
      let providerDouble = [dummyBuilding] |> getMockProvider
      let jsonBuilderDouble = 
        mockWebObjectBuilder {buildingWebObject with Name = "this is building"}
      let controller = 
        new BuildingsController(providerDouble, jsonBuilderDouble)

      test <@ let x = controller.Get() |> List.head
              x.Name = "this is building" @>
    )
  ]