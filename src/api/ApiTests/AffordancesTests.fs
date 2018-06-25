module AffordancesTests

open Affordances
open Utils
open Service
open Building

[<Tests>]
let creatingUrisToAffordances = 
  "Creates correct uris to Api affordances" =>?
  [
    "Creates uri for collection of buildings" ->? fun _ ->
      let affordance = CollectionOfBuildings
      
      test <@ buildUri affordance = "/buildings" @>

    "Creates uri for single building" ->? fun _ -> 
      let affordance = Building(BuildingId 7)
      
      test <@ buildUri affordance = "/buildings/7" @>

    "Creates uri for buildings services" ->? fun _ ->
      let affordance = BuildingServices(BuildingId 8, CollectionOfServices)

      test <@ buildUri affordance = "/buildings/8/services" @>

    "Creates uri for specific service in building" ->? fun _ ->
      let affordance = 
        BuildingServices(BuildingId 9, Service(ServiceId 2))

      test <@ buildUri affordance = "/buildings/9/services/2" @>

    "Creates uri for issues in specific service" ->? fun _ ->
      let affordance = 
        BuildingServices(
          BuildingId 7, 
          ServiceIssues (ServiceId 6, CollectionOfIssues))

      test <@ buildUri affordance = "/buildings/7/services/6/issues" @>    
  ]
