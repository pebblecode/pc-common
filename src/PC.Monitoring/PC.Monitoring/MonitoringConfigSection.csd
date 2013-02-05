<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="2e48f4fb-cc78-45c4-8f46-469122d07e7a" namespace="PC.Monitoring" xmlSchemaNamespace="urn:PC.Monitoring" assemblyName="PC.Monitoring" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="MonitoringConfiguration" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="monitoringConfiguration">
      <elementProperties>
        <elementProperty name="serviceMonitorGroups" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="serviceMonitorGroups" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/2e48f4fb-cc78-45c4-8f46-469122d07e7a/ServiceMonitorGroups" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="ServiceMonitorConfiguration">
      <attributeProperties>
        <attributeProperty name="Type" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/2e48f4fb-cc78-45c4-8f46-469122d07e7a/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/2e48f4fb-cc78-45c4-8f46-469122d07e7a/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Settings" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="settings" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/2e48f4fb-cc78-45c4-8f46-469122d07e7a/Settings" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="ServiceMonitorGroups" xmlItemName="serviceMonitorGroup" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/2e48f4fb-cc78-45c4-8f46-469122d07e7a/ServiceMonitorGroup" />
      </itemType>
    </configurationElementCollection>
    <configurationElementCollection name="Settings" xmlItemName="add" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/2e48f4fb-cc78-45c4-8f46-469122d07e7a/Add" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="Add">
      <attributeProperties>
        <attributeProperty name="Key" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="key" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/2e48f4fb-cc78-45c4-8f46-469122d07e7a/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Value" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="value" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/2e48f4fb-cc78-45c4-8f46-469122d07e7a/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="ServiceMonitorGroup">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/2e48f4fb-cc78-45c4-8f46-469122d07e7a/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="ServiceMonitors" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="serviceMonitors" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/2e48f4fb-cc78-45c4-8f46-469122d07e7a/ServiceMonitors" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="ServiceMonitors" xmlItemName="serviceMonitorConfiguration" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/2e48f4fb-cc78-45c4-8f46-469122d07e7a/ServiceMonitorConfiguration" />
      </itemType>
    </configurationElementCollection>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>