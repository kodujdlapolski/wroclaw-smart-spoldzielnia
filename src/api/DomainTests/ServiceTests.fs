module ServiceTests
open Building
open Service
open Utils

[<Tests>]
let getServicessTests = 

  let dummyBuilding : Building = 
    { Id = BuildingId(1); Name = "building"; Description = "it's a buildingz" }

  let dummyServiceDto = 
    { ServiceId = 1; 
      ServiceName = "name"; 
      ServiceDescription = "description" }  

  let mockRepository services = 
    fun _ -> Some(services|> Seq.ofList)  

  "Retrieving services collection" =>? 
  [
    "Should provide services" ->? fun _ ->
      let buildingDomainService _ = Ok dummyBuilding
      let repo = mockRepository [dummyServiceDto]
      let act = getServicesForBuilding buildingDomainService repo

      let expectedService = 
        { Id = ServiceId(dummyServiceDto.ServiceId);
          Name = dummyServiceDto.ServiceName;
          Description = dummyServiceDto.ServiceDescription;
          Building = dummyBuilding }
      test <@ act (BuildingId 1) =  Ok([expectedService]) @>

    "When no building found should return BuildingNotFound error" ->? fun _ ->
      let buildingDomainService _ = Error NotFound
      let repo = mockRepository []
      let act = getServicesForBuilding buildingDomainService repo

      test <@ act (BuildingId 1) = Error BuildingNotFound @>

    "When failed to retrieve building should return error" ->? fun _ ->
      let buildingDomainService _ = Error Building.Panic
      let repo = mockRepository []
      let act = getServicesForBuilding buildingDomainService repo

      test <@ act (BuildingId 1) = Error Panic @>

    "When failed to retrieve services should return error" ->? fun _ -> 
      let buildingDomainService _ = Ok dummyBuilding
      let repo _ = None
      let act = getServicesForBuilding buildingDomainService repo

      test <@ act (BuildingId 1) = Error Panic @>

    "When found two buildings should return consistency error" ->? fun _ -> 
      let buildingDomainService _ = Error FoundDuplicate
      let repo _ = None
      let act = getServicesForBuilding buildingDomainService repo

      test <@ act (BuildingId 1) = Error Consistency @>    
  ]