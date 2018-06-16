module FrameworkACL
open Domain

type IBuildingsProvider = 
  abstract member Get: unit -> Building list
  abstract member Get: int -> Building