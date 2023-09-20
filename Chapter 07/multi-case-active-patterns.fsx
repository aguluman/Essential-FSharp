type Rank =
    | Ace
    | Two
    | Three
    | Four
    | Five
    | Six
    | Seven
    | Eight
    | Nine
    | Ten
    | Jack
    | Queen
    | King

type Suit =
    | Clubs
    | Diamonds
    | Hearts
    | Spades

type Card = Rank * Suit

//The active pattern needs to take a Card as input, determine its suit and return either Red or Black as output.
let (|Red|Black|) (card :Card) =
    match card with
    | _, Diamonds | _, Hearts -> Red
    | _, Clubs | _, Spades -> Black
//This isn't a PAP so no wildcard

let describeColor card =
    match card with
    |Red -> "red"
    |Black -> "black"
    |> printfn "This card is %s"

describeColor (Two, Hearts)