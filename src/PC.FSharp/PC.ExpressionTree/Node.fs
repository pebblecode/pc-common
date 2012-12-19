namespace PebbleCode.ExpressionTree

open Microsoft.FSharp.Core;
open System;
open System.Collections.Generic
open System.IO;
open System.Runtime.Serialization.Formatters.Binary;

open PebbleCode.Entities
open PebbleCode.Framework
open PebbleCode.Framework.Utilities;

// *****************************************************************************
// ***
// *** The guts of an expression tree... THE NODE
// ***
// *****************************************************************************

type Node = 
    
    // *************************************************************************
    // *** The different types of node allowed
    // *************************************************************************

    // Value. A literal value
    | Value   of NodeMeta * ValueSource * decimal

    // A date value, with reference date
    | DateValue of NodeMeta * DateTime * DateTime

    // If. Decision node taking three nodes. Test, ValueIfTrue, ValueIfFalse
    | If      of NodeMeta * Node * Node * Node

    // Comp. Comparison node taking two nodes and an op.
    | Comp    of NodeMeta * CompOp * Node * Node

    // Calc. Calculate a value by taking a list of nodes and an operator to apply to them
    | List    of NodeMeta * NodeOp * list<Node>

    // Negatives the signage of a child node
    | Negative of NodeMeta * Node
    
    // Not operation on a child node
    | Not of NodeMeta * Node

    // Not operation on a child node
    | Absolute of NodeMeta * Node


    // *************************************************************************
    // *** Grab the meta for the node
    // *************************************************************************
    member this.Meta =
        match this with
        | Value (meta,_,_)
        | DateValue (meta,_,_)
        | List  (meta,_,_)
        | If (meta,_,_,_)
        | Comp (meta,_,_,_)
        | Negative (meta,_)
        | Not (meta,_)
        | Absolute (meta,_)
            -> meta

    // *************************************************************************
    // *** Useful description of node
    // *************************************************************************
    member this.Description =
        match this with
        | Value (_,source,_) -> sprintf "%O" source
        | DateValue (_,_,_) -> "Date"
        | List  (_,op,_) -> sprintf "%O" op
        | If (_,_,_,_) -> "IF"
        | Comp (_,op,_,_) -> sprintf "%O" op
        | Negative (_,_) -> "Negative"
        | Not (_,_) -> "Not"
        | Absolute (_,_) -> "Absolute"

    // *************************************************************************
    // *** Grab the node op for a node
    // *************************************************************************
    member this.NodeOp =
        match this with
        | List  (_,op,_)
            -> Some(op)
        | _ 
            -> None

    // *************************************************************************
    // *** Convenience properties that call onto the meta
    // *************************************************************************
    member this.Name = this.Meta.Name
    member this.Id = this.Meta.Id

    // *************************************************************************
    // *** Grab all the children of the node
    // *************************************************************************
    member this.ChildNodes =
        match this with
        
        // Simple nodes do not need resolving
        | Value  (_,_,_)     -> []
        | DateValue (_,_,_)  -> []

        // If, Comp and Calc know about all their nodes
        | If      (_, node1, node2, node3) -> [node1; node2; node3]
        | Comp  (_, _, node1, node2) -> [node1; node2]
        | List    (_, _, nodes) -> nodes
        | Negative (_,subTree)     -> [subTree]
        | Not (_,subTree)     -> [subTree]
        | Absolute (_,subTree)     -> [subTree]

    member this.AllDescendants =
        let rec findDescendants (node:Node) =
            match node.ChildNodes with
            | [] -> []
            | children -> children @ (List.concat (List.map findDescendants children))
        this :: findDescendants this

    // *************************************************************************
    // *** Get ALL attributes of this node and and child nodes
    // *** combined into one dictionary
    // *************************************************************************
    member this.AllAttributes =
        this.ChildNodes
        |> List.map (fun x -> x.Meta.allAttr)
        |> List.append [this.Meta.allAttr]
        |> List.reduce (fun x y -> x.Merge(y))

    // *************************************************************************
    // *** Take this node and convert to a simple value node (essentially 
    // *** dropping any subtree the node may have had)
    // *************************************************************************
    member this.SimplifyToValue (valueSource:ValueSource) (newSource:NodeIdSource) =
        let newNode = 
            Node.Value(
                new NodeMeta(valueSource.ToString() + "_" + this.Meta.Name, this.Meta), 
                valueSource, 
                this.PreciseValue)

        // Remove the override flag and cache value to make this node re-eval
        newNode.Meta.CachedValue <- None
        newNode.Meta.Override <- None
        newNode.Meta.Id.Source <- newSource
        newNode

    // *************************************************************************
    // *** Clone method
    // *************************************************************************
    member this.Clone =
        match this with
        | Value (meta,var1,var2) -> Node.Value(new NodeMeta(meta.Name, meta), var1, var2)
        | DateValue (meta,var1,var2) -> Node.DateValue(new NodeMeta(meta.Name, meta), var1, var2)
        | List  (meta,var1,var2) -> Node.List(new NodeMeta(meta.Name, meta), var1, var2)
        | If (meta,var1,var2,var3) -> Node.If(new NodeMeta(meta.Name, meta), var1, var2, var3)
        | Comp (meta,var1,var2,var3) -> Node.Comp(new NodeMeta(meta.Name, meta), var1, var2, var3)
        | Negative (meta,var1) -> Node.Negative(new NodeMeta(meta.Name, meta), var1)
        | Not (meta,var1) -> Node.Not(new NodeMeta(meta.Name, meta), var1)
        | Absolute (meta,var1) -> Node.Absolute(new NodeMeta(meta.Name, meta), var1)

    // *************************************************************************
    // ***  
    // ***  All after here is part of EVALUATE...
    // ***  
    // *************************************************************************

    // Evaluate a list of nodes using a single NodeOp to combine them
    member this.EvaluateNodeOp nodes (nodeOp:NodeOp) =
        let value =
            List.map (fun (x:Node) -> x.Evaluate()) nodes
            |> nodeOp.Evaluate
        value

    // Method to evaluate a node in an expression tree, returning the overridden value or 
    // the result of evaluating its sub tree
    member private this.EvaluateExpression  =

        //Has this Node value already been calculated?
        if this.Meta.CachedValue.IsNone then

            //Calculate and store this value
            this.Meta.CachedValue <- Some(
                match this with
        
                    // ** VALUE **
                    // A literal value
                    | Value  (_,_,value) -> value

                    // ** DATE VALUE **
                    // Evaluate to a decimal by returning number of 
                    // days difference between value and reference dates
                    | DateValue (_, date, refDate) -> 
                        let diff = date.Date - refDate.Date
                        decimal diff.TotalDays 

                    // ** IF **
                    // Evaluate test node.
                    // If non-zero then return value of true node.
                    // Else return value of false node.
                    | If (_, test, valueIfTrue, valueIfFalse)
                        -> match (test.EvaluateExpression) with
                            | 0m -> valueIfFalse.EvaluateExpression
                            | _  -> valueIfTrue.EvaluateExpression

                    // ** COMP **
                    // Evaluate both nodes and apply comparison. Return 1 if true, 0 if false
                    | Comp (_, compOp, leftNode, rightNode)
                        -> match compOp.Op (leftNode.EvaluateExpression) (rightNode.EvaluateExpression) with
                            | true -> 1m
                            | false -> 0m

                    // ** LIST **
                    // Calculate a value by taking a list of nodes and an operator to apply to them
                    | List (_, nodeOp, nodes)
                        -> this.EvaluateNodeOp nodes nodeOp

                    // ** NEGATIVE **
                    // Evaluate subtree, make negative
                    | Negative  (_,subTree) -> 0m - subTree.EvaluateExpression
                    
                    // ** NOT **
                    // Evaluate subtree, apply not unary opration
                    | Not (_,subTree) 
                        -> match (subTree.EvaluateExpression) with
                            | 0m -> 1m
                            | _ -> 0m

                    // ** Absolute **
                    // Evaluate subtree, make it an absolute value
                    | Absolute (_,subTree)
                        -> Math.Abs(subTree.EvaluateExpression)
                )

            //Print the cached value in debug
            if this.Name.Contains("Master") then
                System.Diagnostics.Debug.WriteLine("{0} :: ", this.Id, this.Meta.CachedValue.Value)

        // Return the evaluated node value for use higher up the tree
        // If its overridden, this is where we return the override
        if this.Meta.Override.IsNone then
            this.Meta.CachedValue.Value
        else
            this.Meta.Override.Value

    member this.IsOverridden =
        this.Meta.Override.IsSome

    // Get a list of any child nodes that are "dead". I.e. not used due to
    // the IF test.
    member this.DeadChildNodes : Node list =
        match this with
        | If (_, test, valueIfTrue, valueIfFalse)
            -> match (test.EvaluateExpression) with
                | 0m -> [ valueIfTrue ]
                | _  -> [ valueIfFalse ]
        | _ -> []

    // Apply the precision factor from the nodes meta data
    member private this.ApplyPrecision (value:decimal) =
        let precision = this.Meta.RoundingRules.CalculationPrecision
        match precision with
            | -1 -> value
            | _ ->
            match this.Meta.RoundingRules.RoundingStyle with
                | RoundingStyle.Round -> MathUtils.Round(value, precision)
                | RoundingStyle.RoundUp -> MathUtils.RoundUp(value, precision)
                | RoundingStyle.RoundDown -> MathUtils.RoundDown(value, precision)
                | RoundingStyle.Truncate -> MathUtils.Truncate(value, precision)

    // Method to evaluate a node in an expression tree, returning the overridden value or 
    // the result of evaluating its sub tree
    member this.Evaluate() = 
        this.EvaluateExpression |> this.Meta.RoundingRules.ApplyCalculationPrecision

    member this.PreciseValue = this.Evaluate()

    member this.DisplayValue = 
        this.PreciseValue
        |> this.Meta.RoundingRules.ApplyDisplayPrecision

    member this.PreciseValue_NoOverride =
        // make sure evaluated
        ignore(this.EvaluateExpression)
        // return cached value (no override applied)
        this.Meta.CachedValue.Value

    member this.DisplayValue_NoOverride = 
        this.PreciseValue_NoOverride
        |> this.Meta.RoundingRules.ApplyDisplayPrecision

    // Built in string formatting
    member private this.FormatString precision =
        let format = 
            match this.Meta.DisplayAsPercentage with
            | true -> "P" 
            | _ -> "N"

        match precision with
        | -1 -> format + "12"
        | _ -> format + precision.ToString()

    member private this.Format (value:decimal) (precision:int) =
        value.ToString(this.FormatString precision)

    // Get the value for displaying to the user
    member private this.FormatDisplayValueString (value:decimal) = 
        match this with
        | Comp(_,_,_,_) -> this.BooleanDisplayValue
        | Not(_,_) -> this.BooleanDisplayValue
        | List(_,op,_) when op.IsBooleanOp -> this.BooleanDisplayValue
        | Value(m,_,_) when m.NodeFormat = NodeFormat.Boolean -> this.BooleanDisplayValue
        | _ ->
            // Try to format according to rules
            let valueString = this.Format value this.Meta.RoundingRules.DisplayPrecision

            // Is the value, when formatted, still showing 0 but is actually a really small number?
            // See here: https://pebbleit.basecamphq.com/projects/5068702/todo_items/82378523/comments
            // If so, then format to significant figures instead of precision
            if (Decimal.Parse(valueString.TrimEnd('%')) = 0m && value <> 0m) then
                let rec firstSignificantDecimal x = 
                    if x >= 0.1m || x <= -0.1m then
                        0
                    else
                        (firstSignificantDecimal (x * 10m)) + 1
                this.Format value ((firstSignificantDecimal value) + this.Meta.RoundingRules.DisplayPrecision)
            else
                valueString

    member this.DisplayValueString = this.FormatDisplayValueString  this.PreciseValue
    member this.DisplayValueString_NoOverride = this.FormatDisplayValueString  this.PreciseValue_NoOverride

    member this.BooleanDisplayValue = if this.BooleanValue then "YES" else "NO"

    member this.BooleanValue =
        match this.PreciseValue with
        | 0m -> false
        | _ -> true

    member this.PreciseValueString = 
        this.Format this.PreciseValue this.Meta.RoundingRules.CalculationPrecision

    // Constant value nodes
    static member Constant (value:decimal) = Node.Value(new NodeMeta(sprintf "Constant: %f" value), ValueSource.Constant, value);
    static member Zero = Node.Value(new NodeMeta("Zero"), ValueSource.Constant, 0m);

    override this.ToString() = this.Name

    member this.FindChild (nodeId:NodeId) =
        List.tryFind (fun (x:Node) -> x.Id = nodeId) this.ChildNodes

    member this.Find (path:NodeId list) =
        match path with
        | [] 
            -> Some(this)
        | [head] 
            -> this.FindChild head
        | head::tail
            -> match (this.FindChild head) with
                | None -> None
                | node -> node.Value.Find(tail)

    member this.FindChildStr (nodeIdStr:string) =
        List.tryFind (fun (x:Node) -> x.Id.Name = nodeIdStr) this.ChildNodes

    member this.FindStr (path:string list) = 
        match path with
        | []
            -> Some(this)
        | [head] 
            -> this.FindChildStr head
        | head::tail
            -> match (this.FindChildStr head) with
                | None -> None
                | node -> node.Value.FindStr(tail)