namespace PebbleCode.ExpressionTree

// *****************************************************************************
// *** The operations supported on a comp node
// *****************************************************************************

type CompOp =
    | GreaterThan
    | GreaterThanOrEqual
    | LessThan
    | LessThanOrEqual
    | Equal
    | NotEqual
    | Custom of (decimal -> decimal -> bool) * string

    member this.Op =
        match this with
        | GreaterThan -> (>)
        | GreaterThanOrEqual -> (>=)
        | LessThan -> (<)
        | LessThanOrEqual -> (<=)
        | Equal -> (=)
        | NotEqual -> (<>)
        | Custom(op,_) -> op

    override this.ToString() = 
        match this with
        | GreaterThan -> ">"
        | GreaterThanOrEqual -> ">="
        | LessThan -> "<"
        | LessThanOrEqual -> "<="
        | Equal -> "="
        | NotEqual -> "!="
        | Custom(_,opString) -> opString
