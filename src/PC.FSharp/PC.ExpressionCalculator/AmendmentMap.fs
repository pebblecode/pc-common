namespace PebbleCode.ExpressionCalculator

open System
open System.Collections.Generic
open System.Runtime.Serialization

open PebbleCode.ExpressionTree

/// <summary>
/// Represents a set of amendments on the calculation
/// </summary>
[<Serializable>]
type AmendmentMap =
    inherit Dictionary<NodeId, Amendment>
        
    new() = {
        inherit Dictionary<NodeId, Amendment>()
        }

    new(info:SerializationInfo, ctx:StreamingContext) = {
        inherit Dictionary<NodeId, Amendment>(info, ctx)
        }

    member public this.Clone() =
        let clone = new AmendmentMap()
        for nodeId in this.Keys do
            clone.Add(nodeId, this.[nodeId])
        clone
