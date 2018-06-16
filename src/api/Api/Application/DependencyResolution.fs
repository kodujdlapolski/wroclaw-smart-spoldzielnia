module DependencyResolution

open Microsoft.Extensions.DependencyInjection
open FrameworkACL
open BuildingsWebObject
open DataAccess
open ConnectionString

let registerServices (kernel: IServiceCollection) = 
  kernel
    .AddTransient<IBuildingsProvider>
      (fun _ -> 
        { new IBuildingsProvider 
          with member __.Get() = getBuildings connectionString () 
        }) |> ignore

  kernel
    .AddTransient<IResponseBuilder>
      (fun _ -> 
        { new IResponseBuilder 
          with member __.Build r b = toWebObject urlProvider r b 
          }) |> ignore
  ()