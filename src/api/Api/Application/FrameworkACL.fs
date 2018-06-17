module FrameworkACL
open Domain

type IBuildingsProvider = 
  abstract member Get : GetAllBuildings
  abstract member GetSingle : GetBuilding