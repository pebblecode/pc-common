namespace PebbleCode.ExpressionTree

// *****************************************************************************
// *** The operations supported on a comp node
// *****************************************************************************

type ValueSource =
    | Constant
    | Input
    | Previous
    | PreviousInput
    | Precedent
    | Entity
    | Deserialised

    override this.ToString() = 
        match this with
        | Constant -> "Const"
        | Input -> "Input"
        | Previous -> "Previous"
        | PreviousInput -> "PreviousInput"
        | Precedent -> "Precedent"
        | Entity -> "Entity"
        | Deserialised -> "Deserialised"
