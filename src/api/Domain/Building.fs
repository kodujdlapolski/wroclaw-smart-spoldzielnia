module Building

type BuildingId = BuildingId of int

type Building = 
  {
    Id : BuildingId;
    Name : string;
    Description : string;
  }
 
type BuildingError = 
  | NotFound
  | FoundDuplicate
  | Panic

type RetrievedBuilding = Result<Building,BuildingError>
type RetrievedBuildings = Result<Building seq,BuildingError>

type BuildingRepository = unit -> Building seq option
type SingleBuildingRepository = BuildingId -> Building seq option
type GetBuilding = SingleBuildingRepository -> BuildingId -> RetrievedBuilding
type GetAllBuildings = BuildingRepository -> unit -> RetrievedBuildings

let getAllBuildings : GetAllBuildings = 
  fun repository () -> 
    match repository() with
    | None -> Error Panic
    | Some(buildings) -> Ok buildings

let getBuilding : GetBuilding = 
  fun repository id -> 
    match repository id with
    | None -> Error Panic
    | Some(buildings) ->
      match buildings |> List.ofSeq with 
      | [] -> Error NotFound
      | _::_::_ -> Error FoundDuplicate
      | [building] -> Ok building