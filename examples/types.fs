module IntroToFSharp.Types

// Examples of types

// type abreviation
type SizeType = uint32

// type abrievation with generics
type Reducer<'a, 'b> = 'a -> 'b -> 'a

// Classes
type MyClass(x: string) = 
  let private_value = x
  do
    printfn "%s" private_value
  member this.PublicProperty = 
    "Public: " + private_value
  member this.PrintPublicProperty() =
    printfn "%s" this.PublicProperty

// Inheritance
type Point(x: int, y: int) as this =
  do
    this.PrintPoint()
  member this.X = x
  member this.Y = y
   
  new() = Point(0, 0)
  
  member this.PrintX() = 
    printfn "%d" this.X
  member this.PrintY() = 
    printfn "%d" this.Y

  // default so can be overriden
  abstract PrintPoint : unit -> unit
  default this.PrintPoint() = 
    printfn "Point(%d, %d)" this.X this.Y

type NamedPoint(name: string, x: int, y: int) = 
  inherit Point(x, y)
  // Properties are public
  member this.Name = name

  override this.PrintPoint() =
    printfn "Name: %s Point(%d, %d)" this.Name this.X this.Y

// Structures
type Point1D =
  struct 
    // val declares an uninitialised field 
    val x: float
  end

[<Struct>]
type AlternatePoint1D = 
  val x: float

// Records
type MyRecord = {
    X: int;
    Y: int;
    Z: int 
    }

let myRecord1 = { X = 1; Y = 2; Z = 3; }

let myRecord2 = { MyRecord.X = 1; MyRecord.Y = 2; MyRecord.Z = 3 }

// Unions
type BinaryTree =
  | Leaf of int
  | Node of int * BinaryTree * BinaryTree

let myTree = Node(1, Node(3, Leaf(2), Leaf(4)), Node(6, Leaf(5), Leaf(7)))

// Options
let mySomeOption = Some("A Value")
let myNoneOption = None

let processOption opt = 
  match opt with
  | Some(opt) -> printfn "%s" opt
  | None -> printfn "%s" "No Value"
