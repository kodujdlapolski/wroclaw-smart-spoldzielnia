module Service
open Building

type ServiceId = ServiceId of int

type Service = 
  {
    Id : ServiceId;
    BuildingId : BuildingId;
    Name : string;
    Description : string;
  }

type ServicesError = 
  | ServiceNotFound
  | BuildingNotFound
  | Consistency
  | Panic

type RetrievedServices = Result<Service seq, ServicesError>
type ServicesRepository = BuildingId -> Service seq option

type GetServicesForBuilding = BuildingId -> RetrievedServices

let getServicesForBuilding 
    : (GetBuilding -> ServicesRepository
      -> GetServicesForBuilding) = 
    fun buildingDomainService servicesRepo buildingId -> 
      match buildingDomainService buildingId with
      | Ok(_) -> 
        match servicesRepo buildingId with
        | None -> Error Panic
        | Some(services) -> Ok services
      | Error NotFound -> Error BuildingNotFound
      | Error Building.Panic -> Error Panic
      | Error FoundDuplicate -> Error Consistency
