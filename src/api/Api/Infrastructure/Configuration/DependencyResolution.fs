module DependencyResolution

open Microsoft.Extensions.DependencyInjection
open Providers
open DataAccess
open ConnectionString
open JsonResultBuilder

let registerServices (kernel: IServiceCollection) = 
  kernel.AddTransient<IBuildingsProvider>(fun _ -> {new IBuildingsProvider with member __.Get() = getBuildings connectionString () }) |> ignore
  kernel.AddTransient<IJsonResultBuilder>(fun _ -> {new IJsonResultBuilder with member __.BuildJsonResult(x) = toJson x }) |> ignore
  ()