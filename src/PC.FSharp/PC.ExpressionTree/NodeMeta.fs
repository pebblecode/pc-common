namespace PebbleCode.ExpressionTree

open System.Collections.Generic
open PebbleCode.Entities
    
// *****************************************************************************
// *** 
// *** Meta data for a node in the tree
// *** 
// *****************************************************************************

[<System.ComponentModel.TypeConverterAttribute("CT.Client.ValuationEditor.NodeMetaExpandableObjectConverter,  CT.Client.ValuationEditor")>]
type NodeMeta = 

    // *************************************************************************
    // ***  Private Members
    // *************************************************************************
    val mutable private _name : string
    val private _attributes : Dictionary<string, string>
    val mutable private _roundingRules : RoundingRules
    val mutable private _cachedValue : decimal option
    val mutable private _nodeId : NodeId
    val mutable private _displayAsPercentage : bool
    val mutable private _namespace : string
    val mutable private _comment: string
    val mutable private _override: decimal option
    val mutable private _nodeFormat: NodeFormat
    
    // *************************************************************************
    // ***  Constructor and copy constructor
    // *************************************************************************
    new (name:string) = {
        _name=name
        _attributes = new Dictionary<string, string>()
        _roundingRules = { RoundingRules.CalculationPrecision = -1; RoundingRules.DisplayPrecision = 2; RoundingRules.RoundingStyle = RoundingStyle.Round }
        _cachedValue = None
        _nodeId = NodeId.Named(name)
        _displayAsPercentage = false
        _namespace = ""
        _comment = ""
        _override = None
        _nodeFormat = NodeFormat.Numeric
        }

    new (nodeId:NodeId) = {
        _name=nodeId.Name
        _attributes = new Dictionary<string, string>()
        _roundingRules = { RoundingRules.CalculationPrecision = -1; RoundingRules.DisplayPrecision = 2; RoundingRules.RoundingStyle = RoundingStyle.Round }
        _cachedValue = None
        _nodeId = new NodeId(nodeId)
        _displayAsPercentage = false
        _namespace = ""
        _comment = ""
        _override = None
        _nodeFormat = NodeFormat.Numeric
        }

    new (name:string, source:NodeMeta) = {
        _name=name
        _attributes = new Dictionary<string, string>(source._attributes)
        _roundingRules = source._roundingRules
        _cachedValue = source._cachedValue
        _nodeId = new NodeId(source._nodeId)
        _displayAsPercentage = source._displayAsPercentage
        _namespace = source._namespace
        _comment = source._comment
        _override = source._override
        _nodeFormat = NodeFormat.Numeric
        }

    new (name:string, nodeFormat:NodeFormat) = {
        _name=name
        _attributes = new Dictionary<string, string>()
        _roundingRules = { RoundingRules.CalculationPrecision = -1; RoundingRules.DisplayPrecision = 2; RoundingRules.RoundingStyle = RoundingStyle.Round }
        _cachedValue = None
        _nodeId = NodeId.Named(name)
        _displayAsPercentage = false
        _namespace = ""
        _comment = ""
        _override = None
        _nodeFormat = nodeFormat
        }

    // *************************************************************************
    // ***  Static constructor method to construct a new meta for an ammened value
    // *************************************************************************
    static member ForAmendment (source:NodeMeta) =
        let meta = new NodeMeta(sprintf "*%s" source.Name, source)
        meta._cachedValue <- None
        meta._roundingRules <- { RoundingRules.CalculationPrecision = -1; RoundingRules.DisplayPrecision = source._roundingRules.DisplayPrecision; RoundingRules.RoundingStyle = source._roundingRules.RoundingStyle }
        meta

    static member Constant (value:decimal) =
        new NodeMeta(sprintf "Value: %f" value)
        
    // *************************************************************************
    // ***  Get/set/check attributes
    // *************************************************************************    
    member this.getAttr (name:string) = this._attributes.[name]
    member this.hasAttr (name:string) = this._attributes.ContainsKey name
    member this.allAttr = new Dictionary<string, string>(this._attributes)
    interface IAttributeContainer with
        member this.setAttr (name:string, value:string) = this._attributes.[name] <- value

    // *************************************************************************
    // ***  Read/write properties
    // *************************************************************************
    member this.Name
        with get() = this._name
        and set newValue = this._name <- newValue

    member this.CachedValue
        with get() = this._cachedValue
        and set newValue = this._cachedValue <- newValue

    member this.Id
        with get() = this._nodeId
        and set newValue = this._nodeId <- newValue

    member this.RoundingRules
        with get() = this._roundingRules
        and set newValue = this._roundingRules <- newValue

    member this.DisplayAsPercentage
        with get() = this._displayAsPercentage
        and set newValue = this._displayAsPercentage <- newValue

    member this.Namespace
        with get() = this._namespace
        and set newValue = this._namespace <- newValue

    member this.Comment
        with get() = this._comment
        and set newValue = this._comment <- newValue

    member this.Override
        with get() = this._override
        and set newValue = this._override <- newValue

    member this.NodeFormat
        with get() = this._nodeFormat
        and set newValue = this._nodeFormat <- newValue
