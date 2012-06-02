#light

let sqr x = x * x

let sumOfSquares nums = 
    let mutable acc = 0
    for x in nums do 
        acc <- acc + sqr x
    acc

let _ = sumOfSquares [1;2;3;4]
