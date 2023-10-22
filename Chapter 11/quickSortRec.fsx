//A Quick-Sort algorithm in Fsharp using recursion.
let rec quicksort input =
    match input with
    | [] -> []
    | head :: tail ->
        let smaller, larger = List.partition (fun n -> head >= n) tail
        List.concat [ quicksort smaller; [ head ]; quicksort larger ]

[ 5; 9; 5; 2; 7; 9; 1; 1; 3; 5 ] |> quicksort |> printfn "%A"
