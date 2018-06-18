module Service
open Building

type ServiceId = ServiceId of int

type Service = 
  {
    Id : ServiceId;
    Name : string;
    Description : string;
  }

type GetServicesForBuilding = BuildingId -> Service list