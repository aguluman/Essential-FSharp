open ComputationExpression.OptionDemo
open ComputationExpression.AsyncDemo
open ComputationExpression.AsyncResultDemoTests
open System.IO
[<EntryPoint>]
let main _argv =
    calculate 8 0 |> printfn "\ncalculate 8 0 = %A"
    calculate 8 2 |> printfn "calculate 8 2 = %A\n"

    Path.Combine(__SOURCE_DIRECTORY__, "resources", "customers.csv")
    |> getFileInformation
    |> Async.RunSynchronously
    |> printfn "%A\n"


    printfn $"Success: %b{success}"
    printfn $"BadPassword: %b{badPassword}"
    printfn $"InvalidUser: %b{invalidUser}"
    printfn $"IsSuspended: %b{isSuspended}"
    printfn $"IsBanned: %b{isBanned}"
    printfn $"HasBadLuck: %b{hasBadLuck}"

    0