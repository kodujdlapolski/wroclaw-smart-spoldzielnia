module BuildingErrorResponseTests

open Utils
open Building
open BuildingResponse

[<Tests>]
let singleBuildingErrorResponse = 
  let dummyBuildingId = BuildingId 5

  "Building error response for retrieving single building" =>?
  [
    "When building is not found" =>? 
    [
      "Should return 404 status code" ->? fun _ ->
        
        let result = toBuildingError dummyBuildingId NotFound

        test <@ fst result = 404 @>

      "Should return message that building could not be found" ->? fun _ ->
        
        let result = toBuildingError (BuildingId 3) NotFound

        test <@ snd result = "Building 3 was not found" @>
    ]

    "When two buildings were found" =>? 
    [
      "Should return 500 status code" ->? fun _ ->
        
        let result = toBuildingError dummyBuildingId FoundDuplicate

        test <@ fst result = 500 @>

      "Should return message about internal error" ->? fun _ ->
        
        let result = toBuildingError (BuildingId 3) FoundDuplicate

        test <@ snd result = "There was an error in retrieving building 3" @>
    ]
    
    "When provider returns panic error" =>? 
    [
      "Should return 500 status code" ->? fun _ ->
        
        let result = toBuildingError dummyBuildingId Panic

        test <@ fst result = 500 @>

      "Should return message about internal error" ->? fun _ ->
        
        let result = toBuildingError (BuildingId 3) Panic

        test <@ snd result = "There was an error in retrieving building 3" @>
    ]
  ]

[<Tests>]
let collectionOfBuildingsErrorResponse = 

  "Building error response for retrieving collection of buildings" =>?
  [
    "For any error returned" =>? 
    [
      "Should return 500 status code" ->? fun _ ->
        
        let result = toBuildingsError Panic

        test <@ fst result = 500 @>

      "Should return message about internal error" ->? fun _ ->
        
        let result = toBuildingsError Panic

        test <@ snd result = 
                  "There was an error in retrieving list of buildings" @>
    ]
  ]