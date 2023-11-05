module Chapter_13.Shared

open Giraffe.ViewEngine

let masterPage msg content =
    html [] [
        head [] [
            title [] [ str msg ]
            link [ _rel "stylesheet"; _href "css/main.css" ]
        ]
        body [] content
    ]