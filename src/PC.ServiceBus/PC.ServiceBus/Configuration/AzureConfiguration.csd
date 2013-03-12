<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="ddb6a769-afe9-421b-93a9-704412236da7" namespace="PC.ServiceBus.Configuration" xmlSchemaNamespace="urn:PC.ServiceBus.Configuration" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
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
    <configurationSection name="AzureConfiguration" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="azureConfiguration">
      <elementProperties>
        <elementProperty name="Settings" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="settings" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/ddb6a769-afe9-421b-93a9-704412236da7/Settings" />
          </type>
        </elementProperty>
        <elementProperty name="Topics" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="topics" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/ddb6a769-afe9-421b-93a9-704412236da7/topics" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="Settings" xmlItemName="add" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/ddb6a769-afe9-421b-93a9-704412236da7/Add" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="Add">
      <attributeProperties>
        <attributeProperty name="Key" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="key" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/ddb6a769-afe9-421b-93a9-704412236da7/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Value" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="value" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/ddb6a769-afe9-421b-93a9-704412236da7/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="topics" xmlItemName="topic" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementCollectionMoniker name="/ddb6a769-afe9-421b-93a9-704412236da7/topic" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="subscription">
      <attributeProperties>
        <attributeProperty name="name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/ddb6a769-afe9-421b-93a9-704412236da7/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="topic" xmlItemName="subscription" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <attributeProperties>
        <attributeProperty name="name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/ddb6a769-afe9-421b-93a9-704412236da7/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="isEventBus" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="isEventBus" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/ddb6a769-afe9-421b-93a9-704412236da7/Boolean" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <itemType>
        <configurationElementMoniker name="/ddb6a769-afe9-421b-93a9-704412236da7/subscription" />
      </itemType>
    </configurationElementCollection>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>