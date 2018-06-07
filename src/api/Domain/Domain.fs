module Domain

type Building = 
 {
   Id : int;
   Name : string;
   Description : string;
 }

 type GetBuilding = int -> Building