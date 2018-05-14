module BuildingWebObjectBuilderTests

open Utils
open Models
open BuildingsWebObject
open System.Net.Http
open BuildingsWebObject

[<Tests>]
let toWebObjectTests = 
  let dummyBuilding = {Name = "dummyName"; Description = "dummyDescription"; Id = 1}
  let dummyUrlProvider str = ""
  let dummyRequest = new HttpRequestMessage(HttpMethod.Get, "")

  let act = toWebObject dummyUrlProvider dummyRequest
  "Creating Building Web Object" =>? [
    
    ("should map Name" ->?
      let building = {dummyBuilding with Name = "building Name"}
      
      let result = act building  
      
      test <@ result.Name = building.Name @>
    )

    ("should map Decription" ->?
      let building = {dummyBuilding with Description = "building Description"}

      let result = act building

      test <@ result.Description = building.Description @>
    )
    
    ("should map Id" ->?
      let building = {dummyBuilding with Id = 777}

      let result = act building

      test <@ result.Id = string building.Id @>
    )

    ("should add links" =>? [
      
      ("should add self link" ->?
        let result = act dummyBuilding

        test <@ result.Links |> List.filter (fun l -> l.Relation = "self") |> List.length = 1 @>
      )

      ("self link should have format {buildingsResourceUrl}/{id}" ->? 
        
        let baseUrlProviderDouble x = "buildingsResourceUrl"
        let act = toWebObject baseUrlProviderDouble dummyRequest
        let building = {dummyBuilding with Id = 42}
        
        let selfLink = (act building).Links |> List.find (fun l -> l.Relation = "self")

        test <@ selfLink.Href = "buildingsResourceUrl/42" @>
      )
    ]
    )
  ]