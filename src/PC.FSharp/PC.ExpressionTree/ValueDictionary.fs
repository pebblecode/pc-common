namespace CT.ExpressionTree

open Microsoft.FSharp.Collections

type NodeDictionary = 

    val mutable private _nodes : Map<NodeId,Node>

    new (nodes:List<Node>) = {
        _nodes = new Map<NodeId,Node>(List.map (fun(node:Node) ->  node.Id, node) nodes)
        }

    new () = {
        _nodes = Map.empty<NodeId,Node>
        }

    member this.GetPreciseValueOrNull(nodeId:NodeId) =
        if this._nodes.ContainsKey(nodeId) then
            Some(this._nodes.[nodeId].PreciseValue);
        else
            option.None

    member this.Item
        with get (nodeId:NodeId) = this._nodes.[nodeId]
        and set (nodeId:NodeId) (node:Node) = 
            this._nodes <- this._nodes.Add(nodeId, node)

    member this.ContainsKey nodeId =
        this._nodes.ContainsKey nodeId

    member this.Add nodeId node =
        this._nodes <- this._nodes.Add(nodeId, node)