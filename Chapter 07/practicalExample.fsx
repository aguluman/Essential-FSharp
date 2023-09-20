type Score = int * int

let (|CorrectScore|_|) (expected: Score, actual: Score) =
    if expected = actual then Some() else None

let (|Draw|HomeWin|AwayWin|) (score: Score) =
    match score with
    | h, a when h = a -> Draw
    | h, a when h > a -> HomeWin
    | _ -> AwayWin

let (|CorrectResult|_|) (expected: Score, actual: Score) =
    match (expected, actual) with
    | Draw, Draw -> Some ()
    | HomeWin, HomeWin -> Some ()
    | AwayWin, AwayWin -> Some ()
    | _ -> None

//Without the multi-case active pattern for the result of a score, we would have to write this:
(*
let (|CorrectResultMCAP|_|) (expected: Score, actual: Score) =
    match expected, actual with //It can work with or without the brackets
    | (h, a), (h', a') when h = a && h'= a'-> Some ()
    | (h, a), (h', a') when h > a && h' > a'-> Some ()
    | (h, a), (h', a') when h < a && h' < a'-> Some ()
    | _ -> None   //This is readable, but the multi-case active pattern is more concise
    *)

(*let goalScore (expected:Score) (actual:Score) =
    let h,a = expected
    let h',a' = actual
    let home = [h; h'] |> List.min
    let away = [a; a'] |> List.min
    (home * 15) + (away * 10)*)

//We can simplify the goalScore function using a couple of helper function for tuples called fst and snd.
//It gets the first and second elements of a tuple respectively
let goalScoreTuples (expected: Score) (actual: Score) =
    let home = [ fst expected; fst actual ] |> List.min
    let away = [ snd expected; snd actual ] |> List.min
    (home * 15) + (away * 10)

//Lets create our function to calculate the total points for each game.
(*let calculatePoints (expected:Score) (actual: Score) =
    let pointsForCorrectScore =
        match (expected, actual) with
        | CorrectScore -> 300
        | _ -> 0
    let pointsForCorrectResult =
        match (expected, actual) with
        | CorrectResult -> 100
        | _ -> 0
    let pointsForGoals = goalScoreTuples expected actual
    pointsForCorrectScore + pointsForCorrectResult + pointsForGoals*)


//We can combine the pattern matching into a new function thus making calculatePoints more concise
let resultScore (expected:Score) (actual:Score) =
    match (expected, actual) with
    | CorrectScore -> 400
    | CorrectResult -> 100
    | _ -> 0
    //We returned 400 for CorrectScore as we no longer able to add the CorrectResult points later.
    //This allows us to simplify the calculatePoints function.

(*let calculatePointsConcise (expected:Score) (actual:Score) =
    let pointsForResult = resultScore expected actual
    let pointsForGoals = goalScoreTuples expected actual
    pointsForResult + pointsForGoals*)

//As the resultScore and goalScore function have the same signature,
//We can use a higher order function to remove the duplication

let calculatePointsHOF (expected:Score) (actual: Score) =
    [resultScore; goalScoreTuples]
    |> List.sumBy (fun f -> f expected actual)

//Now for tests
let assertNoScoreDrawCorrect =
    calculatePointsHOF (0,0) (0,0) = 400
let assertHomeWinExactMatch =
    calculatePointsHOF (3,2) (3,2) = (*485*) 465//True
let assertHomeWin =
    calculatePointsHOF (5,1) (4,3) = (*180*) 170//True
let assertIncorrect =
    calculatePointsHOF (2,1) (0,7) = (*20*) 10//True
let assertDraw =
    calculatePointsHOF (2,2) (3,3) = (*170*) 150//True