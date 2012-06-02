#light

let sqr x = x * x

let rec sumOfSquares nums =
    match nums with
    | []   -> 0
    | h::t -> sqr h + sumOfSquares t

let _ = sumOfSquares [1;2;3;4]
