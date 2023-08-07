//If we do not want to return a new list, we use List.iter function.
let myList = [1..9]

let print items =
    items
    (*|> List.iter(fun x -> (printfn "My value is %i" x))*)
    |> List.iter(printfn "My value is %i")

print myList

//It deconstructs the list and prints each item.