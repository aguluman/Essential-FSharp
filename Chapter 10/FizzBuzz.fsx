// This is a class type in fsharp
type FizzBuzz() =
    member _.Calculate(value) =
        [ (3, "Fizz"); (5, "Buzz") ]
        |> List.map (fun (v, s) -> if value % v = 0 then s else "")
        |> List.reduce (+)
        |> fun s -> if s = "" then string value else s

//To instantiate the class type we do.
let fizzBuzz = FizzBuzz()
let fifteen = fizzBuzz.Calculate(15)
let fifteenPA = 15 |> fizzBuzz.Calculate

//This is a function that uses the new FizzBuzz class type
let doFizzBuzz range =
    let fizzBuzz = FizzBuzz()
    //range |> List.map(fun n -> fizzBuzz.Calculate(n))not simple enough
    range |> List.map fizzBuzz.Calculate //simple enough

let output = doFizzBuzz [1..15]

//Instead of hard-coding the mapping, we can pass it in through the default constructor
type FizzBuzzTwo(mapping) =
    let calculate n =
        mapping
        |> List.map (fun (v, s) -> if n % v = 0 then s else "")
        |> List.reduce (+)
        |> fun s -> if s = "" then string n else s

    member _.Calculate(value) = calculate value