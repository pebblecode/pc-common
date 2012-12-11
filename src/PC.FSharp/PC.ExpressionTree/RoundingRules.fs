namespace PebbleCode.ExpressionTree

open System
open PebbleCode.Framework.Utilities

// *****************************************************************************
// *** 
// *** Different style of rounding a node value
// *** 
// *****************************************************************************

type RoundingStyle = 
    | Round
    | RoundUp
    | RoundDown
    | Truncate

    member this.Apply value precision =
        match this with
            | Round -> MathUtils.Round(value, precision)
            | RoundUp -> MathUtils.RoundUp(value, precision)
            | RoundDown -> MathUtils.RoundDown(value, precision)
            | Truncate -> MathUtils.Truncate(value, precision)

    override this.ToString() = 
        match this with
            | Round -> "Round"
            | RoundUp -> "RoundUp"
            | RoundDown -> "RoundDown"
            | Truncate -> "Truncate"

// *****************************************************************************
// *** 
// *** Encapsulate rounding rules
// *** 
// *****************************************************************************

[<System.ComponentModel.TypeConverterAttribute("CT.Client.ValuationEditor.NodeMetaExpandableObjectConverter,  CT.Client.ValuationEditor")>]
type RoundingRules = 
    {
        CalculationPrecision:int;
        DisplayPrecision:int;
        RoundingStyle:RoundingStyle;
    }

    member private this.ApplyPrecision value precision =
        match precision with
            | -1 -> value
            | _ -> this.RoundingStyle.Apply value precision

    member this.ApplyCalculationPrecision (value:decimal) =
        this.ApplyPrecision value this.CalculationPrecision

    member this.ApplyDisplayPrecision (value:decimal) =
        this.ApplyPrecision value this.DisplayPrecision