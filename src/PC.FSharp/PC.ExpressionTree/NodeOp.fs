namespace PebbleCode.ExpressionTree

// *****************************************************************************
// *** The operations supported on a calc node
// *****************************************************************************

type NodeOp =
    | Add
    | Sub
    | Mul
    | Div
    | Min
    | Max
    | And
    | Or
    | Nil
    | Count

    member this.Evaluate : (decimal list -> decimal) =
        match this with
        | Add -> List.sum
        | Mul -> List.reduce (*)
        | Div -> List.reduce (/)
        | Sub -> List.reduce (-)
        | Min -> List.reduce (min)
        | Max -> List.reduce (max)
        | And -> List.reduce (fun x y 
                                -> match (x, y) with
                                    | (0m, _) -> 0m
                                    | (_, 0m) -> 0m
                                    | _ -> 1m)
        | Or -> List.reduce (fun x y 
                                -> match (x, y) with
                                    | (0m, 0m) -> 0m
                                    | _ -> 1m)
        | Nil -> (fun x -> 0m)
        | Count -> (fun x -> decimal(List.length x))

    member this.IsBooleanOp : bool = 
        match this with
        | And | Or -> true
        | _ -> false

    override this.ToString() = 
        match this with
        | Add -> "Add"
        | Mul -> "Mul"
        | Div -> "Div"
        | Sub -> "Sub"
        | Min -> "Min"
        | Max -> "Max"
        | And -> "And"
        | Or -> "Or"
        | Nil -> "Nil"
        | Count -> "Count"
