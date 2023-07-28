type Customer =
    { Id: int
      IsVip: bool
      Credit: decimal }

let getPurchase customer =
    let purchases = if customer.Id % 2 = 0 then 120M else 80M
    (customer, purchases)

let tryPromoteToVip purchases =
    let customer, amount = purchases

    if amount > 100M then
        { customer with IsVip = true }
    else
        customer

let increaseCreditIfVip customer =
    let increase = if customer.IsVip then 100M else 50M

    { customer with
        Credit = customer.Credit + increase }


//Composition Operator
let upgradeCustomerComposed = getPurchase >> tryPromoteToVip >> increaseCreditIfVip

//Nested Style
let upgradeCustomerNested customer =
    increaseCreditIfVip (tryPromoteToVip (getPurchase customer))

//Procedural Style
let upgradeCustomerProcedural customer =
    let customerWithPurchases = getPurchase customer
    let promotedCustomer = tryPromoteToVip customerWithPurchases
    let increasedCreditCustomer = increaseCreditIfVip promotedCustomer
    increasedCreditCustomer

//Forward Pipe operator
let upgradeCustomerPiped customer =
    customer |> getPurchase |> tryPromoteToVip |> increaseCreditIfVip


//Test
let customerVIP = { Id = 1; IsVip = true; Credit = 0.0m }

let customerSTD =
    { Id = 2
      IsVip = false
      Credit = 100.0m }

let assertVIP =
    upgradeCustomerPiped customerVIP = { Id = 1
                                         IsVip = true
                                         Credit = 100.0m }

let assertSTDtoVIP =
    upgradeCustomerPiped customerSTD = { Id = 2
                                         IsVip = true
                                         Credit = 200.0m }

let assertSTD =
    upgradeCustomerPiped
        { customerSTD with
            Id = 3
            Credit = 50.0m } = { Id = 3
                                 IsVip = false
                                 Credit = 100.0m }
