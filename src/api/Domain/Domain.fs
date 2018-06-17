module Domain

type Building = 
 {
   Id : int;
   Name : string;
   Description : string;
 }

 type GetBuilding = int -> Building option
 type GetBuildingById = int -> Building seq
 type GetAllBuildings = unit -> Building list

 let getBuilding : GetBuildingById -> GetBuilding = 
  fun repository -> 
    fun id -> 
      match repository id |> List.ofSeq with
      | [] -> None
      | x :: _ -> Some(x)