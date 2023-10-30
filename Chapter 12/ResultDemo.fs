namespace ComputationExpression

module ResultDemo =
    open FsToolkit.ErrorHandling

    type Customer =
        { Id: int
          IsVip: bool
          Credit: decimal }

    let getPurchases customer =
        try
            let purchases =
                if customer.Id % 2 = 0 then
                    (customer, 120M)
                else
                    (customer, 80M)

            Ok purchases
        with ex ->
            Error ex.Message

    let tryPromoteToVip purchases =
        let customer, amount = purchases

        if amount > 100M then
            { customer with IsVip = true }
        else
            customer

    let increasedCreditIfVip customer =
        try
            let increase = if customer.IsVip then 100M else 50M

            Ok
                { customer with
                    Credit = customer.Credit + increase }
        with ex ->
            Error ex.Message

    (*
    let upgradeCustomer customer =
        customer
        |> getPurchase
        |> Result.map tryPromoteToVip
        |> Result.bind increasedCreditIfVip  *)

    let upgradeCustomer customer =
        result {
            let! purchases = getPurchases customer
            let promoted = tryPromoteToVip purchases
            return! increasedCreditIfVip promoted
        }
    //It is readable compared to the earlier upgradeCustomer function