let isLeapYear year =
    year % 400 = 0 || ( year % 4 = 0 && year % 100 <> 0)

[2000; 2001; 2020] |> List.map isLeapYear = [true; false; true]

//Parameterized partial active patterns can make it more readable
let (|IsDivisibleBy|_|) divisor n =
    if n % divisor = 0 then Some() else None

let(|NotDivisibleBy|_|) divisor n =
    if n % divisor <> 0 then Some() else None

let isLeapYearPPAP year =
    match year with
    | IsDivisibleBy 400 -> true
    | IsDivisibleBy 4 & NotDivisibleBy 100 -> true
    | _ -> false

[2000; 2001; 2020] |> List.map isLeapYearPPAP = [true; false; true]


//Lets try the isLeapYear again but this time with helper functions
let isDivisibleBy divisor year =
    year % divisor = 0

let notDivisibleBy divisor year =
    not (year |> isDivisibleBy divisor)

let isLeapYearHF year =
    year |> isDivisibleBy 400 || (year |> isDivisibleBy 4 && year |> notDivisibleBy 100)

[2000; 2001; 2020] |> List.map isLeapYearHF = [true; false; true]

//We can also use a match expression with guard clauses
let isLeapYearGC input =
    match input with
    | year when year |> isDivisibleBy 400 -> true
    | year when year |> isDivisibleBy 4 && year |> notDivisibleBy 100 -> true
    | _ -> false

[2000; 2001; 2020] |> List.map isLeapYearGC = [true; false; true]


//We can remove the notDivisibleBy function and pipe the isDivisible result to the not function.
let isLeapYearGC2 input =
    match input with
    | year when year |> isDivisibleBy 400 -> true
    | year when year |> isDivisibleBy 4 && year |> isDivisibleBy 100 |> not -> true
    | _ -> false

[2000; 2001; 2020] |> List.map isLeapYearGC2 = [true; false; true]
