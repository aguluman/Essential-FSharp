//The interface

open Microsoft.FSharp.Core

type IFizzBuzz =
    abstract member Calculate : int -> string

type FizzBuzzTwo(mapping) =
    let calculate n =
        mapping
        |> List.map (fun (v, s) -> if n % v = 0 then s else "")
        |> List.reduce (+)
        |> fun s -> if s <> "" then string n else s

    interface IFizzBuzz with
        member _.Calculate(value) = calculate value

let doFizzBuzz range =
    let fizzBuzz = FizzBuzzTwo(mapping = [ 3, "Fizz"; 5, "Buzz" ])
    range |> List.map (fizzBuzz :> IFizzBuzz).Calculate

let output = doFizzBuzz [ 1..15 ]
