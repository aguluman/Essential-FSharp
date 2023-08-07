(*
namespace Source.Customer

open System

type Customer = { Name: String }

module Domain =
    let createCustomer (name: String) =
        { Name = name }
        
module Db =
    open System.IO
    
    let saveCustomer (customer: Customer) =
        File.WriteAllText("customer.txt", customer.Name) //Imagine this talks to a database
        
*)


//We can also separate the namespace and module definition

(*
namespace MyApplication //Namespace

open System

module Customer = //Module
    type Customer = { Name: String }

    //We can also use nest modules
    module Domain =
        let createCustomer (name: String) = { Name = name }

    module Db =
        open System.IO

        let saveCustomer (customer: Customer) =
            File.WriteAllText("customer.txt", customer.Name) //Imagine this talks to a database
            
*)


//We can use the module keyword at the top level to define a module, in this example,
//Source is the namespace and Customer is the module

(*
module Source.Customer

open System

type Customer = { Name: String }

module Domain =
    let createCustomer (name: String) =
        { Name = name }
        
module Db =
    open System.IO
    
    let saveCustomer (customer: Customer) =
        File.WriteAllText("customer.txt", customer.Name) //Imagine this talks to a database
        
*)


//A good starting point is to add a namespace to the top of each file using the project name
//and use modules for everything


module Source.Customer

type Customer =
    { Id: int
      IsVip: bool
      Credit: decimal }

let getPurchases customer =
    let purchases = if customer.Id % 2 = 0 then 120M else 80M
    (customer, purchases)

let tryPromoteToVip purchases =
    let (customer, amount) = purchases

    if amount > 100M then
        { customer with IsVip = true }
    else
        customer

let increaseCreditIfVip customer =
    let increase = if customer.IsVip then 100M else 50M

    { customer with
        Credit = customer.Credit + increase }

let upgradeCustomer = getPurchases >> tryPromoteToVip >> increaseCreditIfVip

let customerVIP = { Id = 1; IsVip = true; Credit = 0.0M }

let customerSTD =
    { Id = 2
      IsVip = false
      Credit = 100.0M }

let areEqual expected actual = actual = expected

let assertVIP =
    let expected =
        { Id = 1
          IsVip = true
          Credit = 100.0M }

    areEqual expected (upgradeCustomer customerVIP)

let assertSTDtoVIP =
    let expected =
        { Id = 2
          IsVip = true
          Credit = 200.0M }

    areEqual expected (upgradeCustomer customerSTD)

let assertSTD =
    let expected =
        { Id = 3
          IsVip = false
          Credit = 100.0M }

    areEqual
        expected
        (upgradeCustomer
            { Id = 3
              IsVip = false
              Credit = 100.0M })
