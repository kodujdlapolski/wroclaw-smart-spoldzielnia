module ServiceTests
open Building
open Service
open Utils

[<Tests>]
let getServicessTests = 

  let dummyBuilding : Building = 
    { Id = BuildingId(1); Name = "building"; Description = "it's a buildingz" }

  let dummyService = 
    { Id = 1; 
      Name = "name"; 
      Description = "description" }  

  let mockRepository services = 
    fun _ -> Some(services|> Seq.ofList)  

  // let compareServices (Ok(sv1) : RetrievedServices,Ok(sv2) : RetrievedServices) = 
  //   let s1,s2 = sv1 |> Seq.head, sv2 |> Seq.head
  //   let {Service.Service.Id = s1Id; Service.Service.Description = s1desc; Service.Service.Name = s1Name; Service.Service.Building = s1b} = s1
  //   let {Service.Service.Id = s2Id; Service.Service.Description = s2desc; Service.Service.Name = s2Name; Service.Service.Building = s2b} = s2
  //   s1Id = s2Id && s1desc = s2desc && s1Name = s2Name && s1b = s2b

  "Retrieving services collection" =>? 
  [
    "Should provide services" ->? fun _ ->
      let buildingDomainService _ = Ok dummyBuilding
      let repo = mockRepository [dummyService]
      let act = getServicesForBuilding buildingDomainService repo

      let expectedService = 
        { Id = ServiceId(dummyService.Id);
          Name = dummyService.Name;
          Description = dummyService.Description;
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