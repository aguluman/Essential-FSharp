let calculate i =
    if i % 3 = 0 && i % 5 = 0 then "FizzBuzz"
    elif i % 3 = 0 then "Buzz"
    elif i % 5 = 0 then "Buzz"
    else i |> string

[1..15] |> List.map calculate

//Lets try pattern matching
//--> using 0 to represent true and _ to represent false
let calculatePM i =
    match ( i % 3, i % 5 ) with
    | 0, 0 -> "FizzBuzz"
    | 0, _ -> "Fizz"
    | _, 0 -> "Buzz"
    | _ -> i |> string

//--> using true and _ to represent false
let calculatePMBool i =
    match ( i % 3 = 0, i % 5 = 0 ) with
    | true, true -> "FizzBuzz"
    | true, _ -> "Fizz"
    | _, true -> "Buzz"
    | _ -> i |> string

[1..15] |> List.map calculatePM
[1..15] |> List.map calculatePMBool

//we can use a parameterized partial active patterns to do this
let (|IsDivisibleBy|_|) divisor n =
    if n % divisor = 0 then Some() else None

//Lets try using an Active Pattern Match
let calculatePAP i =
    match i with
    | IsDivisibleBy 3 & IsDivisibleBy 5 & IsDivisibleBy 7 -> "FizzBuzzBazz"
    | IsDivisibleBy 3 & IsDivisibleBy 5 -> "FizzBuzz"
    | IsDivisibleBy 3 & IsDivisibleBy 7 -> "FizzBazz"
    | IsDivisibleBy 5 & IsDivisibleBy 7 -> "BuzzBazz"
    | IsDivisibleBy 3 -> "Fizz"
    | IsDivisibleBy 5 -> "Buzz"
    | IsDivisibleBy 7 -> "Bazz"
    | _ -> i |> string

//An alternative suggestion would be to use a list as the input parameter of the partial-active-pattern
let (|IsDivisibleByList|_|) divisors n =
    if divisors |> List.forall (fun div -> n % div = 0)
    then Some ()
    else None

let calculatePAPList i =
    match i with
    | IsDivisibleByList [3;5;7] -> "FizzBuzzBazz"
    | IsDivisibleByList [3;5] -> "FizzBuzz"
    | IsDivisibleByList [3;7] -> "FizzBazz"
    | IsDivisibleByList [5;7] -> "BuzzBazz"
    | IsDivisibleByList [3] -> "Fizz"
    | IsDivisibleByList [5] -> "Buzz"
    | IsDivisibleByList [7] -> "Bazz"
    | _ -> i |> string


//Maybe a different approach will yield a better result
//FC stands for functional composition
let calculateFC n =
    [(3, "Fizz"); (5, "Buzz"); (7, "Bazz")]
    |> List.map (fun (divisor, result) -> if n % divisor = 0 then result else "")
    |> List.reduce (+) // (+) is a shortcut for (fun acc v -> acc + v)
    |> fun input -> if input = "" then string n else input

[1..150] |> List.map calculateFC

//How FSharp developers write their functions
let calculateFunction mapping n =
    mapping
    |> List.map (fun (divisor, result) -> if n % divisor = 0 then result else "")
    |> List.reduce (+)
    |> fun input -> if input = "" then string n else input

[1..115] |> List.map (calculateFunction [(3, "Fizz"); (5, "Buzz"); (7, "Bazz")])