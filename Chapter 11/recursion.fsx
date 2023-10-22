5! = 5 *  4 * 3 * 2 * 1 = 120
let rec fact n =
    match n with
    | 1 -> 1
    | n -> n * fact (n -1)


//the larger n gets, the more memory you need to perform
//the calculation and it can also lead to stack overflows.
//We can solve this problem with Tail Call Optimisation.

//Tail Call Optimisation
let fact n =
    let rec loop n acc =
        match n with
        | 1 -> acc //If the accumulator used addition, we would set it to 0 initially.
        | _ -> loop (n -1) (acc * n)
    loop n 1