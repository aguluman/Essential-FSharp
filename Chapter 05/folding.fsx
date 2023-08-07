//List.Fold is used to do similar aggregation task on a list of values.
//In C#, Aggregate is the LINQ equivalent of List.Fold.

//A sum of numbers from 1 to 10 can be calculated as follows:
[ 1..10 ] |> List.fold (fun accumulator value -> accumulator + value) 0

//The first argument of List.fold is a function that takes two arguments: the accumulator and the current value.
//The second argument is the initial value of the accumulator.

//It can be simplified to look like,
[ 1..10 ] |> List.fold (+) 0

//For product(multiplication) we can do:
[ 1..10 ] |> List.fold (fun accumulator value -> accumulator * value) 1

//It can be further simplified to look like,
[ 1..10 ] |> List.fold (*) 1

let items = [ (1, 0.25M); (5, 0.25M); (1, 2.25M); (1, 125M); (7, 10.9M) ]

let getTotal items =
    items
    |> List.fold (fun accumulator (quantity, price) -> accumulator + decimal quantity * price) 0M

let total = getTotal items


//An alternative style is to another of the forward pipe operators, ||>.
//This version supports a pair of tuples inputs

let calculateTotal items =
    (0M, items)
    ||> List.fold (fun accumulator (quantity, price) -> accumulator + decimal quantity * price)

let calculation = calculateTotal items

//It is necessary to know the caliber of weapons you have in your arsenal,
//but in simple or mediocre encounter stick to the simpler artillery
//------ Fold is a nice feature to have, but it's best to try simpler version like `List.sumBy` first.
