module ServiceErrorResponseTests

open Utils
open Building
open Service
open ServiceResponse

[<Tests>]
let collectionOfServicesErrorResponse = 
  let dummyBuildingId = BuildingId 5

  "Service error response for retrieving list of services for building" =>?
  [
    "When building is not found" =>? 
    [
      "Should return 404 status code" ->? fun _ ->
        
        let result = toServicesError dummyBuildingId BuildingNotFound

        test <@ fst result = 404 @>

      "Should return message that building could not be found" ->? fun _ ->
        
        let result = toServicesError (BuildingId 3) BuildingNotFound

        test <@ snd result = "Building 3 does not exist" @>
    ]

    "When panic error occurs" =>? 
    [
      "Should return 500 status code" ->? fun _ ->
        
        let result = toServicesError dummyBuildingId Panic

        test <@ fst result = 500 @>

      "Should return message about internal error" ->? fun _ ->
        
        let result = toServicesError (BuildingId 3) Panic

        test <@ snd result = 
            "There was an error in retrieving services for building 3" @>
    ]
  ]