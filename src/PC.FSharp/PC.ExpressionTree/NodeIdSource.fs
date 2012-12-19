namespace PebbleCode.ExpressionTree

// *****************************************************************************
// *** The different sources a node id can have
// *****************************************************************************

type NodeIdSource = 
    | Current
    | Previous
    | Precedent
    
    override this.ToString() = 
        match this with
        | Current -> "Cur"
        | Previous -> "Prev"
        | Precedent -> "Prec"

    static member public FromString (input:string) =
        match input with
        | "Cur" -> Current
        | "Prev" -> Previous
        | "Prec" -> Precedent
        | _ -> failwith "Unreconised string"
