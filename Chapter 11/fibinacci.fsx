//performing fibonacci sequence in fsharp.
let rec fib (n:int64) =
    match n with
    | 0L -> 0L
    | 1L -> 1L
    | s -> fib (s - 1L) + fib (s - 2L)

printfn$"fib 50 = {fib 50L}"

//Let try this with Tail call recursion.
let fibTailCall (n:int64) =
    let rec loop x (a,b) =
        match x with
        | 0L -> a
        | 1L -> b
        | x -> loop (x - 1L) (b, a + b)
    loop (n - 1L) (0L, 1L)

printfn$"fibTailCall 50 = {fibTailCall 50L}"