module BuildingsControllerTests

open Utils
open Api.Controllers
open Models
open Providers 
open BuildingsWebObject
open Microsoft.AspNetCore.Mvc
open Newtonsoft.Json

let deserialize<'a> x = 
  let settings = 
    let s = new JsonSerializerSettings()
    s.ContractResolver <- new Serialization.CamelCasePropertyNamesContractResolver()
    s
  JsonConvert.DeserializeObject<'a>(x, settings)

[<Tests>]
let buildingsControllerTests = 

  let dummyBuilding = {  Id = 1; Name = "dummyName"; Description = "dummyDescription";}
  let dummyBuildingWebObject = {Name = "dummyName"; Description = "dummyDescription"; Id = "id"; Links = []}

  let getMockProvider buildings = 
    {new IBuildingsProvider with member __.Get() = buildings }

  let getMockWebObjectBuilder webObjectt = 
    {new IResponseBuilder with member __.Build(x) = webObjectt }  



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

      test <@ let [x] = controller.Get()
              x.Name = "this is building" @>
    )    
  ]