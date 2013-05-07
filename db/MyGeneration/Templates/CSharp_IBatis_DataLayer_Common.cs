    // GUI assigned variables
    private string _assemblyName;
	private string _rootNamespace;
    private string _entityNamespace;
    private string _dbName;
    private bool _addReadOnlyAccessors;

    // Variables set per iteration of table
    private Source _workingSource;
    private IColumns _columns;
    private string _tableName;
    private string _className;
    private string _listClassName;
	private string _fieldIdsType;
    private bool _isVersioned;
    private bool _isControlledUpdate;
    private IColumn _pk;
    private bool _hasSummaryFields;
    private bool _doingSummary;
    private string _historyTable;
    private bool _createIncompleteClass;
    private string _versionColumnName = "version_no";
    private string _timestampColumnName = "version_date";
    private string _propertyAuthorizationName = "property_authorization";

    private void AssignMemberVariables()
    {
        ITable currentTable = input["currentSource"] as ITable;
        if (currentTable != null)
            _workingSource = new TableSource(currentTable);
        else
        {
            IView currentView = input["currentSource"] as IView;
            if (currentView != null)
                _workingSource = new ViewSource(currentView);
        }

        if (_workingSource == null)
            throw new Exception("no current table or view");

        
        // Grab some value from the input
        _assemblyName = input["assemblyName"].ToString();
        _dbName = input["chooseDatabase"].ToString();
        _rootNamespace = input["rootNamespace"].ToString();
        _entityNamespace = input["classNamespace"].ToString();
        _doingSummary = input.Contains("doingSummary") ? (bool)input["doingSummary"] : false;
        _addReadOnlyAccessors = false; //(bool)input["chkReadOnlyClass"];

		// Basic flags and values for table
        _columns = _workingSource.Columns;
        _tableName = _workingSource.Alias.Replace(" ", "");
        _className = ToClassName(_tableName );
		_listClassName = GetListClassName(_className);
		_fieldIdsType = _className + "Fields";
        _isVersioned = IsVersioned(_workingSource.Columns);
        _isControlledUpdate = IsControlledUpdate(_workingSource.Columns);
        _pk = GetPrimaryKey(_workingSource.Columns);
        _hasSummaryFields = HasSummaryFields(_workingSource.Columns);
        _historyTable = GetHistoryTable(_workingSource);

		// Override some flags for summary
		if (_doingSummary)
		{
			_className = _className + "Summary";
			_isVersioned = false;
        }
    }
    
    private string ToClassName(string tableName)
    {
        return ToClassName(tableName, _workingSource.Properties);
    }
    
    private string ToClassName(string tableName, IPropertyCollection properties)
    {
        if (properties.ContainsKey("ClassName"))
            return properties["ClassName"].Value;

        string className = null;

        // Remove S from end of words, and end of string
        className = tableName.Replace("s_", "_");
        className = className.Replace("fitnes_", "fitness_");
        className = className.Replace("onu_", "onus_");
        if (className.EndsWith("sses") || className.EndsWith("ches"))
            className = className.Substring(0, className.Length - 2);
        else if (className.EndsWith("s"))
            className = className.Substring(0, className.Length - 1);

        // Remove vw_ from the start of names... for views
        // and add View to the end
        if (className.StartsWith("vw_"))
            className = className.Substring(3) + "View";

        // Pascalise what's left
        return ToPascalCase(className);
    }

    private string ToPascalCase(string name)
    {
        string notStartingAlpha = Regex.Replace(name, "^[^a-zA-Z]+", "");
        return RemoveSeparatorAndCapNext(notStartingAlpha, true);
    }

    private string ToVariableName(string name)
    {
        string result = RemoveSeparatorAndCapNext(name, false);
        if (result == "event")
			return "evt";
		return result;
    }

    private string RemoveSeparatorAndCapNext(string input, bool pascal)
    {
        string dashUnderscore = "-_";
        string workingString = input;
        char[] chars = workingString.ToCharArray();
        int under = workingString.IndexOfAny(dashUnderscore.ToCharArray());
        while (under > -1)
        {
            chars[under + 1] = Char.ToUpper(chars[under + 1], CultureInfo.InvariantCulture);
            workingString = new String(chars);
            under = workingString.IndexOfAny(dashUnderscore.ToCharArray(), under + 1);
        }

        if (pascal) chars[0] = Char.ToUpper(chars[0], CultureInfo.InvariantCulture);
        else chars[0] = Char.ToLower(chars[0], CultureInfo.InvariantCulture);
        workingString = new string(chars);
        return Regex.Replace(workingString, "[-_]", "");
    }

    private IColumn GetPrimaryKey(IColumns Columns)
    {
        // See if it has been overridden?
        IColumn pkOverride = GetPrimaryKeyOverride();
        if (pkOverride != null)
            return pkOverride;

        // See if there is an actual primary key
        foreach (IColumn c in Columns)
            if (c.IsInPrimaryKey) return c;

        // See if there is an ID column
        foreach (IColumn c in Columns)
            if (c.Name.ToLower() == "id") return c;

        // Must not be one
        return null;
    }

    private string GetDeleteAllOrderBy()
    {
        if (_workingSource.Properties.ContainsKey("deleteAllOrder"))
        {
            return "order by " + _workingSource.Properties["deleteAllOrder"].Value;
        }
        return "";
    }

    private IColumn GetPrimaryKeyOverride()
    {
        if (_workingSource.Properties.ContainsKey("PrimaryKeyOverride"))
        {
            foreach(IColumn c in _workingSource.Columns)
            {
                if (c.Name == _workingSource.Properties["PrimaryKeyOverride"].Value) 
                    return c;
            }
        }
        return null;
    }

    private bool HasSummaryFields(IColumns columns)
    {
        foreach (IColumn column in columns)
        {
            if (IsSummaryField(column))
                return true;
        }
        return false;
    }

    private string GetHistoryTable(Source source)
    {
        if (source.Properties.ContainsKey("HistoryTable"))
            return source.Properties["HistoryTable"].Value;
        else
            return null;
    }

	private bool IsSummaryField(IColumn column)
	{
		return column.Properties.ContainsKey("SummaryField");
	}

    private bool IsVersioned(IColumns Columns)
    {
        bool containsVersionCol = false;
        bool containsTimestampCol = false;
        foreach (IColumn c in Columns)
        {
            if (c.Name == _versionColumnName)
                containsVersionCol = true;
            if (c.Name == _timestampColumnName)
                containsTimestampCol = true;
        }
        return containsVersionCol && containsTimestampCol;
    }

    private bool IsControlledUpdate(IColumns Columns)
    {
        foreach (IColumn c in Columns)
        {
            if (c.Name == _propertyAuthorizationName) return true;
        }
        return false;
    }

    private bool IsVersionField(IColumn column)
    {
        return (_isVersioned &&
            (column.Name == _versionColumnName || column.Name == _timestampColumnName));
    }

    private bool IsVersionedOrControlledUpdateField(IColumn column)
    {
        return IsVersionField(column) || column.Name == _propertyAuthorizationName;
    }

	private bool ValidateField(IColumn column)
	{
		if ( IsExcluded(column) ) return false;
		if ( IsVersionField(column)) return false;
		if ( IsKeyField(column) ) return false;
		return true;
	}
	
    private string ColumnToMemberVariable(IColumn column)
    {
        return "_" + ToVariableName(UniqueColumn(column));
    }

    private string ToMemberVariable(string name)
    {
        return "_" + ToVariableName(name);
    }

    private string ToVariable(IColumn column)
    {
        return ToVariableName(UniqueColumn(column));
    }

    private string ColumnToPropertyName(IColumn column)
    {
        return ToPropertyName(UniqueColumn(column));
    }

    private string ToPropertyName(string name)
    {
        return ToPascalCase(name);
    }

    private string ToConstantName(string name)
    {
        return SplitOnCapitals(ToPascalCase(name), "_").ToUpper();
    }

    private string ColumnToDisplayName(IColumn column)
    {
		if (column.Properties.ContainsKey("displayName"))
			return column.Properties["displayName"].Value;
        return SplitOnCapitals(ToPascalCase(UniqueColumn(column)));
    }
	
	private string SplitOnCapitals(string stringToSplit)
	{
        return SplitOnCapitals(stringToSplit, " ");
	}
	
	private string SplitOnCapitals(string stringToSplit, string separator)
	{
		return Regex.Replace(stringToSplit, @"([a-z])([A-Z])", @"$1" + separator + "$2", RegexOptions.None);
	}

    private string ColumnToVariableType(IColumn column)
    {
        return ColumnToMemberType(column);
    }

    private string ColumnToMemberType(IColumn column)
    {
        string stdType = ColumnToDbPropertyType(column);
        bool isNullable = stdType.EndsWith("?");

        if (IsIntDate(column) && isNullable)
            return "DateTime?";
        else if (IsIntDate(column) && !isNullable)
            return "DateTime";
        else if (IsFlags(column) && isNullable)
            return "Flags?";
        else if (IsFlags(column) && !isNullable)
            return "Flags";
		else if (IsEnum(column) && isNullable)
			return EnumType(column) + "?";
		else if (IsEnum(column) && !isNullable)
			return EnumType(column);
        else if (IsSerialised(column))
            return SerialisedType(column);
        else
            return stdType;
    }
		
	private string ColumnToDbPropertyName( IColumn column )
	{
		return ToDbPropertyName( UniqueColumn( column ) );
	}

    private string ToDbPropertyName(string name)
    {
		return "Db" + ToPascalCase( name );
    }

    private string ColumnToDbPropertyType(IColumn column)
    {
        string retVal = column.LanguageType.Trim();
        if (column.IsNullable && IsValueType(column))
        {
            retVal = retVal + "?";
        }
        return retVal;
    }

    private bool IsValueType(IColumn column)
    {
        string type = column.LanguageType.Trim();
        return (type == "int" ||
              type == "long" ||
              type == "short" ||
              type == "uint" ||
              type == "ulong" ||
              type == "ushort" ||
              type == "double" ||
              type == "decimal" ||
              type == "DateTime" ||
              type == "bool");
    }

    private bool IsIntDate(IColumn column)
    {
        return column.Properties.ContainsKey("IntDate");
    }

    private bool IsFlags(IColumn column)
    {
        return column.Properties.ContainsKey("FlagsType");
    }

    private string FlagsType(IColumn column)
    {
        return column.Properties["FlagsType"].Value;
    }

    private bool IsEnum(IColumn column)
    {
        return column.Properties.ContainsKey("EnumType");
    }

    private string EnumType(IColumn column)
    {
        return column.Properties["EnumType"].Value;
    }

    private bool IsSerialised(IColumn column)
    {
        return column.Properties.ContainsKey("SerialisedType");
    }

    private string SerialisedType(IColumn column)
    {
        return column.Properties["SerialisedType"].Value;
    }

    private string InitialValue(IColumn column)
    {
        if (column.Properties.ContainsKey("InitialValue"))
            return column.Properties["InitialValue"].Value;
        return null;
    }

    private string UniqueColumn(IColumn column)
    {
        string c = column.Alias.Replace(" ", "");
        if (column.Table != null && column.Table.Alias.Replace(" ", "") == c)
        {
            c += "Name";
        }
        if (column.View != null && column.View.Alias.Replace(" ", "") == c)
        {
            c += "Name";
        }
        return c;
    }

    private bool IsKeyField(IColumn field)
    {
        return (field.IsInPrimaryKey || field.IsInForeignKey);
    }
	
	private string GetListClassName(string entityClassName)
	{
		return entityClassName + "List";
	}
	
	private bool IsExcluded(IColumn column)
	{
		return column.Properties.ContainsKey("Excluded") || 
			(_doingSummary && !IsSummaryField(column));
	}

    private List<IIndex> GetUniqueIndexes()
    {
        List<IIndex> indexes = new List<IIndex>();
        if (_workingSource.Indexes != null)
        {
            foreach(IIndex index in _workingSource.Indexes)
            {
                if (index.Unique == false ||
                    (index.Columns.Count == 1 && index.Columns[0] == _pk))
                    continue;
                indexes.Add(index);
            }
        }
        return indexes;
    }

    private List<IForeignKey> GetNonPrimaryForeignKeys()
    {
        List<IForeignKey> foreignKeys = new List<IForeignKey>();
        if (_workingSource.ForeignKeys != null)
        {
	        foreach(IForeignKey foreignKey in _workingSource.ForeignKeys)
	        {
                // Only look at one col -> one col keys
		        if (foreignKey.PrimaryColumns.Count != 1 || foreignKey.ForeignColumns.Count != 1)
			        continue;
				    
                // Skip keys originating on this tables primary key
                // Unless both ends of the key are on this table
		        if (foreignKey.PrimaryColumns[0].Table == _workingSource.Table 
                    && foreignKey.PrimaryColumns[0].Name == _pk.Name
                    && foreignKey.ForeignColumns[0].Table != _workingSource.Table)
			        continue;
            
                // Add this key.
                foreignKeys.Add(foreignKey);
	        }
        }
        return foreignKeys;
    }

    private List<IForeignKey> GetPrimaryForeignKeys()
    {
        List<IForeignKey> foreignKeys = new List<IForeignKey>();
        if (_workingSource.ForeignKeys != null)
        {
		    foreach(IForeignKey foreignKey in _workingSource.ForeignKeys)
		    {
                // Only look at one col -> one col keys
			    if (foreignKey.PrimaryColumns.Count != 1 || foreignKey.ForeignColumns.Count != 1)
				    continue;
				    
                // Skip keys not originating on this tables primary key
			    if (foreignKey.PrimaryColumns[0].Table != _workingSource.Table 
                    || foreignKey.PrimaryColumns[0].Name != _pk.Name)
				    continue;
            
                // Add this key.
                foreignKeys.Add(foreignKey);
		    }
        }
        return foreignKeys;
    }

    private string GetFKeyName(Source source)
    {
        string name = source.Name;
        name = name.TrimEnd('s');
        return name + "_id";
    }

    private string GetFKeyName(ITable table)
    {
        string name = table.Name;
        if (name.EndsWith("ses"))
            name = name.Substring(0, name.Length - 2);
        else
            name = name.TrimEnd('s');
        name = name.Replace("s_", "_");  // Drop mid name plurals (for join tables)
        return name + "_id";
    }

    private class ResultMap
    {
        public string Id;
        public string Name;
        public string Method;
    }

    private List<ResultMap> GetResultMaps(IColumn column)
    {
        List<ResultMap> resultMaps = new List<ResultMap>();
        ResultMap defaultMap = new ResultMap();
        defaultMap.Id = _className + "By" + ToPascalCase(column.Name);
        defaultMap.Name = _className + "Result";
        defaultMap.Method = "By" + ToPascalCase(column.Name);
        resultMaps.Add(defaultMap);

        if (_workingSource.Properties.ContainsKey("additionalResultMaps") && _workingSource.Properties["additionalResultMaps"].Value != null)
        {
            foreach (string map in _workingSource.Properties["additionalResultMaps"].Value.Split(',', ';'))
            {
                ResultMap resultMap = new ResultMap();
                resultMap.Id =  _className + map.Replace(_className, "") + "By" +ToPascalCase(column.Name);
                resultMap.Name = map;
                resultMap.Method = "By" + ToPascalCase(column.Name) + map.Replace(_className, "");
                resultMaps.Add(resultMap);
            }
        }
        return resultMaps;
    }

    private List<ResultMap> GetResultMaps(string name)
    {
        List<ResultMap> resultMaps = new List<ResultMap>();
        ResultMap defaultMap = new ResultMap();
        defaultMap.Id = _className + name;
        defaultMap.Name = _className + "Result";
        defaultMap.Method = name;
        resultMaps.Add(defaultMap);

        if (_workingSource.Properties.ContainsKey("additionalResultMaps") && _workingSource.Properties["additionalResultMaps"].Value != null)
        {
            foreach (string map in _workingSource.Properties["additionalResultMaps"].Value.Split(',', ';'))
            {
                ResultMap resultMap = new ResultMap();
                resultMap.Id = _className + map.Replace(_className, "") + name;
                resultMap.Name = map;
                resultMap.Method = name + map.Replace(_className, "");
                resultMaps.Add(resultMap);
            }
        }
        return resultMaps;
    }

    private class KeyDetails
    {
        public string ColumnName;
		public string ClassName;
		public string ListClassName;
		public string MemberName;
		public string PropertyName; // The name of the BO property
		public string KeyPropertyName; // The name of the foreign key property
		public string KeyPropertyType; // The type of the foreign key property

        public bool ForeignKeyIsNullable { get { return KeyPropertyType.EndsWith("?"); } }
        public string KeyPropertyNameWithValue { get { return ForeignKeyIsNullable ? KeyPropertyName + ".Value" : KeyPropertyName; } }
    }

    private KeyDetails GetForeignKeyDetails(IForeignKey foreignKey)
    {
        KeyDetails result = new KeyDetails();
        result.ClassName = ToClassName(foreignKey.ForeignColumns[0].Table.Name, foreignKey.ForeignTable.Properties);
		result.ListClassName = GetListClassName(result.ClassName);
        result.ColumnName = foreignKey.ForeignColumns[0].Name;
        result.KeyPropertyType = ColumnToMemberType(foreignKey.ForeignColumns[0]);
        result.KeyPropertyName = ColumnToPropertyName(foreignKey.ForeignColumns[0]);

        // Using an alias?
        if (foreignKey.Alias != foreignKey.Name)
        {
            // I DON'T THINK THIS WORKS AT THE MO
		    result.MemberName = ToMemberVariable(foreignKey.Alias);
		    result.PropertyName = ToPropertyName(foreignKey.Alias);
        }
        else
        {
		    // Is the foreign column name more than just xxxx_id, where xxx is our table name?
		    string suffix = result.ColumnName.ToLower().Replace(GetFKeyName(foreignKey.PrimaryColumns[0].Table).ToLower(), "").TrimEnd('_');
		    if (suffix.Length > 0)
			    suffix = "Using" + ToPropertyName(suffix);
				
		    result.MemberName = ToMemberVariable(result.ListClassName) + suffix;
		    result.PropertyName = ToPropertyName(result.ListClassName) + suffix;
        }
        
        return result;
    }

    private KeyDetails GetPrimaryKeyDetails(IForeignKey foreignKey)
    {
        KeyDetails result = new KeyDetails();
        result.ClassName = ToClassName(foreignKey.PrimaryColumns[0].Table.Name, foreignKey.PrimaryColumns[0].Table.Properties);
		result.ListClassName = GetListClassName(result.ClassName);
        result.ColumnName = foreignKey.ForeignColumns[0].Name;
        result.KeyPropertyType = ColumnToMemberType(foreignKey.ForeignColumns[0]);
        result.KeyPropertyName = ColumnToPropertyName(foreignKey.ForeignColumns[0]);

        // Using an alias?
        if (foreignKey.Alias != foreignKey.Name)
        {
            // I DON'T THINK THIS WORKS AT THE MO
		    result.MemberName = ToMemberVariable(foreignKey.Alias);
		    result.PropertyName = ToPropertyName(foreignKey.Alias);
        }
        else
        {
		    // Is the foreign column name more than just xxxx_id, where xxx is our table name?
		    string prefix = result.ColumnName.ToLower().Replace(GetFKeyName(foreignKey.PrimaryColumns[0].Table).ToLower(), "").TrimEnd('_');
				
		    result.MemberName = ToMemberVariable(prefix + result.ClassName);
		    result.PropertyName = ToPropertyName(prefix + result.ClassName);
        }

        return result;
    }

    // Helper class to allow us to treat ITable and IView the same
    private abstract class Source
    {
        public abstract bool IsTable { get; }
        public abstract bool IsView { get; }
        public abstract string Alias { get; }
        public abstract string Description { get; }
        public abstract string Name { get; }
        public abstract IColumns Columns { get; }
        public abstract IIndexes Indexes { get; }
        public abstract IForeignKeys ForeignKeys { get; }
        public abstract IColumns PrimaryKeys { get; }
        public abstract IPropertyCollection Properties { get; }
        public abstract ITable Table { get; }
        public abstract IView View { get; }
    }

    private class TableSource : Source
    {
        public ITable _table;

        public TableSource(ITable table)
        {
            _table = table;
        }

        public override bool IsTable { get { return true; } }
        public override bool IsView { get { return false; } }
        public override string Alias { get { return _table.Alias; } }
        public override string Description { get { return _table.Description; } }
        public override string Name { get { return _table.Name; } }
        public override IColumns Columns { get { return _table.Columns; } }
        public override IIndexes Indexes { get { return _table.Indexes; } }
        public override IForeignKeys ForeignKeys { get { return _table.ForeignKeys; } }
        public override IColumns PrimaryKeys { get { return _table.PrimaryKeys; } }
        public override IPropertyCollection Properties { get { return _table.Properties; } }
        public override ITable Table { get { return _table; } }
        public override IView View { get { return null; } }
    }

    private class ViewSource : Source
    {
        private IView _view;

        public ViewSource(IView view)
        {
            _view = view;
        }

        public override bool IsTable { get { return false; } }
        public override bool IsView { get { return true; } }
        public override string Alias { get { return _view.Alias; } }
        public override string Description { get { return _view.Description; } }
        public override string Name { get { return _view.Name; } }
        public override IColumns Columns { get { return _view.Columns; } }
        public override IIndexes Indexes { get { return null; } }
        public override IForeignKeys ForeignKeys { get { return null; } }
        public override IColumns PrimaryKeys { get { return null; } }
        public override IPropertyCollection Properties { get { return _view.Properties; } }
        public override ITable Table { get { return null; } }
        public override IView View { get { return _view; } }
    }
    
    private Dictionary<string, List<LookupColumnInfo>> GetMultiColumnListLookUps()
    {
        return GetMultiColumnLookUps("ListLookUp");
    }

    private Dictionary<string, List<LookupColumnInfo>> GetMultiColumnEntityLookUps()
    {
        return GetMultiColumnLookUps("EntityLookUp");
    }

    private Dictionary<string, List<LookupColumnInfo>> GetMultiColumnLookUps(string value)
    {
		Dictionary<string, List<LookupColumnInfo>> multiColumnLookUps = new Dictionary<string, List<LookupColumnInfo>>();
		foreach (IColumn column in _columns)
		{
            foreach (IProperty property in column.Properties)
            {
                if (property.Value == value
                    || (property.Value.StartsWith(value) && property.Value[value.Length] == ',' ))
                {
                    // strip the comparison
                    string comparison = "";
                    if (property.Value != value)
                        comparison = property.Value.Substring(value.Length+1);
                    if (comparison == "")
                        comparison = "=";
                    
                    // Make sure key is in dictionary
                    if (!multiColumnLookUps.ContainsKey(property.Key))
                        multiColumnLookUps.Add(property.Key, new List<LookupColumnInfo>());

                    // Add info for this column
                    LookupColumnInfo columnInfo = new LookupColumnInfo();
                    columnInfo.Column = column;
                    columnInfo.Comparison = comparison;
                    multiColumnLookUps[property.Key].Add(columnInfo);
                }
            }
		}
        return multiColumnLookUps;
    }

    private class LookupColumnInfo
    {
        public IColumn Column;
		public string Comparison;
        public string XmlSafeComparison
        {
            get
            {
                return Comparison.Replace("<", "&lt;").Replace(">", "&gt;");
            }
        }
    }

    private class ParameterMetadata
    {
        private readonly Dictionary<string, string> _settings = new Dictionary<string, string>();

        public string ResultMap { get { return _settings.ContainsKey("ResultMap") ? _settings["ResultMap"] : null; } }
        public string Operator { get { return _settings.ContainsKey("Operator") ? _settings["Operator"] : "="; } }
        public string FnStyle { get { return _settings.ContainsKey("FnStyle") ? _settings["FnStyle"] : null; } }

        public ParameterMetadata(string settingsString)
        {
            if (string.IsNullOrEmpty(settingsString)) return;

            foreach (string setting in settingsString.Split('|'))
            {
                string[] values = setting.Split(':');
                if (values.Length == 2)
                {
                    if (!setting.Contains(values[0]))
                    {
                        _settings.Add(values[0], values[1]);
                    }
                    else
                    {
                        _settings[values[0]] = values[1];
                    }
                }
            }
        }
    }