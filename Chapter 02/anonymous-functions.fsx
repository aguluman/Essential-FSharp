let add x y = x + y
let sum = add 1 4

//We can re-write the add function to use a lambda
let add2 = fun x y -> x + y
let sum2 = add2 4 9
let apply f x y = f x y
let sum3 = apply add2 9 17
let sum4 = apply (fun x y -> x + y) 1 4 