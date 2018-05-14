module Providers
open Models

type IBuildingsProvider = 
  abstract member Get: unit -> Building list