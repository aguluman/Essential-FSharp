module Chapter_13.Todos

open System
open System.Collections.Generic
open Chapter_13.TodoStore
open Microsoft.AspNetCore.Http
open Giraffe
open Giraffe.EndpointRouting

module Handlers =

    let viewTodos =
        fun (_: HttpFunc) (ctx: HttpContext) ->
            let store = ctx.GetService<TodoStore>()
            store.GetAll() |> ctx.WriteJsonAsync


    let viewTodo (id: Guid) =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let store = ctx.GetService<TodoStore>()

                return!
                    (match store.Get(id) with
                     | Some todo -> json todo
                     | None -> RequestErrors.NOT_FOUND "Not Found")
                        next
                        ctx
            }

    let createTodo =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let! newTodo = ctx.BindJsonAsync<NewTodo>()
                let store = ctx.GetService<TodoStore>()

                let created =
                    { Id = Guid.NewGuid()
                      Description = newTodo.Description
                      Created = DateTime.UtcNow.AddHours(1)
                      IsCompleted = false }
                    |> store.Create

                return! json created next ctx
            }

    let updateTodo (_id: Guid) =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let! todo = ctx.BindJsonAsync<Todo>()
                let store = ctx.GetService<TodoStore>()

                return!
                    (match store.Update(todo) with
                     | true -> json true
                     | false -> RequestErrors.GONE "Gone")
                        next
                        ctx
            }

    let deleteTodo (id: Guid) =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let store = ctx.GetService<TodoStore>()

                return!
                    (match store.Get(id) with
                     | Some existing ->
                         let deleted = store.Delete(KeyValuePair<TodoId, Todo>(id, existing))
                         json deleted
                     | None -> RequestErrors.GONE "Gone")
                        next
                        ctx
            }

module TodoApiRoutes =
    let Routes =
        [ GET [ routef "/%O" Handlers.viewTodo; route "" Handlers.viewTodos ]
          POST [ route "" Handlers.createTodo ]
          PUT [ routef "/%O" Handlers.updateTodo ]
          DELETE [ routef "/%O" Handlers.deleteTodo ] ]
