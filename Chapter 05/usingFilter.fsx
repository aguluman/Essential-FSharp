//We can filter a list == using a predicate function with the signature a -> bool and the List.filter function
let myList = [1..9]

let getEvens items =
    items
    |> List.filter ( fun x -> x % 2 = 0)

let evens = getEvens myList

