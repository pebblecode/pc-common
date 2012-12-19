namespace PebbleCode.ExpressionCalculator

open System
open System.Collections.Generic
open System.Runtime.Serialization

open PebbleCode.ExpressionTree
open PebbleCode.Framework
open PebbleCode.Framework.Dates

[<Serializable>]
type Calculation = 
    inherit Dictionary<NodeId, Node>

    // *****************************************************************************
    // *** Constructor
    // *****************************************************************************

    val mutable private _amendments:AmendmentMap
    val private _evaluatedEvent:Event<EventHandler, EventArgs>

    // Default constructor
    new () = {
        inherit Dictionary<NodeId, Node>()
        _amendments = new AmendmentMap()
        _evaluatedEvent = new Event<EventHandler, EventArgs>()
        }
        
    // Construct with amendments
    new(amendments:Amendment list) as this = 
        Calculation()
        then
            for amendment in amendments do
                this._amendments.Add(amendment.NodeId, amendment)

    // *****************************************************************************
    // *** Support for serialise/deserialise
    // *****************************************************************************

    new(info:SerializationInfo, ctx:StreamingContext) = {
        inherit Dictionary<NodeId, Node>(info, ctx)
        _amendments = info.GetValue("_amendments", typeof<AmendmentMap>) :?> AmendmentMap
        _evaluatedEvent = new Event<EventHandler, EventArgs>()
        }

    interface ISerializable with
        member this.GetObjectData(info:SerializationInfo, context:StreamingContext) =
            if info = null then raise(ArgumentNullException("info"))
            base.GetObjectData(info, context)
            info.AddValue("_amendments", this._amendments)

    // *****************************************************************************
    // *** Events
    // *****************************************************************************

    member private this.Evaluated = this._evaluatedEvent
    member public this.EvaluatedEvent = this.Evaluated.Publish

    // *****************************************************************************
    // *** Root node
    // *****************************************************************************

    static member public RootNodeId:NodeId = NodeId.Named("Root")
    member public this.RootNode = this.[Calculation.RootNodeId]

    // *****************************************************************************
    // *** Amendments
    // *****************************************************************************

    member public this.AddAmendment(amendment:Amendment) =
        this._amendments.[amendment.NodeId] <- amendment
        this.Evaluate()
        
    member public this.RemoveAmendment (nodeId:NodeId) =
        if this._amendments.Remove(nodeId) then
            this.Evaluate()
            true
        else
            false

    member public this.RemoveAllAmendments() =
        this._amendments.Clear()
        this.Evaluate()
        
    member public this.GetAmendments() = 
        List.map (fun x -> x) (this._amendments.Values.ToFSharpList())
        
    // *****************************************************************************
    // *** Evaluate: evaluates all nodes and fires associated event
    // *****************************************************************************

    member public this.Evaluate() =
        //Is there anything to evaluate?
        if this.Values.Count > 0 then
            // apply amendments into the main value collection, 
            // clearing cached values at same time
            for node in this.Values do
                if this._amendments.ContainsKey(node.Id) then
                    node.Meta.Override <- Some(this._amendments.[node.Id].Value)
                else
                    node.Meta.Override <- None
                node.Meta.CachedValue <- None // clear cache at same time

            // evaluate the root node which will trickle to all nodes in the tree
            let rootValue = this.[Calculation.RootNodeId].Evaluate()

            // let everyone know we did an evaluate
            this.Evaluated.Trigger(this, new EventArgs())

    // *****************************************************************************
    // *** Methods
    // *****************************************************************************

    member public this.ContainsNode nodeId =
        this.ContainsKey(nodeId)

    /// <summary>
    /// Get the precise value of a node, if it exists, null otherwise
    /// </summary>
    member public this.GetPreciseValueOrNull(nodeId:NodeId) =
        if this.ContainsKey(nodeId) then
            new Nullable<decimal>(this.[nodeId].PreciseValue);
        else
            new Nullable<decimal>()

    /// <summary>
    /// Check the integrity of the dictionary. The ID of all nodes must match the
    /// key they are indexed against.
    /// </summary>
    member public this.IntegrityCheck() =
        for nodeId in this.Keys do
            let node = this.[nodeId]
            if node.Id.GetHashCode() <> nodeId.GetHashCode() then
                failwith "Integrity of the NodeDictionary is not in tact. Check calculation structure"

    /// <summary>
    /// Get a filtered dict with only nodes which match the predicate
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    member public this.Filter(predicate:NodeId -> bool) =
        let result = new Calculation(this.GetAmendments())
        for key in this.Keys do
            if predicate key then
                result.Add(key, this.[key]);
        result

    /// <summary>
    /// Simplify this node dictionary into a new node dictionary of value nodes
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    member public this.SimplifyToValues(newValueSource:ValueSource, newNodeSource:NodeIdSource) =
        let result = new Calculation(this.GetAmendments())
        for key in this.Keys do
            let simplifiedNode = this.[key].SimplifyToValue newValueSource newNodeSource
            // Debug code
            //System.Diagnostics.Debug.WriteLine("{0}\t\t{1}", key, simplifiedNode);
            //if result.ContainsKey(simplifiedNode.Id) then
                //Node other = result[simplifiedNode.Id];
            result.Add(simplifiedNode.Id, simplifiedNode)
        result

        /// <summary>
        /// Clones this calculation, including all nodes and amendments
        /// </summary>
        /// <returns></returns>
    member public this.Clone() =
        let clone = new Calculation()
        for nodeId in this.Keys do
            clone.Add(nodeId, this.[nodeId].Clone)
        clone._amendments <- this._amendments.Clone()
        clone

