//We can join(concatenate) 2/two lists together.
let list1 = [1..5]
let list2 = [3..7]
let emptyArray = []

let joined = list1 @ list2
let joinedEmpty = list1 @ emptyArray
let emptyJoined = emptyArray @ list2
//We can use the List.concat function to do the same job as @ operator
let joinedByFunction = List.concat [list1; list2]