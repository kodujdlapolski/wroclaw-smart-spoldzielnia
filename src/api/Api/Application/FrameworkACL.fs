module FrameworkACL
open Domain

type IBuildingsProvider = 
  abstract member Get : GetAllBuilding
  abstract member GetSingle : GetBuilding