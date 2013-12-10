/*****************************************************************************/
/***                                                                       ***/
/***    This is an automatically generated file. It is generated using     ***/
/***    MyGeneration in conjunction with IBatisBusinessObject template.    ***/
/***                                                                       ***/
/***    DO NOT MODIFY THIS FILE DIRECTLY!                                  ***/
/***                                                                       ***/
/***    If you need to make changes either modify the template and         ***/
/***    regenerate or derive a class from this class and override.         ***/
/***                                                                       ***/
/*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using PebbleCode.Framework.Collections;
using PebbleCode.Entities;
using PebbleCode.Framework.Dates;
using PebbleCode.Framework.Utilities;

namespace PebbleCode.Entities.Tests.Integration.Entities
{
	/// <summary>
	///	Generated by MyGeneration using the IBatis Object Mapping template
	/// </summary>
	[Serializable]
	public partial class ControlledUpdateThing : ConcreteControlledUpdateEntity
	{
        /// <summary>
        /// Get the entity type
        /// </summary>
		public override Flags EntityFlag { get { return EntityType.ControlledUpdateThing; } }
		
		#region Private Members
		[DataMember]
		private int _id;
		[DataMember]
		private string _name;
		#endregion
		
		/// <summary>
        /// Constructor
		/// </summary>
		public ControlledUpdateThing()
		{
			// Initialise database field values
			OnCreated();
		}
		
		
		
		#region Public Properties
			
		/// <summary>
		/// Public accessor for _id
		/// </summary>		
		public virtual int Id
		{
			get { return _id; }
		}
		
		/// <summary>
		/// Public accessor for _name
		/// </summary>		
		public virtual string Name
		{
			get { return _name; }
			set
			{
				if (value != null) 
				{
					value = value.Trim();
				}
			  
				if (_name != value && PropertyValueChanging("Name", value))
				{
					_name = value;
					PropertyValueChanged("Name");
				}
			}
		}
		
		#endregion
		
		#region Database Properties
			
		/// <summary>
		/// Database accessor for _id. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal int DbId
		{
			get { return _id;  }
			set { _id = value; }
		}

		/// <summary>
		/// Database accessor for _name. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal string DbName
		{
			get { return _name;  }
			set { _name = value; }
		}

		#endregion
		
		
		
		#region IClonable Members
		
		/// <summary>
		///	Returns a copy of the instance of ControlledUpdateThing
		/// </summary>
		/// <remarks>
		/// Overrides the base version, but calls OnClone to allow base classes chance
		/// to clone their information
		/// </remarks>
		public override object Clone()
        {
			// All of the value types are cloned using memberwise clone
			ControlledUpdateThing clone = (ControlledUpdateThing)this.MemberwiseClone();

			// Copy any other instances which are not known about by myGen
			OnClone(clone);
		
			// Return our pristine clone!
            return clone;
		}
		
		#endregion		
		
		#region Test Utilities
		
		/// <summary>
		/// Used for testing - allows stub objects to be created simply.  Ensure that
		/// the test assembly's name is added to AssemblyInfo (in your projects entities 
		/// dll - e.g. CT.Entities) inside an InternalsVisisbleToAttribute
		/// </summary>
		/// <param name="id">ID to use for test stub</param>
		internal virtual void SetId(int id)
		{
			_id = id;
			_isNew = false;
		}
		
		#endregion		
		
		/// <summary>
		/// Overriden method to get the Id of the entity
		/// </summary>
		public override int Identity
		{
			get { return _id; }
		}
	}

    /// <summary>
    /// Event args class for ControlledUpdateThing
    /// </summary>
    public class ControlledUpdateThingEventArgs : EventArgs
    {
        public ControlledUpdateThing ControlledUpdateThing { get; private set; }

        public ControlledUpdateThingEventArgs(ControlledUpdateThing controlledUpdateThing)
        {
            ControlledUpdateThing = controlledUpdateThing;
        }
    }
}