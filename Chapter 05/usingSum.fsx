//We can add up the items in a list using the List.sum function
let myList = [1..9]

let sum items =
    items
    |> List.sum
let mySum = sum myList