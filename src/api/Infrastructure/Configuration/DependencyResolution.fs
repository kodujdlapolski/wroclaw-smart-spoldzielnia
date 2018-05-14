module DependencyResolution

open Microsoft.Extensions.DependencyInjection
open Providers
open DataAccess
open ConnectionString
open BuildingsWebObject

let registerServices (kernel: IServiceCollection) = 
  kernel.AddTransient<IBuildingsProvider>(fun _ -> {new IBuildingsProvider with member __.Get() = getBuildings connectionString () }) |> ignore
  kernel.AddTransient<IResponseBuilder>(fun _ -> {new IResponseBuilder with member __.Build(x) = toWebObject x }) |> ignore
  ()