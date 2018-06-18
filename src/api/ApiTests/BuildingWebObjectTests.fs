module BuildingWebObjectTests

open Utils
open Building
open BuildingsWebObject
open Affordances

[<Tests>]
let toWebObjectTests = 
  let dummyBuilding = 
    { Name = "dummyName"; 
      Description = "dummyDescription"; 
      Id = BuildingId(1)
    }

  let dummyUriBuilder _ = ""

  let act = buildWebObject dummyUriBuilder

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
      let building = {dummyBuilding with Id = BuildingId(777)}

      let result = act building

      test <@ result.Id = "777" @>
    
    "should add links" =>? 
    [
      "should add self link" ->? fun _ ->
        let result = act dummyBuilding

        test <@ 
             result.Links 
            |> Map.filter (fun k _ -> k = "self") 
            |> Map.count = 1 @>
      
      "self link should base on single building affordance" ->? fun _ ->
        
        let uriBuilderStub = function
        | Building(BuildingId _) -> "single building uri"
        | _ -> "other"

        let act = buildWebObject uriBuilderStub
        let building = {dummyBuilding with Id = BuildingId(42)}
        
        let selfLink = (act building).Links |> Map.find "self"

        test <@ selfLink.Href = "single building uri" @>

      "should add services link" ->? fun _ ->
        let result = act dummyBuilding

        test <@
             result.Links
             |> Map.filter (fun k _ -> k = "services")
             |> Map.count = 1
            @>
    ]
  ]