module FrameworkACL
open Domain

//Types here exist for the sole purpose to be registered in IServiceCollection

type IBuildingsProvider = 
  abstract member Get : unit -> Building list
  abstract member GetSingle : int -> Building option