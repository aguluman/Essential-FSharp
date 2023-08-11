open System.IO

type Customer =
    { CustomerId: string
      Email: string
      IsEligible: string
      IsRegistered: string
      DateRegistered: string
      Discount: string }

type DataReader = string -> Result<string seq, exn>

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

let parse (data: string seq) =
    data
    |> Seq.skip 1 //Ignore the header row.
    |> Seq.map parseLine
    |> Seq.choose id //Ignores None and unwraps Some values.

let readFile: DataReader =
    fun path ->
        try
            File.ReadLines(path) |> Ok
        with ex ->
            Error ex

let output data =
    data |> Seq.iter (fun x -> printfn $"{x}")

let import (dataReader: DataReader) path =
    match path |> dataReader with
    | Ok data -> data |> parse |> output
    | Error ex -> printfn $"Error: {ex.Message}"

(*let import path =
    match path |> readFile with
    | Ok data -> data |> parse |> Seq.iter (fun x -> printfn $"{x}")
    | Error ex -> printfn $"Error: {ex.Message}"*)

let importWithFileReader = import readFile

let fakeDataReader: DataReader =
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
    Path.Combine(__SOURCE_DIRECTORY__, "resources", "customer.csv")
    |> importWithFileReader

    import fakeDataReader "_"

    0