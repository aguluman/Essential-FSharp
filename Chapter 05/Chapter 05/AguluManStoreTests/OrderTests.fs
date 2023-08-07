namespace OrderTests

open Xunit
open FsUnit
open AguluManStore.Orders
open AguluManStore.Orders.Domain


module ``add item to order`` =

    [<Fact>]
    let ``when product does not exist in empty order`` () =
        let myEmptyOrder = { Id = 1; Items = [] }

        let expected =
            { Id = 1
              Items = [ { ProductId = 1; Quantity = 3 } ] }

        let actual = myEmptyOrder |> addItem { ProductId = 1; Quantity = 3 }
        actual |> should equal expected

    [<Fact>]
    let ``when product does not exist in non empty order`` () =
        let myOrder =
            { Id = 1
              Items = [ { ProductId = 1; Quantity = 1 } ] }

        let expected =
            { Id = 1
              Items = [ { ProductId = 1; Quantity = 1 }; { ProductId = 2; Quantity = 5 } ] }

        let actual = myOrder |> addItem { ProductId = 2; Quantity = 5 }
        actual |> should equal expected

    [<Fact>]
    let ``when product exists in non empty order`` () =
        let myOrder =
            { Id = 1
              Items = [ { ProductId = 1; Quantity = 1 } ] }

        let expected =
            { Id = 1
              Items = [ { ProductId = 1; Quantity = 4 } ] }

        let actual = myOrder |> addItem { ProductId = 1; Quantity = 3 }
        actual |> should equal expected


module ``add multiple items to an empty order`` =

    [<Fact>]
    let ``when new products are added to empty order`` () =
        let myEmptyOrder = { Id = 1; Items = [] }

        let expected =
            { Id = 1
              Items = [ { ProductId = 1; Quantity = 1 }; { ProductId = 2; Quantity = 5 } ] }

        let actual =
            myEmptyOrder
            |> addItems [ { ProductId = 1; Quantity = 1 }; { ProductId = 2; Quantity = 5 } ]

        actual |> should equal expected

    [<Fact>]
    let ``when new and existing product are updated to order`` () =
        let myOrder =
            { Id = 1
              Items = [ { ProductId = 1; Quantity = 1 } ] }

        let expected =
            { Id = 1
              Items = [ { ProductId = 1; Quantity = 2 }; { ProductId = 2; Quantity = 5 } ] }

        let actual =
            myOrder
            |> addItems [ { ProductId = 1; Quantity = 1 }; { ProductId = 2; Quantity = 5 } ]

        actual |> should equal expected


module ``removing a product`` =

    [<Fact>]
    let ``should remove all items of existing productId`` () =
        let myOrder =
            { Id = 1
              Items = [ { ProductId = 1; Quantity = 1 } ] }

        let expected = { Id = 1; Items = [] }

        let actual = myOrder |> removeProduct 1
        actual |> should equal expected

    [<Fact>]
    let ``should do nothing for non existent productId`` () =
        let myOrder =
            { Id = 2
              Items = [ { ProductId = 1; Quantity = 1 } ] }

        let expected =
            { Id = 2
              Items = [ { ProductId = 1; Quantity = 1 } ] }

        let actual = myOrder |> removeProduct 2
        actual |> should equal expected

module ``reduce item quantity`` =

    [<Fact>]
    let ``reduce existing item quantity`` () =
        let myOrder =
            { Id = 1
              Items = [ { ProductId = 1; Quantity = 5 } ] }

        let expected =
            { Id = 1
              Items = [ { ProductId = 1; Quantity = 2 } ] }

        let actual = myOrder |> reduceItem 1 3
        actual |> should equal expected


    [<Fact>]
    let ``reduce existing item and remove`` () =
        let myOrder =
            { Id = 2
              Items = [ { ProductId = 1; Quantity = 5 } ] }

        let expected = { Id = 2; Items = [] }

        let actual = myOrder |> reduceItem 1 5
        actual |> should equal expected

    [<Fact>]
    let ``reduce item with false productId and quantity`` () =
        let myOrder =
            { Id = 3
              Items = [ { ProductId = 1; Quantity = 1 } ] }

        let expected =
            { Id = 3
              Items = [ { ProductId = 1; Quantity = 1 } ] }

        let actual = myOrder |> reduceItem 2 5
        actual |> should equal expected

    [<Fact>]
    let ``reduce item with no quantity for empty order`` () =
        let myEmptyOrder = { Id = 4; Items = [] }
        let expected = { Id = 4; Items = [] }

        let actual = myEmptyOrder |> reduceItem 2 5
        actual |> should equal expected


module ``empty an order of all items`` =

    [<Fact>]
    let ``order with existing item`` () =
        let myOrder =
            { Id = 1
              Items = [ { ProductId = 1; Quantity = 1 } ] }

        let expected = { Id = 1; Items = [] }
        let actual = myOrder |> clearItems

        actual |> should equal expected

    [<Fact>]
    let ``order with existing items`` () =
        let myOrder =
            { Id = 1
              Items = [ { ProductId = 1; Quantity = 1 }; { ProductId = 2; Quantity = 5 } ] }

        let expected = { Id = 1; Items = [] }

        let actual = myOrder |> clearItems
        actual |> should equal expected

    [<Fact>]
    let ``empty order is unchanged`` () =
        let myEmptyOrder = { Id = 2; Items = [] }
        let expected = { Id = 2; Items = [] }

        let actual = myEmptyOrder |> clearItems
        actual |> should equal expected