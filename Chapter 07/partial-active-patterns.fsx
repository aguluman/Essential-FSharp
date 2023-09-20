open System

//Here we are using a function to parse a String to a DateTime
(*let parse (input:string) =
    match DateTime.TryParse(input) with
    | true, value -> Some value
    | false, _ -> None*)

(*let isDate = parse "2019-12-20"
let isNotDate = parse "Hello"*)



//Here is a partial active pattern to handle the DateTime
let  (|ValidDate|_|)(input:string)=
    match DateTime.TryParse(input) with
    | true, value -> Some value
    | false, _ -> None


//It can be written to use an If expression
let (|IsValidDate|_|) (input:string) =
    let success, value = DateTime.TryParse(input)
    if success then Some () else None

let parse input =
    match input with
    | ValidDate dt -> printfn $"{dt}"
    | _ -> printfn $"{input} is not a valid case"

parse "2019-12-20"
parse "Hello"

let isValidDate input =
    match input with
    |IsValidDate -> true
    | _ -> false

isValidDate "2019-12-20"
isValidDate "Hello"