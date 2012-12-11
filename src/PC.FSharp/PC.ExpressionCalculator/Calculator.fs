namespace PebbleCode.ExpressionCalculator

open System
open System.Collections.Generic

open PebbleCode.Entities
open PebbleCode.Framework
open PebbleCode.Framework.Collections
open PebbleCode.Framework.Dates
open PebbleCode.ExpressionTree

type BuildCachedMember = unit -> Node

[<AbstractClass>]
type Calculator (nodeRoundingRules:list<NodeRoundingRule> , calculation:Calculation) = 

    // Store constructor params    
    let _nodeRoundingRules = nodeRoundingRules
    let mutable _calculation = calculation

    // *****************************************************************************
    // *** Constructor
    // *****************************************************************************
    do
        // Nothing for now
        ignore()

    // *****************************************************************************
    // *** Convenience properties to stored data
    // *****************************************************************************

    member this.NodeRoundingRules = _nodeRoundingRules
    member this.Calculation = _calculation

    // *****************************************************************************
    // *** Accessing related Node dictionaries
    // *****************************************************************************

    member public this.Inputs (nodeId:NodeId) (nodeName:string) (inputType : string) = 
        this.InputOrDefault 0m nodeId (Some nodeName) (inputType : string)
    member public this.InputOrDefault (defaultNodeValue:decimal) (nodeId:NodeId) (nodeName:string option) (inputType : string) =
        this.GetNamedNodeOrDefault this.Calculation NodeIdSource.Current ValueSource.Input nodeId defaultNodeValue nodeName
        |> this.setAttr "InputType" inputType

    //Utility function to decorate the retrieval of a specific node from a related dictionary, 
    //alternating to a default constant node
    member public this.GetNodeOrDefault (calculation:Calculation) (nodeSource:NodeIdSource) (valueSource:ValueSource) (nodeId:NodeId)(defaultNodeValue:decimal) =
        this.GetNamedNodeOrDefault calculation nodeSource valueSource nodeId defaultNodeValue None

    member public this.GetNamedNodeOrDefault (calculation:Calculation) (nodeSource:NodeIdSource) (valueSource:ValueSource) (nodeId:NodeId)(defaultNodeValue:decimal) (nodeName:string option) =
        let alteredNodeId = new NodeId(nodeId)
        alteredNodeId.Source <- nodeSource
        this.ProcessNode
            alteredNodeId
            (fun () -> 
                if (calculation.ContainsKey(alteredNodeId)) then
                    calculation.[alteredNodeId]
                else
                    
                    let meta = match nodeName with
                                | None -> new NodeMeta(alteredNodeId)
                                | Some(newName) -> 
                                    let m = new NodeMeta(newName)
                                    m.Id <- alteredNodeId
                                    m

                    this.ValueNode (meta) valueSource defaultNodeValue
            )

    // *****************************************************************************
    // *** Overridden by derived calculations to build all nodes, adding them to the cache
    // *****************************************************************************

    abstract member BuildNodes : unit -> Node list
    default this.BuildNodes() = []

    // *****************************************************************************
    // *** Calculate (which actually builds and evaluates all nodes) and associated event
    // *****************************************************************************

    member public this.Calculate(amendments:Amendment list) =
        _calculation.Clear()
        List.iter (fun x -> _calculation.AddAmendment(x)) amendments
        let rootNode = this.BuildRootNode()
        this.Calculation.Evaluate() 
        this.Calculation.IntegrityCheck()
        this.OnCalculated

    abstract member OnCalculated : unit
    default this.OnCalculated = ignore()

    member private this.BuildRootNode() : Node =
        this.ProcessNode
            (Calculation.RootNodeId)
            (fun () -> 
                this.ListNode
                    (new NodeMeta(Calculation.RootNodeId))
                    NodeOp.Nil
                    (this.BuildNodes())
            )

    // *****************************************************************************
    // *** Apply rounding rules to nodes as they are added to the calculation
    // *****************************************************************************

    member this.ApplyRounding (node:Node) =
        let rule =
            this.NodeRoundingRules
            |> List.tryFind (fun nrr -> nrr.Match node)

        if Option.isSome(rule) then
            Option.get(rule).Apply node


    // *****************************************************************************
    // *** Add and get nodes
    // *****************************************************************************

    member this.AddNode (node:Node) =
        if this.Calculation.ContainsKey(node.Id) = false then
            this.ApplyRounding(node)                // Apply rounding rules to every node
            this.Calculation.Add(node.Id, node)     // Add the node
            List.iter this.AddNode node.ChildNodes  // And its children

    member this.GetNode (nodeId:NodeId) =
        if this.Calculation.ContainsKey(nodeId) then
            this.Calculation.[nodeId]
        else
            failwith (sprintf "Node [%s] not found" nodeId.Name)

    member this.HasNode (nodeId:NodeId) =
        this.Calculation.ContainsKey(nodeId)

    // *****************************************************************************
    // *** Wrap around building of nodes to enable caching
    // *****************************************************************************

    member public this.ProcessNode (id:NodeId) (builderFn:BuildCachedMember) = 
        // If we haven't cached the node, build it
        if not(this.Calculation.ContainsKey(id)) then

            let mutable calculationNode = builderFn()   // Build the subtree

            // Integrity check. If this fails, it probably means that somewhere in builderFn you
            // are calling ProcessNode on the exact same node that is about to be added here.
            // That can lead to problems when we overwrite the nodeId as we are about to do. You
            // may need to add another level to the calculation. builderFn should not end up 
            // calling ProcessNode as the first thing it doesn, it must create a Node, not get 
            // one from the cache
            if (this.Calculation.ContainsKey(calculationNode.Meta.Id)) then
                failwith "Invalid calculation structure. See code comments for more info";
            
            // We need to set the ID of this node. The IDs of nodes that pass through process
            // node are set here, to override any generated ID. Then after overriding the ID
            // we need to check for amendments to the node.
            calculationNode.Meta.Id <- new NodeId(id)

            // Add the node to the cache
            this.AddNode(calculationNode)

        // Return the cached node
        this.GetNode id
    
    // *****************************************************************************
    // *** Node constructors. These must be used to ensure that ALL nodes can be
    // *** amended as intended.
    // *****************************************************************************

    // In theory we can delete these now. They did have extra code in to push all node creation
    // through an amend node method. But not that amendments are managed in the calculation we
    // could get rid of them. Big rename job to do it though. Leave here for today! GPJ.

    member public this.ValueNode (nodeMeta:NodeMeta) (source:ValueSource) (value:decimal) =
        Node.Value(nodeMeta, source, value)

    member public this.DateValueNode (nodeMeta:NodeMeta) (value:DateTime) (reference:DateTime) =
        Node.DateValue(nodeMeta, value, reference)

    member public this.IfNode (nodeMeta:NodeMeta) (test:Node) (valueIfTrue:Node) (valueIfFalse:Node) =
        Node.If(nodeMeta, test, valueIfTrue, valueIfFalse)

    member public this.CompNode (nodeMeta:NodeMeta) (compOp:CompOp) (node1:Node) (node2:Node) =
        Node.Comp(nodeMeta, compOp, node1, node2)

    member public this.ListNode (nodeMeta:NodeMeta) (nodeOp:NodeOp) (nodes:list<Node>) =
        Node.List(nodeMeta, nodeOp, nodes)

    member public this.NegativeNode (node:Node) = 
        let meta = new NodeMeta(sprintf "Negative: %s" node.Name)
        meta.Id <- new NodeId(meta.Id)
        Node.Negative(meta, node)

    member public this.AbsoluteNode (node:Node) = 
        let meta = new NodeMeta(sprintf "Absolute: %s" node.Name)
        meta.Id <- new NodeId(meta.Id)
        Node.Absolute(meta, node)

    member public this.NotNode (node:Node) = 
        let meta = new NodeMeta(sprintf "Not: %s" node.Name)
        meta.Id <- new NodeId(meta.Id)
        Node.Not(meta, node)

    // *****************************************************************************
    // *** Utility members
    // *****************************************************************************

    member this.setAttr (name:string) (value:string) (target:Node) =
        let attribContainer = target.Meta :> (IAttributeContainer) 
        attribContainer.setAttr (name, value)
        target

    member this.setRounding ((calcPrecision:int), (displayPrecision:int), (style:RoundingStyle)) (target:Node) =
        target.Meta.RoundingRules <- { RoundingRules.CalculationPrecision = calcPrecision; DisplayPrecision = displayPrecision; RoundingStyle = style }
        target

    member this.isPercentage (target:Node) =
        target.Meta.DisplayAsPercentage <- true
        target

    member this.setNamespace (ns:string) (target:Node) =
        target.Meta.Namespace <- ns
        target

    member this.setComment (comment:string) (target:Node) =
        target.Meta.Comment <- comment
        target

    member this.conditional (check:bool) (call:Node -> Node) (target:Node) =
        if (check) then 
            call target
        else
            target

