module ServiceResponse
open Building
open Service
open Affordances
open Links
open ServiceWebObject

let toServiceWebObject 
  uriBuilder 
  { Name = name;
    Description = description;
    Id = ServiceId(id);
    Building = building } =

  let self = BuildingServices(building.Id, Service(ServiceId id))
  let issues = BuildingServices(
                    building.Id, 
                    ServiceIssues(ServiceId id, CollectionOfIssues))
  let toWebObject links = 
    { Name = name; 
      Description = description; 
      Id = id |> string; 
      Links = links |> Map.ofList
    }
  let links = [
    ("self", { Href = self |> uriBuilder; Templated = false });
    ("issues", { Href = issues |> uriBuilder; Templated = false })
    ]
  toWebObject links

let toServicesError (BuildingId id) error = 
  match error with
  | BuildingNotFound -> (404,sprintf "Building %i does not exist" id)
  | Panic 
  | ServiceNotFound ->  
      (500,
       sprintf "There was an error in retrieving services for building %i" id )