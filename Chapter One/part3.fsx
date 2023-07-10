type Customer =
    | Eligible of Id : string
    | Registered of Id : string
    | Guest of Id : string

let calculateTotal customer spend =
    let discount =
        match customer with
        | Eligible _ when spend >= 100.0m -> spend * 0.1m
        | _ -> 0.0M

    spend - discount

let john = Eligible "John" 

let mary = Eligible "Mary" 

let richard = Eligible "Richard" 

let sarah = Guest "Sarah"


let assertJohn = calculateTotal john 100.0M = 90.0M
let assertMary = calculateTotal mary 99.0M = 99.0M
let assertRichard = calculateTotal richard 100.0M = 100.0M
let assertSarah = calculateTotal sarah 100.0M = 100.0M