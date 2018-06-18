module Affordances

type BuildingId = BuildingId of int
type ServiceId = ServiceId of int

type Services = 
  | CollectionOfServices
  | Service of ServiceId

type Buildings = 
  | CollectionOfBuildings
  | Building of BuildingId
  | BuildingServices of BuildingId * Services

type Affordance = 
  | Buildings of Buildings

let buildUri = function
  | CollectionOfBuildings -> "/buildings" 
  | Building(BuildingId(id)) -> sprintf "/buildings/%i" id
  | BuildingServices(BuildingId(id),CollectionOfServices) -> 
      sprintf "/buildings/%i/services" id
  | BuildingServices(BuildingId(bid), Service(ServiceId(sid))) -> 
      sprintf "/buildings/%i/services/%i" bid sid