namespace PebbleCode.ExpressionTree

open PebbleCode.Entities
open PebbleCode.Framework
open PebbleCode.Framework.Collections
open PebbleCode.Framework.Utilities

open System
    
[<AllowNullLiteralAttribute>]
type NodeId = 

    val private _name : string
    [<NonSerialized>]
    val private _entities : List<Entity>
    val mutable private _entitiesString : string option
    val mutable private _codeString : string option
    val mutable private _hash : int option
    val mutable private _amended : bool
    val mutable private _source : NodeIdSource
    val mutable private _codes : string list

    // *************************************************************************
    // ***  Constructor and copy constructor
    // *************************************************************************

    new (name:string, entities:List<Entity>) = {
        _name=name
        _entities = entities
        _entitiesString = None
        _codeString = None
        _hash = None
        _amended = false
        _source = NodeIdSource.Current
        _codes = []
        }

    new (name:string, codes: string list) = {
        _name=name
        _entities = List<Entity>.Empty
        _entitiesString = None
        _codeString = None
        _hash = None
        _amended = false
        _source = NodeIdSource.Current
        _codes = codes
        }

    new (nodeId:NodeId) = {
        _name=nodeId._name
        _entities = nodeId._entities
        _entitiesString = nodeId._entitiesString
        _codeString = nodeId._codeString
        _hash = nodeId._hash
        _amended = nodeId._amended
        _source = nodeId._source
        _codes = nodeId._codes
        }

    new (name:string, entities:string, codes:string, ammended:bool, source:NodeIdSource) = {
        _name=name
        _entities = List<Entity>.Empty
        _entitiesString = Some(entities)
        _codeString = Some(codes)
        _hash = None
        _amended = ammended
        _source = source
        _codes = []
        }

    // *************************************************************************
    // ***  static builders for convenience
    // *************************************************************************

    interface System.IComparable with
        member this.CompareTo(obj) =
            if this.Equals(obj) then
                0
            else
                obj.GetHashCode().CompareTo(this.GetHashCode())

    // *************************************************************************
    // ***  static builders for convenience
    // *************************************************************************
    static member Named name = new NodeId(name, List<Entity>.Empty)
    static member NamedAndEntity (name, entity: Entity) = new NodeId(name, [entity])
    static member NamedAndEntityList (name, entities: Entity list) = new NodeId(name, entities)

    // *************************************************************************
    // ***  Properties
    // *************************************************************************
    member this.Amended
        with get() = this._amended
        and set newValue = 
            this._amended <- newValue
            this._hash <- None  // Reset the hash

    member this.Source
        with get() = this._source
        and set newValue = 
            this._source <- newValue
            this._hash <- None  // Reset the hash

    member this.Name = this._name
    member this.Entities = this._entities

    member this.EntitiesString =
        if this._entitiesString.IsNone then
            this._entitiesString <- Some(List.fold (fun (str:string) (ent:Entity) -> str + sprintf "_e%d" ent.Identity) "" this._entities )
        this._entitiesString.Value

    member this.CodeString = 
        if this._codeString.IsNone then
            this._codeString <- Some((List.fold (fun (str:string) (code:string) -> str + sprintf "_%s" code) "" this._codes).TrimStart('_'))
        this._codeString.Value

    // *************************************************************************
    // ***  Node id matching.
    // *************************************************************************
    member this.Match (name:string) = 
        this._name = name

    member this.Match (entityType:Flags) = 
        this._name = entityType.ToString()

    member this.Match ((entityType:Flags),(name:string)) = 
        this._name = name
        && this._entities.Length = 1
        && this._entities.[0].EntityFlag = entityType

    // *************************************************************************
    // ***  Build and cache the hash of this node id
    // *************************************************************************
    member this.Hash = 
        if this._hash.IsNone then
            this._hash <- Some(this.ToString().GetHashCode32())
        this._hash.Value
        
    override this.GetHashCode() = this.Hash
    override this.Equals(obj) = obj.ToString().Equals(this.ToString())

    override this.ToString() = 
        sprintf "Node:%O:%s:%s_%s%s" 
            this._source
            (match this._amended with | true -> "T" | false -> "F")
            this._name
            this.EntitiesString
            this.CodeString
        
    // Override operator equality so we can use a = b notation in C#
    static member op_Equality (lhs:NodeId, rhs:NodeId) =
        match lhs,rhs with
        | null, null -> true
        | null, _ -> false
        | _, null -> false
        | _, _ -> lhs.Equals(rhs)
