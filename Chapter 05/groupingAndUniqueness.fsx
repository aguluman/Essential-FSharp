//If we wanted to get the unique numbers from a list.
//Firstly we can use `List.groupBy` function to return a tuple for each distinct value.
let myList = [ 1; 2; 3; 4; 5; 7; 6; 5; 4; 3 ]

let groupByResult = myList |> List.groupBy (fun x -> x) //The anonymous function fun x -> x can be replaced with id (identity function)

//To get unique items from a resul list, we can also use`List.map`
let unique items =
    items
    |> List.groupBy id
    (*|> List.map fst*)
    |> List.map (fun (i, _) -> i)

let gbAndMapResult = unique myList

//A function called List.distinct can do exactly what `List.groupBy` and `List.map` just did
let distinct = myList |> List.distinct

//There is also a built in collection type called Set, that can produce the exact same result as well.
let uniqueSet items = items |> Set.ofList
let setResult = uniqueSet myList

//Many of the collection types have a way of converting from and to each-other