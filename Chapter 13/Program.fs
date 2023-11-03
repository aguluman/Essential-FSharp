open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Giraffe
open Giraffe.EndpointRouting
open Giraffe.ViewEngine
open Giraffe.HttpStatusCodeHandlers


let sayHelloNameHandler (name: string) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        (*task {
            let msg = $"Hello {name}, how are you?"
            return! json {| Response = msg |} next ctx
        }*)
        {| Response = $"Hello {name}, how are you?" |} |> ctx.WriteJsonAsync

let endpoints =
    [ GET
          [ route "/" (text "Hello World from the Giraffe Library and the world of API building with FSharp")
            route "/api" (json {| Response = "Hello World!!!" |})
            routef "/api%s" sayHelloNameHandler ] ]

let notFoundHandler = "Not Found" |> text |> RequestErrors.notFound

let configureApp (appBuilder: IApplicationBuilder) =
    appBuilder.UseRouting().UseGiraffe(endpoints).UseGiraffe(notFoundHandler)

let configureServices (services: IServiceCollection) =
    services.AddRouting().AddGiraffe() |> ignore

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)
    configureServices builder.Services

    let app = builder.Build()

    if app.Environment.IsDevelopment() then
        app.UseDeveloperExceptionPage() |> ignore

    configureApp app
    app.Run()

    0 // Exit code
