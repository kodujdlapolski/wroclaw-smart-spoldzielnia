module BuildingsControllerTests

open Utils
open Api.Controllers
open Models
open Providers 
open BuildingsWebObject
open System.Net.Http

[<Tests>]
let buildingsControllerTests = 

  let dummyBuilding = {  Id = 1; Name = "dummyName"; Description = "dummyDescription";}
  let dummyBuildingWebObject = {Name = "dummyName"; Description = "dummyDescription"; Id = "id"; Links = []}

  let getMockProvider buildings = 
    {new IBuildingsProvider with member __.Get() = buildings }

  let getMockWebObjectBuilder webObject = 
    {new IResponseBuilder with member __.Build _ _ = webObject }  

  "BuildingsController retrieves buildings" =>? [

    ("Should return all found buildings" ->?
      let count = 10
      let providerDouble = dummyBuilding |> List.replicate count |> getMockProvider
      let jsonBuilderDouble = getMockWebObjectBuilder dummyBuildingWebObject
      let controller = new BuildingsController(providerDouble, jsonBuilderDouble)

      test <@ controller.Get() |> List.length = count @> )

    ("Should return json representations" ->?
      let providerDouble = [dummyBuilding] |> getMockProvider
      let jsonBuilderDouble = getMockWebObjectBuilder {dummyBuildingWebObject with Name = "this is building"}
      let controller = new BuildingsController(providerDouble, jsonBuilderDouble)

      test <@ let x = controller.Get() |> List.head
              x.Name = "this is building" @>
    )
  ]