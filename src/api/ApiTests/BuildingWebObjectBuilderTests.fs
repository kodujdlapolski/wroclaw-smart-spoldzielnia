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

  let act = toWebObject dummyUrlProvider dummyRequest
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
            |> List.filter (fun l -> l.Relation = "self") 
            |> List.length = 1 @>
      
      "self link should have format /{id}" ->? fun _ ->
        
        let baseUrlProviderDouble _ = "buildingsResourceUrl"
        let act = toWebObject baseUrlProviderDouble dummyRequest
        let building = {dummyBuilding with Id = 42}
        
        let selfLink = 
          (act building).Links 
          |> List.find (fun l -> l.Relation = "self")

        test <@ selfLink.Href = "/42" @>
    ]
  ]