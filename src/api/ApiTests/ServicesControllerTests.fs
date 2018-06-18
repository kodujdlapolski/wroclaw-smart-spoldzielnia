module ServicesControllerTests

open Utils
open Building
open Service
open Api.Controllers
open FrameworkACL


[<Tests>]
let servicesControllerTests = 
  
  "Should provide collection of services for building" =>?
  [
    "Should return all services" ->? fun _ ->
      test <@ 1 = 2 @>

  ]