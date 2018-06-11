module BuildingTests

open HttpFs.Client
open Utils
open Hopac
open Newtonsoft.Json
open YoLo

//this assumes that api is already running!
let apiUrl = "http://localhost:5000/api" 

[<CLIMutable>]
type BuildingRepresentation = 
  { Id : int; Name: string; Description : string }

[<CLIMutable>]
type Link = 
  { Href : string; Relation : string }

[<CLIMutable>]
type Resource = 
  { Links : Link array }

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

let settings = new JsonSerializerSettings(TypeNameHandling = TypeNameHandling.All)

let getLinkOfRelation relation resource = 
  resource.Links |> Array.find (fun r -> r.Relation = relation)

let followLink relationToFollow resource  = 
        let link = getLinkOfRelation relationToFollow resource
        getApiResponse link.Href

[<Tests>]
let building = 
  "retrieve building" =>? [
    ( "should return error message for non existing resource" ->? fun _ ->
      let result = getApiResponse (apiUrl + "/nonExisting") 

      test <@ result = {StatusCode = 404; Body = "Status Code: 404; Not Found"} @> )

    ( "should provide list of buildings" ->? fun _ ->
      let buildings = getApiResource<BuildingRepresentation array> (apiUrl + "/buildings")
      test <@ buildings.Length > 1 @> )

    ( "buildings should have self links" ->? fun _ ->
      let resources = getApiResource<Resource array>  (apiUrl + "/buildings")
      let {Resource.Links = links} = Array.head resources

      test <@ (links |> Array.find (fun link -> link.Relation = "self")).Href <> "" @>
    )

    ( "following building's self link should lead to an existing building resource" ->? fun _ ->
      let response = 
        getApiResource<Resource array> (apiUrl + "/buildings")
        |> Array.head
        |> followLink "self"

      test <@ response.StatusCode = 200 @>
    )

    ( "building resource should have name" ->? fun _ ->
      let firstBuildingLink = 
        getApiResource<Resource array> (apiUrl + "/buildings")
        |> Array.head
        |> getLinkOfRelation "self"

      let firstBuilding = getApiResource<BuildingRepresentation> firstBuildingLink.Href

      test <@ isNull firstBuilding.Name |> not @>
    )
  ]
