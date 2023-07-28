open System

let tryParseDateTime (input: string) =
    let (success, value) = DateTimeOffset.TryParse input
    if success then Some value else None
//------ The IF-Else statement and the pattern match still gives the same result.
(*match DateTimeOffset.TryParse input with
    | true, result -> Some result
    | false, _ -> None*)

let isDate = tryParseDateTime "22/07/2023"
let isNotDate = tryParseDateTime "Hello"

type PersonName =
    { FirstName: string
      MiddleName: string option
      LastName: string }

let person =
    { FirstName = "Ian"
      MiddleName = None
      LastName = "Russell" }
    
let person2 = { person with MiddleName = Some "Archie" }


