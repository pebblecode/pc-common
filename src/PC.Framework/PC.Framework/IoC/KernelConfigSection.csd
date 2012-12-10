<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="fa4e1ee6-a018-4a16-87fe-4910cd6c6ee2" namespace="PebbleCode.Framework.IoC" xmlSchemaNamespace="urn:PebbleCode.Framework.IoC" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
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
    <configurationSection name="IoCKernelConfiguration" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="ioCKernelConfiguration">
      <elementProperties>
        <elementProperty name="BindingAssemblys" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="bindingAssemblys" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/fa4e1ee6-a018-4a16-87fe-4910cd6c6ee2/BindingAssemblys" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="BindingAssemblys" xmlItemName="assembly" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/fa4e1ee6-a018-4a16-87fe-4910cd6c6ee2/BindingAssembly" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="BindingAssembly">
      <attributeProperties>
        <attributeProperty name="Assembly" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="assembly" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/fa4e1ee6-a018-4a16-87fe-4910cd6c6ee2/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>