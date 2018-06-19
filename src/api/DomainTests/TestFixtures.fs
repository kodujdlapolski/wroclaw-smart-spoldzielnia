module Fixture
open Service
open Building


let dummyServiceDto = 
    { ServiceId = 1; 
      ServiceName = "name"; 
      ServiceDescription = "description" }

let dummyBuilding : Building = 
  { Id = BuildingId(1); 
    Name = "building";
    Description = "it's a buildingz" }

let dummyService = 
  { Id = ServiceId 0;
    Name = "dummy name";
    Description = "dummy desc";
    Building = dummyBuilding }