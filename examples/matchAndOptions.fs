let safediv x y = 
  match y with
  | 0 -> None
  | _ -> Some(x/y)
 
// val safediv : int -> int -> int option
