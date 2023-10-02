open System
open System.IO
open System.Text.RegularExpressions
open FsToolkit.ErrorHandling.ValidationCE

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

type GuestCustomer =
    { CustomerId: CustomerId
      Email: Email
      IsEligible: IsEligible }

type Customer =
    | Registered of RegisteredCustomer
    | Guest of GuestCustomer

type ImportCustomer = CustomerData -> Customer

type FileReader = string -> Result<string seq, exn>

type ValidationError =
    | MissingData of name: string
    | InvalidData of name: string * value: string

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

let (|ParseRegex|_|) regex str =
        let m = Regex(regex).Match(str)

        if m.Success then
            Some(List.tail [ for x in m.Groups -> x.Value ])
        else
            None

let (|IsValidEmail|_|) input =
        match input with
        | ParseRegex ".*?@(.*)" [ _ ] -> Some input
        | _ -> None

let (|IsEmptyString|_|) (input: string) =
        if input.Trim() = "" then Some() else None

let (|IsDecimal|_|) (input: string) =
        let success, value = Decimal.TryParse input
        if success then Some value else None

let (|IsBoolean|_|) (input: string) =
        match input with
        | "1" -> Some true
        | "0" -> Some false
        | _ -> None

let (|IsValidDate|_|) (input: string) =
        let success, value = input |> DateTime.TryParse
        if success then Some value else None

let validateCustomerId customerId =
        if customerId <> "" then
            Ok customerId
        else
            Error(MissingData "CustomerId")

let validateEmail email =
        if email <> "" then
            match email with
            | IsValidEmail _ -> Ok(Some email)
            | _ -> Error(InvalidData("Email", email))
        else
            Ok None

let validateIsEligible (isEligible: string) =
        match isEligible with
        | IsBoolean b -> Ok b
        | _ -> Error(InvalidData("IsEligible", isEligible))

let validateIsRegistered (isRegistered: string) =
        match isRegistered with
        | IsBoolean b -> Ok b
        | _ -> Error(InvalidData("IsRegistered", isRegistered))

let validateDateRegistered (dateRegistered: string) =
        match dateRegistered with
        | IsEmptyString -> Ok None
        | IsValidDate dt -> Ok(Some dt)
        | _ -> Error(InvalidData("DateRegistered", dateRegistered))

let validateDiscount discount =
        match discount with
        | IsEmptyString -> Ok None
        | IsDecimal value -> Ok(Some value)
        | _ -> Error(InvalidData("Discount", discount))

let validateRegisteredCustomer (input:CustomerData) : Result<RegisteredCustomer, ValidationError list> =
    validation {
        let! customerId = validateCustomerId input.CustomerId
        and! email = validateEmail input.Email
        and! isEligible = validateIsEligible input.IsEligible
        and! dateRegistered = validateDateRegistered input.DateRegistered
        and! discount = validateDiscount input.Discount

        return
            { CustomerId = CustomerId customerId
              Email = Email email
              IsEligible = IsEligible isEligible
              DateRegistered = DateRegistered dateRegistered
              Discount = Discount discount }
    }

let validateGuestData (input:CustomerData) : Result<GuestCustomer, ValidationError list> =
    validation {
        let! customerId = validateCustomerId input.CustomerId
        and! email = validateEmail input.Email
        and! isEligible = validateIsEligible input.IsEligible

        return
            { CustomerId = CustomerId customerId
              Email = Email email
              IsEligible = IsEligible isEligible }
    }


let validate (data:CustomerData) =
    match data.IsRegistered with
    | "1" ->
        match validateRegisteredCustomer data with
        | Ok(r) -> Ok(Registered r)
        | Error(e) -> Error(e)

    | _ ->
        match validateGuestData data with
        | Ok(g) -> Ok(Guest g)
        | Error(e) -> Error(e)

let parse (data: string seq) =
        data
        |> Seq.skip 1
        |> Seq.map parseLine
        |> Seq.choose id
        |> Seq.map validate

let output data = data |> Seq.iter (printfn "%A")

let import (fileReader: FileReader) path =
        match path |> fileReader with
        | Ok data -> data |> parse |> output
        | Error ex -> printfn $"Error: %A{ex}"

[<EntryPoint>]
let main _argv =
    Path.Combine(__SOURCE_DIRECTORY__, "resources", "customers.csv")
    |> import readFile

    0
