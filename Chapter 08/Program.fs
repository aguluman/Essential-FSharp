open System
open System.IO
open System.Text.RegularExpressions

type Customer =
    { CustomerId: string
      Email: string
      IsEligible: string
      IsRegistered: string
      DateRegistered: string
      Discount: string }

type ValidatedCustomer =
    { CustomerId: string
      Email: string option
      IsEligible: string
      IsRegistered: string
      DateRegistered: string option
      Discount: string }


type ValidationError =
    | MissingData of name: string
    | InvalidData of name: string * value: string

type FileReader = string -> Result<string seq, exn>

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

let parseLine (line: string) : Customer option =
    match line.Split('|') with
    | [| customerId; email; isEligible; isRegistered; dateRegistered; discount |] ->
        Some
            { CustomerId = customerId
              Email = email
              IsEligible = isEligible
              IsRegistered = isRegistered
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

//We are now creating valid functions using active pattens and the new ValidationError discrimination union type.
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

let getError input =
    match input with
    | Ok _ -> []
    | Error ex -> [ ex ]

(*let convertToStringOption value =
    match value with
    | Ok (Some v) -> Ok (Some (v.ToString()))
    | Ok None -> Ok None
    | Error e -> Error e

let convertToString value =
  match value with
  | Ok v -> Ok (v.ToString())
  | Error e -> Error e

let getValueForStringOption input =
    match convertToStringOption input with
    | Ok v ->  v
    | _ -> failwith "Oops, you shouldn't have got here!!!"

let getValueForString input =
    match convertToString input with
    | Ok v ->  v
    | _ -> failwith "Oops, you shouldn't have got here!!!"*)
let getValue input =
    match input with
    | Ok v ->  v
    | _ -> failwith "Oops, you shouldn't have got here!!!"

let create customerId email isEligible isRegistered dateRegistered discount =
    { CustomerId = customerId
      Email = email
      IsEligible = isEligible
      IsRegistered = isRegistered
      DateRegistered = dateRegistered
      Discount = discount }

//Next we create a list of potential errors using a list comprehension, concatenate them using List.concat,
//and then check to see if the result has any errors, if there are no errors, we can safely call the create function

let validate (input: Customer) : Result<ValidatedCustomer, ValidationError list> =
    let customerId = input.CustomerId |> validateCustomerId
    let email = input.Email |> validateEmail
    let isEligible =  input.IsEligible |> validateIsEligible
    let isRegistered = input.IsRegistered |> validateIsRegistered
    let dateRegistered = input.DateRegistered |> validateDateRegistered
    let discount = input.Discount |> validateDiscount

    let errors =
        [ customerId |> getError
          email |> getError
          isEligible |> getError
          isRegistered |> getError
          dateRegistered |> getError
          discount |> getError ]
        |> List.concat

    match errors with
    | [] ->
        Ok(
            create
                (customerId |> getValue)
                (email |> getValue)
                (isEligible  |> getValue)
                (isRegistered |> getValue)
                (dateRegistered |> getValue)
                (discount |> getValue)
        )
    | _ -> Error errors

let parse (data: string seq) =
    data
    |> Seq.skip 1 //Ignore the header row.
    |> Seq.map parseLine
    |> Seq.choose id //Ignores None and unwraps Some values.
    |> Seq.map validate


let output data =
    data |> Seq.iter (fun x -> printfn $"{x}")

let import (fileReader: FileReader) path =
    match path |> fileReader with
    | Ok data -> data |> parse |> output
    | Error ex -> printfn $"Error: {ex.Message}"

let importWithFileReader = import readFile


let fakeDataReader: FileReader =
    fun _ ->
        seq {
            "CustomerId|Email|Eligible|Registered|DateRegistered|Discount"
            "John|john@test.com|1|1|2015-01-23|0.1"
            "Mary|mary@test.com|1|1|2018-12-12|0.1"
            "Richard|richard@nottest.com|0|1|2016-03-23|0.0"
            "Sarah||0|0||"
        }
        |> Ok


[<EntryPoint>]
let main _ =
    Path.Combine(__SOURCE_DIRECTORY__, "resources", "customers.csv")
    |> importWithFileReader

    //import fakeDataReader "_"

    0
