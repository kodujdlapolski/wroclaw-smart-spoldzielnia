module BuildingTests

open HttpFs.Client
open Utils
open Hopac
open Newtonsoft.Json
open YoLo

//this assumes that api is already running!
let apiUrl = "http://localhost:5000" 

[<CLIMutable>]
type BuildingRepresentation = 
  { Id : string; Name: string; Description : string }

[<CLIMutable>]
type ServiceRepresentation = 
  { Id : string; Name: string; Description : string }

[<CLIMutable>]
type Link = 
  { Href : string; Templated : bool }

[<CLIMutable>]
[<JsonObject>]
type Resource = 
  { 
  [<JsonProperty("_links")>]
  Links : Map<string,Link>
  }

type Response = {StatusCode: int; Body : string}

let getApiResponse url =
  let request = Request.createUrl Get url
  job {
    use! response = getResponse request
    let code = response.statusCode
    let! body = Response.readBodyAsString response
    return {StatusCode = code; Body = (body |> String.trim)}
  }
  |> Job.toAsync
  |> Async.RunSynchronously

let getApiResource<'a> url = 
  let response = getApiResponse url
  JsonConvert.DeserializeObject<'a>(response.Body)

let deserializeResponse<'a> response = 
  JsonConvert.DeserializeObject<'a>(response.Body)

let getLinkOfRelation relation resource = 
  resource.Links |> Map.find relation

let followLink relationToFollow resource  = 
        let link = getLinkOfRelation relationToFollow resource
        getApiResponse (apiUrl + link.Href)

[<Tests>]
let building = 
  "retrieve building" =>? [
    "should return error message for non existing resource" ->? fun _ ->
    
      let result = getApiResponse (apiUrl + "/nonExisting") 

      test <@ result = 
             { StatusCode = 404; 
               Body = "Status Code: 404; Not Found"} @> 

    "should provide list of buildings" ->? fun _ ->

      let buildings = 
        getApiResource<BuildingRepresentation array> (apiUrl + "/buildings")

      test <@ buildings.Length > 1 @> 

    "buildings should have self links" ->? fun _ ->
     
      let resources = getApiResource<Resource array>  (apiUrl + "/buildings")
      let {Resource.Links = links} = Array.head resources

      test <@ ( links |> Map.find "self").Href <> "" @>

    "following building self link leads to an existing building resource" ->?
    
      fun _ ->
        let response = 
          getApiResource<Resource array> (apiUrl + "/buildings")
          |> Array.head
          |> followLink "self"

        test <@ response.StatusCode = 200 @>

    "buildings should have services link" ->? fun _ ->

      let resources = getApiResource<Resource array>  (apiUrl + "/buildings")
      let {Resource.Links = links} = Array.head resources

      test <@ (links |> Map.find "services").Href <> "" @>

    "following services link leads to an exising resource" ->?
      fun _ ->
        let response = 
          getApiResource<Resource array> (apiUrl + "/buildings")
          |> Array.head
          |> followLink "services"

        test <@ response.StatusCode = 200 @>

    "following services link leads list of services for the building" ->?  
      fun _ ->
        let response = 
          getApiResource<Resource array> (apiUrl + "/buildings")
          |> Array.head
          |> followLink "services"
        let services = deserializeResponse<ServiceRepresentation array> response

        test <@ services.Length > 1 @>      

    "building resource should have name" ->? fun _ ->
    
      let buildingLink = 
        getApiResource<Resource array> (apiUrl + "/buildings")
        |> Array.head
        |> getLinkOfRelation "self"

      let firstBuilding =
          getApiResource<BuildingRepresentation> (apiUrl + buildingLink.Href)

      test <@ isNull firstBuilding.Name |> not @>
  ]
