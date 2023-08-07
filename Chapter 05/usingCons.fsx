//We can add an item to a list using the cons (::) operator
let items = [2;5;3;1;4]

let extendedItems = 6::items //head::tail

//Understanding Heads and Tails in a list.
let readList items =
    match items with
    | [] -> "Empty List"
    (*
    | [head] -> $"Head: {head}" //This is a list containing one item.
    *)
    | head::tail -> sprintf "Head: %A and Tail: %A" head tail

let emptyList = readList []
let multipleList = readList [1..5]
let singleItemList = readList [1]