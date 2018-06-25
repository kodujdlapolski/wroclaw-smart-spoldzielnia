module BuildingResponse
open Building
open Affordances
open BuildingWebObject
open Links

let toBuildingError (BuildingId id) error = 
  match error with
  | NotFound -> (404, sprintf "Building %i was not found" id)
  | FoundDuplicate 
  | Panic -> (500, sprintf "There was an error in retrieving building %i" id)

let toBuildingsError _ = 
  (500, "There was an error in retrieving list of buildings" )

let toBuildingWebObject 
  uriBuilder
  { Building.Name = name;
    Building.Description = desc; 
    Building.Id = BuildingId(id) } =
    
  let toWebObject name desc id links = 
    { Name = name; 
      Description = desc; 
      Id = id |> string; 
      Links = links |> Map.ofList
    }
  let self = Building(BuildingId id)
  let services = BuildingServices(BuildingId id, CollectionOfServices)
  let links = [
            ("self", { Href = self |> uriBuilder; Templated = false });
            ("services", {Href = services |> uriBuilder; Templated = false})                   
            ]
  toWebObject name desc (string id) links

