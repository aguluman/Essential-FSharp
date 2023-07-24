//Result.fsx
open System

let tryDivide (x: decimal) (y: decimal) =
    try
        Ok(x / y)
    with :? DivideByZeroException as ex ->
        Error ex

let goodDivide = tryDivide 23m 456m
let badDivide = tryDivide 22m 0m

//The try-with expression is used to handle exceptions and is analogous to Try-Catch in C#.
//We are expecting one specific error type: System.DivideByZeroException.
//The cast operator:? is a pattern matching feature to match a specified type or subtype, in this case, if the error can be cast as System.DivideByZeroException.
//The ex gives us access to the actual exception instance which we then pass as the case data to construct the Error case.