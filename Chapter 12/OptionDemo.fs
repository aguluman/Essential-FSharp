namespace ComputationExpression

[<AutoOpen>]
module Option =
    type OptionBuilder() =
        //Supports let!
        member _.Bind(x,f) = Option.bind f x
        //Supports return
        member _.Return(x) = Some x
        //Supports return!
        member _.ReturnFrom(x) = x

    //Computational Expression for Option
    //Usage will be option {...}
    let option = OptionBuilder()

module OptionDemo =
    let multiply x y = x * y

    let divide x y = if y = 0 then None else Some(x / y)

    (*
    let calculate x y =
        divide x y
        |> fun result ->
            match result with
            | Some v -> multiply v x |> Some
            | None -> None
        |> fun result ->
            match result with
            | Some t -> divide t y
            | None -> None

    let calculateOption y z =
        divide y z
        |> Option.map (fun v -> multiply v y)
        |> Option.bind (fun t -> divide t z)
    *)

    let calculate x y =
        option {
            let! v = divide x y
            let t = multiply v x
            //let! r = divide t y
            return! divide t y
        }