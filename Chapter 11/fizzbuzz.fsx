//Using recursion to solve FizzBuzz
let mapping = [ (3, "Fizz"); (5, "Buzz"); (7, "Bazz") ]

let fizzBuzz initialMapping n =
    let rec loop mapping acc =
        match mapping with
        | [] -> if acc = "" then string n else acc
        | head :: tail ->
            let value = head |> (fun (div, msg) -> if n % div = 0 then msg else "")
            loop tail (acc + value)

    loop initialMapping ""

[ 1..105 ] |> List.map (fizzBuzz mapping) |> List.iter (printfn "%s")

//Using List.fold function to solve this.
let fizzBuzzFold n =
    [ (3, "Fizz"); (5, "Buzz"); (7, "Bazz") ]
    |> List.fold(fun acc (div, msg) ->
        if n % div = 0 then acc + msg else acc) ""
    |> fun s -> if s = "" then string n else s

[1 .. 105] |> List.iter (fizzBuzzFold >> printfn "%s")

//We can modify all of the mapping in the fold function
//rather than passing the value on to another function.
let fizzBuzzFoldModified n =
    [ (3, "Fizz"); (5, "Buzz") ]
    |> List.fold (fun acc (div, msg) ->
        match (if n % div = 0 then msg else "") with
        | "" -> acc
        | s -> if acc = string n then s else acc + s) (string n)

[1 .. 105] |> List.iter(fizzBuzzFoldModified >> printfn "%s")
