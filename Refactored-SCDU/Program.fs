open System
open System.IO
open System.Text.RegularExpressions
open FsToolkit.ErrorHandling.ValidationCE

type ValidationError =
    | MissingData of name: string
    | InvalidData of name: string * value: string

type CustomerId = CustomerId of string
type Email = Email of string
type IsEligible = IsEligible of bool
type DateRegistered = DateRegistered of DateTime
type Discount = Discount of decimal

type CustomerData =
    { CustomerId: string
      Email: string
      IsEligible: string
      IsRegistered: string
      DateRegistered: string
      Discount: string }

type RegisteredCustomer =
    { CustomerId: CustomerId
      Email: Email
      IsEligible: IsEligible
      DateRegistered: DateRegistered
      Discount: Discount }

type GuestCustomer = { Name: string }

type Customer =
    | Registered of RegisteredCustomer
    | Guest of GuestCustomer

type FileReader = string -> Result<string seq, exn>

module FileReader =
    let readFile: string -> Result<string seq, exn> =
        fun path ->
            try
                seq {
                    use reader = new StreamReader(File.OpenRead(path))

                    while not reader.EndOfStream do
                        yield reader.ReadLine()
                }
                |> Ok
            with ex ->
                Error ex


// Parsing.fs
module Parsing =
    let readFile: FileReader =
        fun path ->
            try
                seq {
                    use reader = new StreamReader(File.OpenRead(path))

                    while not reader.EndOfStream do
                        yield reader.ReadLine()
                }
                |> Ok
            with ex ->
                Error ex

    let parseLine (line: string) : CustomerData option =
        match line.Split('|') with
        | [| customerId; email; eligible; registered; dateRegistered; discount |] ->
            Some
                { CustomerId = customerId
                  Email = email
                  IsEligible = eligible
                  IsRegistered = registered
                  DateRegistered = dateRegistered
                  Discount = discount }
        | _ -> None


module Validators =
    let validateWithPattern pattern errorMessage input =
        match pattern input with
        | Some data -> Ok data
        | None -> Error [ InvalidData(errorMessage, input) ]

    let validateCustomerId customerId =
        if customerId |> String.IsNullOrWhiteSpace then
            Error [ MissingData "CustomerId" ]
        elif customerId |> Seq.forall (fun c -> Char.IsLetter(c) || c = ' ') then
            Ok(CustomerId customerId)
        else
            Error [ InvalidData("A non-alphabet was present in CustomerId", customerId) ]


    let validateIsEligible isEligible =
        let isBoolean (str: string) =
            match str with
            | "1" -> Some true
            | "0" -> Some false
            | _ -> None

        isEligible
        |> validateWithPattern isBoolean "IsEligible"
        |> Result.map IsEligible

    let validateDiscount discount =
        let isDecimal (str: string) =
            match Decimal.TryParse str with
            | true, value -> Some value
            | _ -> None

        discount |> validateWithPattern isDecimal "Discount" |> Result.map Discount

    let validateDateRegistered dateRegistered =
        let isValidDate (str: string) =
            match DateTime.TryParse str with
            | true, date -> Some date
            | _ -> None

        dateRegistered
        |> validateWithPattern isValidDate "DateRegistered"
        |> Result.map DateRegistered

    let validateEmail email =
        let isValidEmail str =
            Regex(".*?@(.*)").Match str |> (fun m -> if m.Success then Some str else None)

        email |> validateWithPattern isValidEmail "Email" |> Result.map Email

    let validateRegisteredCustomer (input: CustomerData) : Result<Customer, ValidationError list> =
        validation {
            let! customerId =  input.CustomerId |> validateCustomerId
            and! email = validateEmail input.Email
            and! isEligible = validateIsEligible input.IsEligible
            and! dateRegistered = validateDateRegistered input.DateRegistered
            and! discount = validateDiscount input.Discount

            return
                Registered
                    { CustomerId = customerId
                      Email = email
                      IsEligible = isEligible
                      DateRegistered = dateRegistered
                      Discount = discount }
        }

    let validateName (name: string) =
        if name |> Seq.forall (fun c -> Char.IsLetter(c) || c = ' ') then
            Ok name
        else
            Error [ InvalidData("A non-alphabet was present in Name", name) ]

    let validateGuestData (input: CustomerData) : Result<Customer, ValidationError list> =
        validation {
            let! name = input.CustomerId |> validateName |> Result.mapError id
            return Guest { Name = name }
        }
    (*let validateGuestData (input: CustomerData) : Result<Customer, ValidationError list> =
        match  input.CustomerId |> validateCustomerId with
        | Ok(CustomerId name) -> Ok(Guest {Name = name})
        | Error e -> Error e*)

    let validate (data: CustomerData) =
        match data.IsRegistered with
        | "1" -> validateRegisteredCustomer data
        | _ -> validateGuestData data


module Application =
    open Parsing
    open Validators

    let parse (data: string seq) =
        data |> Seq.skip 1 |> Seq.map parseLine |> Seq.choose id |> Seq.map validate

    let output data = data |> Seq.iter (printfn "%A")

    let import (fileReader: FileReader) path =
        match fileReader path with
        | Ok data -> data |> parse |> output
        | Error ex -> printfn $"Error: %A{ex}"

    [<EntryPoint>]
    let main _argv =
        Path.Combine(__SOURCE_DIRECTORY__, "resources", "customers.csv")
        |> import readFile

        0
