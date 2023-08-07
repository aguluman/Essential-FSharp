//Thanks to the F# language team, there are a wide range of functions available...
//...in the collection modules, to solve any given problem.

//**** Let's find the sum of squares in an odd numbers ****

//Sum of the squares of the odd numbers
let nums = [ 3..20 ]

nums
|> List.filter (fun x -> x % 2 = 1)
|> List.map (fun x -> int x * x)
|> List.sum

//using options and choose
nums
|> List.choose (fun v -> if v % 2 = 1 then Some(v * v) else None)
|> List.sum

//using Fold
nums |> List.fold (fun acc v -> acc + if v % 2 = 1 then (v * v) else 0) 0

//Trying out List.reduce function
match nums with
| [] -> 0
| items -> items |> List.reduce(fun acc v -> acc + if v % 2 = 1 then ( v * v) else 0)
//Caution == Do not use reduce for this sort of problem.
//Firstly reduce is a partial function, so we need to handle empty list
//More importantly, the first item in the list is not processed by the function, this proved true. 3 wasn't squared
//Our answer might be incorrect, Y/N. Yes the result is incorrect.
//List.reduce doesnt work on an empty list as it use the first item as the initial value, so not having one is an issue.

//using sumBy
nums |>  List.sumBy(fun v -> if v % 2 = 1 then (v * v) else 0)