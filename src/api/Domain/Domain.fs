module Domain

type Building = 
 {
   Id : int;
   Name : string;
   Description : string;
 }

 type GetBuildingById = int -> Building seq
 type GetBuilding = GetBuildingById -> int -> Building option

 let getBuilding : GetBuilding = 
  fun repository id -> 
    match repository id |> List.ofSeq with
    | [] -> None
    | x :: _ -> Some(x)