namespace PebbleCode.ExpressionCalculator

open System
open System.Collections.Generic

open PebbleCode.ExpressionTree

/// <summary>
/// Represents an amendment made by the user to the expression tree
/// </summary>
type Amendment(nodeId:NodeId, value:decimal, comment:string, username:string) =

    // Constructor
    let _nodeId = nodeId
    let _value = value
    let _comment = comment
    let _username = username
    let _dateTime = DateTime.UtcNow

    // Public property getters
    member public this.NodeId = _nodeId
    member public this.Value = _value
    member public this.Comment = _comment
    member public this.Username = _username
    member public this.DateTime = _dateTime