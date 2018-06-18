module Affordances
open Building
open Service

type Issues = 
  | CollectionOfIssues

type Services = 
  | CollectionOfServices
  | Service of ServiceId
  | ServiceIssues of ServiceId * Issues

type Buildings = 
  | CollectionOfBuildings
  | Building of BuildingId
  | BuildingServices of BuildingId * Services

type Affordance = 
  | Buildings of Buildings

let buildUri = function
  | CollectionOfBuildings -> "/buildings" 
  | Building(BuildingId(id)) -> sprintf "/buildings/%i" id
  | BuildingServices(BuildingId id,CollectionOfServices) -> 
      sprintf "/buildings/%i/services" id
  | BuildingServices(BuildingId bid, Service(ServiceId(sid))) -> 
      sprintf "/buildings/%i/services/%i" bid sid
  | BuildingServices(
                      BuildingId b, 
                      ServiceIssues(
                        ServiceId s, 
                        CollectionOfIssues)) ->
      sprintf "/buildings/%i/services/%i/issues" b s