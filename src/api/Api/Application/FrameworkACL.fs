module FrameworkACL
open Domain

type IBuildingsProvider = 
  abstract member Get: unit -> Building list