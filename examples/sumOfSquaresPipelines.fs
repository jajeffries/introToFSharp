#light

let sqr x = x * x

let sumOfSquares nums =
    nums 
    |> Seq.map sqr
    |> Seq.sum

let _ = sumOfSquares [1;2;3;4]
