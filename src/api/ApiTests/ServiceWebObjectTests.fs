module ServiceWebObjectTests

open Utils
open Building
open Service
open ServiceWebObject
open Affordances

[<Tests>]
let toWebObjectTests = 

  let dummyBuilding = 
    { 
      Name = "dummyName"; 
      Description = "dummyDescription"; 
      Id = BuildingId(1)
    }
  let dummyService = 
    { Name = "dummyName"; 
      Description = "dummyDescription"; 
      Id = ServiceId(2);
      Building = dummyBuilding
    }

  let dummyUriBuilder _ = ""

  let act = buildWebObject dummyUriBuilder

  "Creating Service Web Object" =>? [
    
    "should map Name" ->? fun _ ->
      let service = {dummyService with Name = "service Name"}
      
      let result = act service  
      
      test <@ result.Name = service.Name @>
    
    "should map Decription" ->? fun _ ->
      let service = {dummyService with Description = "building Description"}

      let result = act service

      test <@ result.Description = service.Description @>
        
    "should map Id" ->? fun _ ->
      let service = {dummyService with Id = ServiceId(777)}

      let result = act service

      test <@ result.Id = "777" @>
    
    "should add links" =>? 
    [
      "should add self link" ->? fun _ ->
        let result = act dummyService

        test <@ 
             result.Links 
            |> Map.filter (fun k _ -> k = "self") 
            |> Map.count = 1 @>
    ]
  ]