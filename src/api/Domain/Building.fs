module Building

type BuildingId = BuildingId of int

type Building = 
  {
    Id : BuildingId;
    Name : string;
    Description : string;
  }

 type GetBuildingById = BuildingId -> Building seq
 type GetBuilding = GetBuildingById -> BuildingId -> Building option

 let getBuilding : GetBuilding = 
  fun repository id -> 
    match repository id |> List.ofSeq with
    | [] -> None
    | x :: _ -> Some(x)