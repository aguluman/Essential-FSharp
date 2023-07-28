open System

let map f result =
    match result with
    | Ok x -> Ok(f, x)
    | Error ex -> Error ex

let tryParseDateTime (input: string) =
    let success, value = DateTimeOffset.TryParse input
    if success then Some value else None

let getResult =
    try
        Ok "Hello"
    with ex ->
        Error ex

let parsedDT = getResult |> map tryParseDateTime
