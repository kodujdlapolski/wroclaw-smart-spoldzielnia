module Service

type ServiceId = ServiceId of int

type Service = 
  {
    Id : ServiceId;
    Name : string;
    Description : string;
  }