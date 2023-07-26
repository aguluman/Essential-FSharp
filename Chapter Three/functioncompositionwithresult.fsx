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

//Trying to convert anonymous functions into named functions
//We will call the Higher-Order function we will create map, that utilizes tryPromoteToVip
let map (tryPromoteToVip: Customer * decimal -> Customer) (result: Result<Customer * decimal, exn>) =
    match result with
    | Ok x -> Ok(tryPromoteToVip x)
    | Error ex -> Error ex
//The map function is a Higher-Order function because it takes the tryPromoteToVip function as an input parameter

//------ We can also write a map function to take generic parameters ------
let mapTwo (f: 'a -> 'b) (result: Result<'a, 'c>) : Result<'b, 'c> =
    match result with
    | Ok x -> Ok (f x)
    | Error ex -> Error ex
//Writing a map function a different way like below.
let mapThree f result =
    match result with
    | Ok x -> Ok(f, x)
    | Error ex -> Error ex

//We will do something similar for the increaseCreditIfVip function.
//Here we do not need to wrap the output in an `Ok` result as the increaseCreditIfVip already does that for us.
let bind (increaseCreditIfVip: Customer -> Result<Customer, exn>) (result: Result<Customer, exn>) : Result<Customer, exn> =
    match result with
    |Ok x -> increaseCreditIfVip x
    | Error ex -> Error ex
//bind is also a higher order function because it takes the increasedCreditIfVip function as an input parameter

//---- We can make bind generic as don't use any of the parameters in the function
let bindTwo (f: 'a -> Result<'b, 'c>) (result: Result<'a, 'c>) : Result<'b, 'c> =
    match result with
    | Ok x -> f x
    | Error ex -> Error ex
    
//We can remove the parameter types too
let bindThree f result =
    match result with
    |Ok x -> f x
    |Error ex -> Error ex

let upgradeCustomer customer =
    customer
    |> getPurchases
    // We can plug in the map and bind function in here.
    |> map tryPromoteToVip
    |> bind increaseCreditIfVip
    
(*  |> fun result ->
        match result with
        | Ok x -> Ok(tryPromoteToVip x)
        | Error ex -> Error ex
    |> fun result ->
        match result with
        | Ok x -> increaseCreditIfVip x
        | Error ex -> Error ex      *)
        
    //`function can be used in place of "fun result ->"`
(*  |> function
        | Ok x -> Ok(tryPromoteToVip x)
        | Error ex -> Error ex
    |> function
        | Ok x -> increaseCreditIfVip x
        | Error ex -> Error ex    *)

    
//Map and bind function can lso look like this
let upgradeCustomerTwo customer =
    let purchasedResult = getPurchases customer
    let promotedResult = map tryPromoteToVip purchasedResult
    let increasedResult = bind increaseCreditIfVip promotedResult
    increasedResult
    
//The Result module in F# has them built in  as a common requirement
let upgradeCustomerThree customer =
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
