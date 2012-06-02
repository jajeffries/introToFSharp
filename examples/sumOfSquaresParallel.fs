open System.Linq
open System

let pmap f (l:seq<_>) = 
    let pe = l.AsParallel<_>()
    ParallelEnumerable.Select(pe, Func<_,_>(f))

let sqr x = x * x

let sumOfSquares nums =
    nums 
    |> Seq.map sqr
    |> Seq.sum

let _ = sumOfSquares [1;2;3;4]
