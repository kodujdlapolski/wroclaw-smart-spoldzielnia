module ServiceTests
open Building
open Service
open Utils

[<Tests>]
let getServicessTests = 

  let mockRepository services = 
    fun _ -> Some(services|> Seq.ofList)  

  "Retrieving services collection" =>? 
  [
    "Should provide services" ->? fun _ ->
      let buildingDomainService _ = Ok Fixture.dummyBuilding
      let repo = mockRepository [Fixture.dummyServiceDto]
      let expectedService = {Fixture.dummyService with Name = "expected name"}
      let converter _ _ = expectedService
      let act = getServicesForBuilding buildingDomainService converter repo

      test <@ act (BuildingId 1) =  Ok([expectedService]) @>

    "When no building found should return BuildingNotFound error" ->? fun _ ->
      let buildingDomainService _ = Error NotFound
      let repo = mockRepository []
      let converter _ _ = Fixture.dummyService
      let act = getServicesForBuilding buildingDomainService converter repo

      test <@ act (BuildingId 1) = Error BuildingNotFound @>

    "When failed to retrieve building should return error" ->? fun _ ->
      let buildingDomainService _ = Error Building.Panic
      let repo = mockRepository []
      let converter _ _ = Fixture.dummyService
      let act = getServicesForBuilding buildingDomainService converter repo

      test <@ act (BuildingId 1) = Error Panic @>

    "When failed to retrieve services should return error" ->? fun _ -> 
      let buildingDomainService _ = Ok Fixture.dummyBuilding
      let repo _ = None
      let converter _ _ = Fixture.dummyService
      let act = getServicesForBuilding buildingDomainService converter repo

      test <@ act (BuildingId 1) = Error Panic @>

    "When found two buildings should return panic error" ->? fun _ -> 
      let buildingDomainService _ = Error FoundDuplicate
      let repo _ = None
      let converter _ _ = Fixture.dummyService
      let act = getServicesForBuilding buildingDomainService converter repo

      test <@ act (BuildingId 1) = Error Panic @>    
  ]

[<Tests>]
let mapServiceTests = 

  "Mapping Service Dto to domain Service" =>? [
    
    "maps Name" ->? fun _ ->
      let serviceDto = 
        { Fixture.dummyServiceDto 
          with ServiceName = "service name" }

      let result = mapServices Fixture.dummyBuilding serviceDto
      test <@ result.Name = serviceDto.ServiceName @>

    "maps Description" ->? fun _ ->
      let serviceDto = 
        { Fixture.dummyServiceDto 
          with ServiceDescription = "service description" }

      let result = mapServices Fixture.dummyBuilding serviceDto
      test <@ result.Description = serviceDto.ServiceDescription @>

    "maps Id" ->? fun _ ->
      let serviceDto = 
        { Fixture.dummyServiceDto 
          with ServiceId = 6 }

      let result = mapServices Fixture.dummyBuilding serviceDto
      test <@ result.Id = ServiceId(serviceDto.ServiceId) @>
      
    "maps Building" ->? fun _ ->
      let building = 
        { Fixture.dummyBuilding with Name = "name" }

      let result = mapServices building Fixture.dummyServiceDto
      test <@ result.Building = building @>
  ]