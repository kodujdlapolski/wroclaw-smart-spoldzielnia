module JsonResultBuilder
open Microsoft.AspNetCore.Mvc

type IJsonResultBuilder = 
  abstract member BuildJsonResult : 'a -> JsonResult

let toJson x = JsonResult(x)