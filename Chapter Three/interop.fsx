//-------- Let's create a null for both a reference type and nullable primitive.-------
open System
open Microsoft.FSharp.Core

//Reference type
let nullObj: string = null

//Nullable type
let nullPri = Nullable<int>()

//To convert from dotnet to an f# option type we can use the Option.ofObj and Option.ofNullable functions
let fromNullObj = Option.ofObj nullObj

let fromNullPri = Option.ofNullable nullPri

//To convert from an f# option type to dotnet type we can use the Option.toObj and Option.toNullable functions
let toNullObj = Option.toObj fromNullObj

let toNullPri = Option.toNullable fromNullPri

//If you want to convert from an Option type to something that doesn't support null but instead expects a placeholder
//value? => we can use pattern matching as Option is a discriminated union or it can use Option.defaultValue function
let resultPM input =
    match input with
    | Some value -> value
    | None -> "------"

let resultDV = Option.defaultValue "------" fromNullObj

//We can use forward-pipe operator
let resultFP = fromNullObj |> Option.defaultValue "------"

//In a situation where we are going to be doing this a lot, one would find out that Partial Application makes this
//pleasurable.
let setUnknownAsDefault = Option.defaultValue "????"

let result =  fromNullObj |> setUnknownAsDefault
let resultTwo = setUnknownAsDefault fromNullObj