module BuildingWebObjectBuilderTests

open Utils
open Domain
open BuildingsWebObject
open System.Net.Http

[<Tests>]
let toWebObjectTests = 
  let dummyBuilding = 
    { Name = "dummyName"; 
      Description = "dummyDescription"; 
      Id = 1
    }

  let dummyUrlProvider _ = ""
  let dummyRequest = new HttpRequestMessage(HttpMethod.Get, "")

  let act = collectionBuildingAffordances dummyUrlProvider dummyRequest
  "Creating Building Web Object" =>? [
    
    "should map Name" ->? fun _ ->
      let building = {dummyBuilding with Name = "building Name"}
      
      let result = act building  
      
      test <@ result.Name = building.Name @>
    
    "should map Decription" ->? fun _ ->
      let building = {dummyBuilding with Description = "building Description"}

      let result = act building

      test <@ result.Description = building.Description @>
        
    "should map Id" ->? fun _ ->
      let building = {dummyBuilding with Id = 777}

      let result = act building

      test <@ result.Id = string building.Id @>
    
    "should add links" =>? 
    [
      "should add self link" ->? fun _ ->
        let result = act dummyBuilding

        test <@ 
             result.Links 
            |> Map.filter (fun k _ -> k = "self") 
            |> Map.count = 1 @>
      
      "self link should have format /buildingsRelativeUrl/{id}" ->? fun _ ->
        
        let baseUrlProviderDouble _ = "buildingsRelativeUrl"
        let act = collectionBuildingAffordances baseUrlProviderDouble dummyRequest
        let building = {dummyBuilding with Id = 42}
        
        let selfLink = 
          (act building).Links 
          |> Map.find "self"

        test <@ selfLink.Href = "/buildingsRelativeUrl/42" @>
    ]
  ]