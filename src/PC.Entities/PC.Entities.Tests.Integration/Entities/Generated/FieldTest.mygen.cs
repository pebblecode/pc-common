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
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Linq;

using PebbleCode.Framework.Collections;
using PebbleCode.Framework.Dates;
using PebbleCode.Framework.Utilities;
using PebbleCode.Entities;

namespace PC.Entities.Tests.Integration.Entities
{
	/// <summary>
	///	Generated by MyGeneration using the IBatis Object Mapping template
	/// </summary>
	[Serializable]
	public partial class FieldTest : Entity<int>
	{
        /// <summary>
        /// Get the entity type
        /// </summary>
		public override Flags EntityFlag { get { return EntityType.FieldTest; } }
		
		#region Private Members
		[DataMember]
		private int _id;
		[DataMember]
		private int _intField;
		[DataMember]
		private int? _intFieldNullable;
		[DataMember]
		private decimal _decimalField;
		[DataMember]
		private decimal? _decimalFieldNullable;
		[DataMember]
		private string _stringField;
		[DataMember]
		private string _stringFieldNullable;
		[DataMember]
		private string _textField;
		[DataMember]
		private string _textFieldNullable;
		[DataMember]
		private DateTime _datetimeField;
		[DataMember]
		private DateTime? _datetimeFieldNullable;
		[DataMember]
		private bool _tinyintField;
		[DataMember]
		private bool? _tinyintFieldNullable;
		[DataMember]
		private DateTime _timestampField;
		[DataMember]
		private DateTime? _timestampFieldNullable;
		[DataMember]
		private TestEnum _enumField;
		[DataMember]
		private TestEnum? _enumFieldNullable;
		[DataMember]
		private PropertyBag _objectField = new PropertyBag();
		[DataMember]
		private PropertyBag _objectFieldNullable = null;
		[DataMember]
		private int _foreignKeyField;
		[DataMember]
		private int? _foreignKeyFieldNullable;
		[DataMember]
		private DateTime _intDateField;
		[DataMember]
		private DateTime? _intDateFieldNullable;
		[DataMember]
		private int _indexedField;
		[DataMember]
		private int? _indexedFieldNullable;
		[DataMember]
		private string _nodeIdField;
		[DataMember]
		private string _nodeIdFieldNullable;
		[DataMember]
		private int _defaultValueIsTwo;
		#endregion
		
		/// <summary>
        /// Constructor
		/// </summary>
		public FieldTest()
		{
			// Initialise database field values
			OnCreated();
		}
		
		#region Foreign Key Entities
				
        /// <summary>
        /// Get the associated ForeignKeyFieldWidget
        /// </summary>
        public Widget ForeignKeyFieldWidget
        {
            get 
            {
                if (!_foreignKeyFieldWidgetPopulated)
                    throw new InvalidOperationException("_foreignKeyFieldWidget not populated yet");
                return _foreignKeyFieldWidget;
            }
			set
			{
				_foreignKeyFieldWidget = value;
				_foreignKeyFieldWidgetPopulated = true;
				ForeignKeyField = value.Identity;
			}
        }
		
		[DataMember]
		private Widget _foreignKeyFieldWidget = null;
		[DataMember]
		private bool _foreignKeyFieldWidgetPopulated = false;
		
        /// <summary>
        /// Is ForeignKeyFieldWidget getable yet?
        /// </summary>
        public bool ForeignKeyFieldWidgetPopulated
        {
            get { return _foreignKeyFieldWidgetPopulated; }
        }
		
        /// <summary>
        /// Does ForeignKeyFieldWidget require population?
        /// </summary>
        public bool ForeignKeyFieldWidgetRequiresPopulation
        {
            get 
			{
				return _foreignKeyFieldWidgetPopulated == false; 
			}
        }
		
        /// <summary>
        /// Clear ForeignKeyFieldWidget and mark as not populated anymore
        /// </summary>
        public void UnpopulateForeignKeyFieldWidget()
        {
			_foreignKeyFieldWidgetPopulated = false;
			_foreignKeyFieldWidget = null;
        }
				
        /// <summary>
        /// Get the associated ForeignKeyFieldNullableWidget
        /// </summary>
        public Widget ForeignKeyFieldNullableWidget
        {
            get 
            {
                if (!_foreignKeyFieldNullableWidgetPopulated)
                    throw new InvalidOperationException("_foreignKeyFieldNullableWidget not populated yet");
                return _foreignKeyFieldNullableWidget;
            }
			set
			{
				_foreignKeyFieldNullableWidget = value;
				_foreignKeyFieldNullableWidgetPopulated = true;
				ForeignKeyFieldNullable = value != null ? value.Identity : (int?)null;
			}
        }
		
		[DataMember]
		private Widget _foreignKeyFieldNullableWidget = null;
		[DataMember]
		private bool _foreignKeyFieldNullableWidgetPopulated = false;
		
        /// <summary>
        /// Is ForeignKeyFieldNullableWidget getable yet?
        /// </summary>
        public bool ForeignKeyFieldNullableWidgetPopulated
        {
            get { return _foreignKeyFieldNullableWidgetPopulated; }
        }
		
        /// <summary>
        /// Does ForeignKeyFieldNullableWidget require population?
        /// </summary>
        public bool ForeignKeyFieldNullableWidgetRequiresPopulation
        {
            get 
			{
				return _foreignKeyFieldNullableWidgetPopulated == false && ForeignKeyFieldNullable.HasValue;
			}
        }
		
        /// <summary>
        /// Clear ForeignKeyFieldNullableWidget and mark as not populated anymore
        /// </summary>
        public void UnpopulateForeignKeyFieldNullableWidget()
        {
			_foreignKeyFieldNullableWidgetPopulated = false;
			_foreignKeyFieldNullableWidget = null;
        }
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// Public accessor for _id
		/// </summary>		
		public virtual int Id
		{
			get { return _id; }
		}
		
		/// <summary>
		/// Public accessor for _intField
		/// </summary>		
		public virtual int IntField
		{
			get { return _intField; }
			set
			{
				if (_intField != value && PropertyValueChanging("IntField", value))
				{
					_intField = value;
					PropertyValueChanged("IntField");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _intFieldNullable
		/// </summary>		
		public virtual int? IntFieldNullable
		{
			get { return _intFieldNullable; }
			set
			{
				if (_intFieldNullable != value && PropertyValueChanging("IntFieldNullable", value))
				{
					_intFieldNullable = value;
					PropertyValueChanged("IntFieldNullable");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _decimalField
		/// </summary>		
		public virtual decimal DecimalField
		{
			get { return _decimalField; }
			set
			{
				if (_decimalField != value && PropertyValueChanging("DecimalField", value))
				{
					_decimalField = value;
					PropertyValueChanged("DecimalField");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _decimalFieldNullable
		/// </summary>		
		public virtual decimal? DecimalFieldNullable
		{
			get { return _decimalFieldNullable; }
			set
			{
				if (_decimalFieldNullable != value && PropertyValueChanging("DecimalFieldNullable", value))
				{
					_decimalFieldNullable = value;
					PropertyValueChanged("DecimalFieldNullable");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _stringField
		/// </summary>		
		public virtual string StringField
		{
			get { return _stringField; }
			set
			{
				if (value != null) 
				{
					value = value.Trim();
				}
			  
				if (_stringField != value && PropertyValueChanging("StringField", value))
				{
					_stringField = value;
					PropertyValueChanged("StringField");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _stringFieldNullable
		/// </summary>		
		public virtual string StringFieldNullable
		{
			get { return _stringFieldNullable; }
			set
			{
				if (value != null) 
				{
					value = value.Trim();
				}
			  
				if (_stringFieldNullable != value && PropertyValueChanging("StringFieldNullable", value))
				{
					_stringFieldNullable = value;
					PropertyValueChanged("StringFieldNullable");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _textField
		/// </summary>		
		public virtual string TextField
		{
			get { return _textField; }
			set
			{
				if (value != null) 
				{
					value = value.Trim();
				}
			  
				if (_textField != value && PropertyValueChanging("TextField", value))
				{
					_textField = value;
					PropertyValueChanged("TextField");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _textFieldNullable
		/// </summary>		
		public virtual string TextFieldNullable
		{
			get { return _textFieldNullable; }
			set
			{
				if (value != null) 
				{
					value = value.Trim();
				}
			  
				if (_textFieldNullable != value && PropertyValueChanging("TextFieldNullable", value))
				{
					_textFieldNullable = value;
					PropertyValueChanged("TextFieldNullable");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _datetimeField
		/// </summary>		
		public virtual DateTime DatetimeField
		{
			get { return _datetimeField; }
			set
			{
				if (_datetimeField != value && PropertyValueChanging("DatetimeField", value))
				{
					_datetimeField = value;
					PropertyValueChanged("DatetimeField");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _datetimeFieldNullable
		/// </summary>		
		public virtual DateTime? DatetimeFieldNullable
		{
			get { return _datetimeFieldNullable; }
			set
			{
				if (_datetimeFieldNullable != value && PropertyValueChanging("DatetimeFieldNullable", value))
				{
					_datetimeFieldNullable = value;
					PropertyValueChanged("DatetimeFieldNullable");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _tinyintField
		/// </summary>		
		public virtual bool TinyintField
		{
			get { return _tinyintField; }
			set
			{
				if (_tinyintField != value && PropertyValueChanging("TinyintField", value))
				{
					_tinyintField = value;
					PropertyValueChanged("TinyintField");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _tinyintFieldNullable
		/// </summary>		
		public virtual bool? TinyintFieldNullable
		{
			get { return _tinyintFieldNullable; }
			set
			{
				if (_tinyintFieldNullable != value && PropertyValueChanging("TinyintFieldNullable", value))
				{
					_tinyintFieldNullable = value;
					PropertyValueChanged("TinyintFieldNullable");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _timestampField
		/// </summary>		
		public virtual DateTime TimestampField
		{
			get { return _timestampField; }
			set
			{
				if (_timestampField != value && PropertyValueChanging("TimestampField", value))
				{
					_timestampField = value;
					PropertyValueChanged("TimestampField");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _timestampFieldNullable
		/// </summary>		
		public virtual DateTime? TimestampFieldNullable
		{
			get { return _timestampFieldNullable; }
			set
			{
				if (_timestampFieldNullable != value && PropertyValueChanging("TimestampFieldNullable", value))
				{
					_timestampFieldNullable = value;
					PropertyValueChanged("TimestampFieldNullable");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _enumField
		/// </summary>		
		public virtual TestEnum EnumField
		{
			get { return _enumField; }
			set
			{
				if (_enumField != value && PropertyValueChanging("EnumField", value))
				{
					_enumField = value;
					PropertyValueChanged("EnumField");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _enumFieldNullable
		/// </summary>		
		public virtual TestEnum? EnumFieldNullable
		{
			get { return _enumFieldNullable; }
			set
			{
				if (_enumFieldNullable != value && PropertyValueChanging("EnumFieldNullable", value))
				{
					_enumFieldNullable = value;
					PropertyValueChanged("EnumFieldNullable");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _objectField
		/// </summary>		
		public virtual PropertyBag ObjectField
		{
			get { return _objectField; }
			set
			{
				if (_objectField != value && PropertyValueChanging("ObjectField", value))
				{
					_objectField = value;
					PropertyValueChanged("ObjectField");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _objectFieldNullable
		/// </summary>		
		public virtual PropertyBag ObjectFieldNullable
		{
			get { return _objectFieldNullable; }
			set
			{
				if (_objectFieldNullable != value && PropertyValueChanging("ObjectFieldNullable", value))
				{
					_objectFieldNullable = value;
					PropertyValueChanged("ObjectFieldNullable");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _foreignKeyField
		/// </summary>		
		public virtual int ForeignKeyField
		{
			get { return _foreignKeyField; }
			set
			{
				if (_foreignKeyField != value && PropertyValueChanging("ForeignKeyField", value))
				{
					_foreignKeyField = value;
					PropertyValueChanged("ForeignKeyField");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _foreignKeyFieldNullable
		/// </summary>		
		public virtual int? ForeignKeyFieldNullable
		{
			get { return _foreignKeyFieldNullable; }
			set
			{
				if (_foreignKeyFieldNullable != value && PropertyValueChanging("ForeignKeyFieldNullable", value))
				{
					_foreignKeyFieldNullable = value;
					PropertyValueChanged("ForeignKeyFieldNullable");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _intDateField
		/// </summary>		
		public virtual DateTime IntDateField
		{
			get { return _intDateField; }
			set
			{
				if (_intDateField != value && PropertyValueChanging("IntDateField", value))
				{
					_intDateField = value;
					PropertyValueChanged("IntDateField");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _intDateFieldNullable
		/// </summary>		
		public virtual DateTime? IntDateFieldNullable
		{
			get { return _intDateFieldNullable; }
			set
			{
				if (_intDateFieldNullable != value && PropertyValueChanging("IntDateFieldNullable", value))
				{
					_intDateFieldNullable = value;
					PropertyValueChanged("IntDateFieldNullable");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _indexedField
		/// </summary>		
		public virtual int IndexedField
		{
			get { return _indexedField; }
			set
			{
				if (_indexedField != value && PropertyValueChanging("IndexedField", value))
				{
					_indexedField = value;
					PropertyValueChanged("IndexedField");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _indexedFieldNullable
		/// </summary>		
		public virtual int? IndexedFieldNullable
		{
			get { return _indexedFieldNullable; }
			set
			{
				if (_indexedFieldNullable != value && PropertyValueChanging("IndexedFieldNullable", value))
				{
					_indexedFieldNullable = value;
					PropertyValueChanged("IndexedFieldNullable");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _nodeIdField
		/// </summary>		
		public virtual string NodeIdField
		{
			get { return _nodeIdField; }
			set
			{
				if (value != null) 
				{
					value = value.Trim();
				}
			  
				if (_nodeIdField != value && PropertyValueChanging("NodeIdField", value))
				{
					_nodeIdField = value;
					PropertyValueChanged("NodeIdField");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _nodeIdFieldNullable
		/// </summary>		
		public virtual string NodeIdFieldNullable
		{
			get { return _nodeIdFieldNullable; }
			set
			{
				if (value != null) 
				{
					value = value.Trim();
				}
			  
				if (_nodeIdFieldNullable != value && PropertyValueChanging("NodeIdFieldNullable", value))
				{
					_nodeIdFieldNullable = value;
					PropertyValueChanged("NodeIdFieldNullable");
				}
			}
		}
		
		/// <summary>
		/// Public accessor for _defaultValueIsTwo
		/// </summary>		
		public virtual int DefaultValueIsTwo
		{
			get { return _defaultValueIsTwo; }
			set
			{
				if (_defaultValueIsTwo != value && PropertyValueChanging("DefaultValueIsTwo", value))
				{
					_defaultValueIsTwo = value;
					PropertyValueChanged("DefaultValueIsTwo");
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
		/// Database accessor for _intField. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal int DbIntField
		{
			get { return _intField;  }
			set { _intField = value; }
		}

		/// <summary>
		/// Database accessor for _intFieldNullable. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal int? DbIntFieldNullable
		{
			get { return _intFieldNullable;  }
			set { _intFieldNullable = value; }
		}

		/// <summary>
		/// Database accessor for _decimalField. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal decimal DbDecimalField
		{
			get { return _decimalField;  }
			set { _decimalField = value; }
		}

		/// <summary>
		/// Database accessor for _decimalFieldNullable. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal decimal? DbDecimalFieldNullable
		{
			get { return _decimalFieldNullable;  }
			set { _decimalFieldNullable = value; }
		}

		/// <summary>
		/// Database accessor for _stringField. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal string DbStringField
		{
			get { return _stringField;  }
			set { _stringField = value; }
		}

		/// <summary>
		/// Database accessor for _stringFieldNullable. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal string DbStringFieldNullable
		{
			get { return _stringFieldNullable;  }
			set { _stringFieldNullable = value; }
		}

		/// <summary>
		/// Database accessor for _textField. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal string DbTextField
		{
			get { return _textField;  }
			set { _textField = value; }
		}

		/// <summary>
		/// Database accessor for _textFieldNullable. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal string DbTextFieldNullable
		{
			get { return _textFieldNullable;  }
			set { _textFieldNullable = value; }
		}

		/// <summary>
		/// Database accessor for _datetimeField. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal DateTime DbDatetimeField
		{
			get { return _datetimeField;  }
			set { _datetimeField = value; }
		}

		/// <summary>
		/// Database accessor for _datetimeFieldNullable. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal DateTime? DbDatetimeFieldNullable
		{
			get { return _datetimeFieldNullable;  }
			set { _datetimeFieldNullable = value; }
		}

		/// <summary>
		/// Database accessor for _tinyintField. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal bool DbTinyintField
		{
			get { return _tinyintField;  }
			set { _tinyintField = value; }
		}

		/// <summary>
		/// Database accessor for _tinyintFieldNullable. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal bool? DbTinyintFieldNullable
		{
			get { return _tinyintFieldNullable;  }
			set { _tinyintFieldNullable = value; }
		}

		/// <summary>
		/// Database accessor for _timestampField. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal DateTime DbTimestampField
		{
			get { return _timestampField;  }
			set { _timestampField = value; }
		}

		/// <summary>
		/// Database accessor for _timestampFieldNullable. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal DateTime? DbTimestampFieldNullable
		{
			get { return _timestampFieldNullable;  }
			set { _timestampFieldNullable = value; }
		}

		/// <summary>
		/// Database accessor for _enumField. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal int DbEnumField
		{
			get { return (int)_enumField;  }
			set { _enumField = (TestEnum)value; }
		}

		/// <summary>
		/// Database accessor for _enumFieldNullable. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal int? DbEnumFieldNullable
		{
			get { return (int?)_enumFieldNullable;  }
			set { _enumFieldNullable = (TestEnum?)value; }
		}

		/// <summary>
		/// Database accessor for _objectField. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal byte[] DbObjectField
		{
			get { return SerialisationUtils.ToBinary<PropertyBag>(_objectField);  }
			set { _objectField = SerialisationUtils.FromBinary<PropertyBag>(value); }
		}

		/// <summary>
		/// Database accessor for _objectFieldNullable. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal byte[] DbObjectFieldNullable
		{
			get { return SerialisationUtils.ToBinary<PropertyBag>(_objectFieldNullable);  }
			set { _objectFieldNullable = SerialisationUtils.FromBinary<PropertyBag>(value); }
		}

		/// <summary>
		/// Database accessor for _foreignKeyField. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal int DbForeignKeyField
		{
			get { return _foreignKeyField;  }
			set { _foreignKeyField = value; }
		}

		/// <summary>
		/// Database accessor for _foreignKeyFieldNullable. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal int? DbForeignKeyFieldNullable
		{
			get { return _foreignKeyFieldNullable;  }
			set { _foreignKeyFieldNullable = value; }
		}

		/// <summary>
		/// Database accessor for _intDateField. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal int DbIntDateField
		{
			get { return DbConvert.ToDateInt(_intDateField);  }
			set { _intDateField = DbConvert.FromDateInt(value); }
		}

		/// <summary>
		/// Database accessor for _intDateFieldNullable. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal int? DbIntDateFieldNullable
		{
			get { return DbConvert.ToDateInt(_intDateFieldNullable);  }
			set { _intDateFieldNullable = DbConvert.FromDateInt(value); }
		}

		/// <summary>
		/// Database accessor for _indexedField. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal int DbIndexedField
		{
			get { return _indexedField;  }
			set { _indexedField = value; }
		}

		/// <summary>
		/// Database accessor for _indexedFieldNullable. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal int? DbIndexedFieldNullable
		{
			get { return _indexedFieldNullable;  }
			set { _indexedFieldNullable = value; }
		}

		/// <summary>
		/// Database accessor for _nodeIdField. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal string DbNodeIdField
		{
			get { return _nodeIdField;  }
			set { _nodeIdField = value; }
		}

		/// <summary>
		/// Database accessor for _nodeIdFieldNullable. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal string DbNodeIdFieldNullable
		{
			get { return _nodeIdFieldNullable;  }
			set { _nodeIdFieldNullable = value; }
		}

		/// <summary>
		/// Database accessor for _defaultValueIsTwo. Only used by IBatis.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal int DbDefaultValueIsTwo
		{
			get { return _defaultValueIsTwo;  }
			set { _defaultValueIsTwo = value; }
		}

		#endregion
		
		
		
		#region IClonable Members
		
		/// <summary>
		///	Returns a copy of the instance of FieldTest
		/// </summary>
		/// <remarks>
		/// Overrides the base version, but calls OnClone to allow base classes chance
		/// to clone their information
		/// </remarks>
		public override object Clone()
        {
			// All of the value types are cloned using memberwise clone
			FieldTest clone = (FieldTest)this.MemberwiseClone();

			// Copy any other instances which are not known about by myGen
			OnClone(clone);
			
			// Copy reference objects
			clone._objectField = (PropertyBag)this._objectField.Clone();
			clone._objectFieldNullable = (PropertyBag)this._objectFieldNullable.Clone();
		
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
    /// Event args class for FieldTest
    /// </summary>
    public class FieldTestEventArgs : EventArgs
    {
        public FieldTest FieldTest { get; private set; }

        public FieldTestEventArgs(FieldTest fieldTest)
        {
            FieldTest = fieldTest;
        }
    }
}