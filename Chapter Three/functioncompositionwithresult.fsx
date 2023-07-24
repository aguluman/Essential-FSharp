type Customer =
    { Id: int
      IsVip: bool
      Credit: decimal }

//imagine this function is fetching data from a database.
let getPurchases customer =
    try
        let purchases =
            if customer.Id % 2 = 0 then
                (customer, 120m)
            else
                (customer, 80m)

        Ok purchases
    with ex ->
        Error ex

let tryPromoteToVip purchases =
    let customer, amount = purchases

    if amount > 100m then
        { customer with IsVip = true }
    else
        customer

//Imagine this function would cause an exception
let increaseCreditIfVip customer =
    try
        let increase = if customer.IsVip then 100m else 50m

        Ok
            { customer with
                Credit = customer.Credit + increase }
    with ex ->
        Error ex

(*
let map (tryPromoteToVip: Customer * decimal -> Customer) (result: Result<Customer * decimal, exn>) : Result<Customer,exn> =
*)
(*let map f result =
    match result with
    | Ok x -> Ok(f x)
    | Error ex -> Error ex

let bind f result =
    match result with
    | Ok x -> f x
    | Error ex -> Error ex*)


let upgradeCustomer customer =
    customer
    |> getPurchases
    |> Result.map tryPromoteToVip
    |> Result.bind increaseCreditIfVip

let customerVIP = { Id = 1; IsVip = true; Credit = 0M }

let customerSTD =
    { Id = 2
      IsVip = false
      Credit = 100.0M }

let assertVIP =
    upgradeCustomer customerVIP = Ok { Id = 1; IsVip = true; Credit = 100m }

let assertSTDtoVIP =
    upgradeCustomer customerSTD = Ok { Id = 2; IsVip = true; Credit = 200m }

let assertSTD =
    upgradeCustomer
        { customerSTD with
            Id = 3
            Credit = 50m } = Ok { Id = 3; IsVip = false; Credit = 100m }
