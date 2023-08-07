let item = []
let items = [2;5;3;1;4]
let ascendingItems = [1..5]
//List Comprehension
let itemsForListComprehension = [
    for x in 1..5 do x
        (* yield x *)  //Since F# 5 we have been able to drop the need for the yield keyword
]