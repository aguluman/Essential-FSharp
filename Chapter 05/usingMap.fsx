//Sometime we might want to perform an operation on each item in a list.
//For example, we might want to add 1 to every item in a list.
//----------To achieve this we use List.map function. --------------
let triple items =
    items
    |>List.map(fun x -> x * 3)
let myTriples = triple [1..10]

//If we have used C#, `Select` is the closest LINQ equivalent to `List.map`, except Select is lazily evaluated.
//If a case arises where we need lazy evaluation, we can use `Seq.map` instead of `List.map`.

//No mutation occurs here, just a times 3 copy of the list with the new values.