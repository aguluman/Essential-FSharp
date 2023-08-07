let items = [ (1, 0.25M); (5, 0.25M); (1, 2.25M); (1, 125M); (7, 10.9M) ]

let sum items =
    items
    |> List.map (fun (quantity, price) -> decimal quantity * price)
    |> List.sum

let total items =
    items
    |> List.sumBy (fun (quantity, price) -> decimal quantity * price)

let sumOutput = sum items
let totalOutput = total items