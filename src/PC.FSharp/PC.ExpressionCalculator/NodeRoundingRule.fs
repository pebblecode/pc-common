namespace PebbleCode.ExpressionCalculator

open System
open System.Text.RegularExpressions
open PebbleCode.ExpressionTree

type NodeRoundingRule =

    // *************************************************************************
    // *** The different types of rule
    // *************************************************************************
    
    // A rule that matches on exact node ids only
    | ExactNodeId of RoundingRules * NodeId

    // A rule that mathces on the name of the node id, ignoring entities
    // Also allows a callback to see if you want every node or not
    | NameOnlyNodeId of RoundingRules * NodeId *  ((NodeId -> bool) option)

    // A rule that mathces on the node meta name, checking if it contains the string
    // (case insensitive)
    | NameContains of RoundingRules * string
    
    // A rule that mathces on the node meta name, checking if it starts with the string
    // (case insensitive)
    | NameStartsWith of RoundingRules * string
    
    // A rule that matches on the node id, taking into account the _all that occurs at the end
    // of a sub fund node id name
    | SubFundName of RoundingRules * NodeId

    // *************************************************************************
    // *** Grab the meta rules for the rule
    // *************************************************************************
    member this.Rule =
        match this with
        | ExactNodeId (rules,_)
        | NameOnlyNodeId (rules,_,_)
        | NameContains (rules,_)
        | NameStartsWith (rules,_)
        | SubFundName (rules,_)
            -> rules

    // *************************************************************************
    // ***  Constructors
    // *************************************************************************

    member this.Match (node:Node) =
        match this with
        | ExactNodeId (_,nodeId)
            -> nodeId = node.Id
        | NameOnlyNodeId (_,nodeId,callback)
            -> nodeId.Name = node.Id.Name && (callback.IsNone || (callback.Value node.Id))
        | NameContains (_,partialName)
            -> node.Meta.Name.IndexOf(partialName, StringComparison.InvariantCultureIgnoreCase) >= 0
        | NameStartsWith (_,partialName)
            -> node.Meta.Name.StartsWith(partialName, StringComparison.InvariantCultureIgnoreCase)
        | SubFundName (_,nodeId)
            -> 
            let underscoreIdx = nodeId.Name.IndexOf("_");
            if (node.Id.Name.Length >= underscoreIdx) then
                nodeId.Name.Substring(0, underscoreIdx) = node.Id.Name.Substring(0, underscoreIdx)
            else
                false

    member this.Apply (node:Node) =
        node.Meta.RoundingRules <- this.Rule

