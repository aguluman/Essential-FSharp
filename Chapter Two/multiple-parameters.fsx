type Customer =
    | Registered of Id: string * IsEligible: bool
    | Guest of Id: string

let calculateTotal customer =
    fun spend ->
        (let discount =
            (match customer with
             | Registered(Id = id; IsEligible = true) when spend >= 100.0M -> spend * 0.1M
             | _ -> 0.0M)

         spend - discount)
