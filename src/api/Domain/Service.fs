module Service
open Building

type ServiceId = ServiceId of int

type ServiceDto = 
  {
    ServiceId : int;
    ServiceName : string;
    ServiceDescription : string;
  }

type Service = 
  {
    Id : ServiceId;
    Building : Building;
    Name : string;
    Description : string;
  }

type ServicesError = 
  | ServiceNotFound
  | BuildingNotFound
  | Consistency
  | Panic

type RetrievedServices = Result<Service list, ServicesError>
type ServicesRepository = BuildingId -> ServiceDto seq option

type GetServicesForBuilding = BuildingId -> RetrievedServices

let mapServices building dto = 
  {
    Id = ServiceId(dto.ServiceId);
    Building = building;
    Name = dto.ServiceName;
    Description = dto.ServiceDescription
  }

let getServicesForBuilding 
    : GetBuilding 
      -> (Building -> ServiceDto -> Service)
      -> ServicesRepository
      -> GetServicesForBuilding = 
    fun buildingDomainService convertToService servicesRepo buildingId -> 
      match buildingDomainService buildingId with
      | Ok(building) -> 
        match servicesRepo buildingId with
        | None -> Error Panic
        | Some serviceDtos -> 
          Ok (serviceDtos 
              |> Seq.map (convertToService building) 
              |> List.ofSeq)
      | Error NotFound -> Error BuildingNotFound
      | Error Building.Panic -> Error Panic
      | Error FoundDuplicate -> Error Consistency
