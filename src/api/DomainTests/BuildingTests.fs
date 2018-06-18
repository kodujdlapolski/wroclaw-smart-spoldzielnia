module BuildingTests
open Building
open Utils

[<Tests>]
let getBuildingTests = 
  let mockRepository buildings = 
        fun _ -> Some(buildings |> Seq.ofList)

  let dummyBuilding = 
    { Id = BuildingId(1); Name = "name"; Description = "description" }
    
  "Retrieving building" =>? 
  [
    "Should provide building" ->? fun _ ->
      let building = {dummyBuilding with Name = "building from repo"}
      let repo = mockRepository [building]
      
      test <@ getBuilding repo (BuildingId 1) = Ok(building) @>

    "When no building found should return error" ->? fun _ ->
      let repo = mockRepository []
      
      test <@ getBuilding repo (BuildingId 123) = Error NotFound @>

    "When two buildings found should return error" ->? fun _ ->
      let a = {dummyBuilding with Name = "Building A"}
      let b = {dummyBuilding with Name = "Building B"}
      let repo = mockRepository [a;b]

      test <@ getBuilding repo (BuildingId 7) = Error FoundDuplicate @>

    "When repository fails to retrieve should return error" ->? fun _ ->
      let repo (BuildingId(_)) : Building seq option = None

      test <@ getBuilding repo (BuildingId 1) = Error Panic @>

  ]

[<Tests>]
let getBuildingsTests = 
  let mockRepository buildings = 
        fun () -> Some(buildings |> Seq.ofList)

  let dummyBuilding = 
    { Id = BuildingId(1); Name = "name"; Description = "description" }

  "Retrieving building collection" =>? 
  [
    "Should provide buildings" ->? fun _ ->
      let building = {dummyBuilding with Name = "building from repo"}
      let repo = mockRepository [building]
      
      test <@ getAllBuildings repo () = Ok([building]|> Seq.ofList) @>

    "When no building found should return empty sequence" ->? fun _ ->
      let repo = mockRepository []
      
      test <@ getAllBuildings repo () = Ok([] |> Seq.ofList) @>

    "When failed to retrieve buildings should return error" ->? fun _ ->
      let repo () : Building seq option = None

      test <@ getAllBuildings repo () = Error Panic @>
  ]