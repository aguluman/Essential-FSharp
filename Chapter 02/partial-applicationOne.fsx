type Customer =
    | Registered of Id: string * IsEligible: bool
    | Guest of Id: string

let calculateTotal customer spend =
    let discount =
        match customer with
        | Registered(Id = id; IsEligible = true) when spend >= 100.0M -> spend * 0.1M
        | _ -> 0.0M

    spend - discount

let john = Registered ( Id = "John", IsEligible = true )

let partial = calculateTotal john
let complete = partial 100.0M
let doesNotWork = calculateTotal 100.0M
//Doesn't compile because, we must add the input parameters in strict left to right order.