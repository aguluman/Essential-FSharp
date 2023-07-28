type Customer =
    | Registered of Id: string * IsEligible: bool
    | Guest of Id: string

let calculateTotal customer spend =
    let discount =
        match customer with
        | Registered(Id = id; IsEligible = true) when spend >= 100.0M -> spend * 0.1M
        | _ -> 0.0M

    spend - discount

let john = Registered(Id = "John", IsEligible = true)
let complete = 100.0M |> calculateTotal john
let assertJohn = calculateTotal john 100.0M = 90.0M


let areEqual expected actual = expected = actual
let assertJohn2 = areEqual 90.0M (calculateTotal john 100.0M)


let isEqualTo expected actual = expected = actual

//Let's use the forward pipe method in its defined prefix form
let assertJohn4 = (|>) (calculateTotal john 100.0M) (isEqualTo 90.0M)

//When we convert from prefix to infix, we don't need to use the parenthesis
let assertJohn3 = calculateTotal john 100.0M |> isEqualTo 90.0M //decimal -> (decimal -> bool) -> bool
