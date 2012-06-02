#light

let sumOfSquares nums =
    nums
    |> Seq.map (fun x -> x * x)
    |> Seq.sum

let _ = sumOfSquares [1;2;3;4]
