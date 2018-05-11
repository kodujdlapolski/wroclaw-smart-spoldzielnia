module BuildingsControllerTests

open Utils
open Api.Controllers
open Models
open Providers 
open JsonResultBuilder
open Microsoft.AspNetCore.Mvc

[<Tests>]
let buildingsControllerTests = 

  let getMockProvider buildings = 
    {new IBuildingsProvider with member __.Get() = buildings }

  let getMockJsonBuilder json = 
    {new IJsonResultBuilder with member __.BuildJsonResult(x) = json }  

  let dummyBuilding = {  Id = 1; Name = "dummyName"; Description = "dummyDescription";}

  "BuildingsController retrieves buildings" =>? [
    
    ("Should return all found buildings" ->?
      let count = 10
      let providerDouble = dummyBuilding |> List.replicate count |> getMockProvider
      let jsonBuilderDouble = JsonResult("") |> getMockJsonBuilder 
      let controller = new BuildingsController(providerDouble, jsonBuilderDouble)
      
      test <@ controller.Get() |> List.length = count @> )

    ("Should return json representations" ->?
      let dummyJsonResult = JsonResult("this is building")
      let providerDouble = [dummyBuilding] |> getMockProvider
      let jsonBuilderDouble = dummyJsonResult |> getMockJsonBuilder
      let controller = new BuildingsController(providerDouble, jsonBuilderDouble)

      test <@ controller.Get() = [dummyJsonResult] @>
    )    
  ]