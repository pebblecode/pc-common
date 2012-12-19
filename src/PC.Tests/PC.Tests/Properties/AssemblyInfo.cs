using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("CT.Bloomberg.Tests")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Pebble IT")]
[assembly: AssemblyProduct("The System")]
[assembly: AssemblyCopyright("Copyright Pebble IT 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// Make our test libraries friends of this library. Allows access
// to internal members
[assembly: InternalsVisibleTo("PC.Tests.Unit")]
[assembly: InternalsVisibleTo("PC.Tests.Integration")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("be1e2a2a-aa4c-4f9d-887a-d9dbbb232fa6")]

[assembly: AssemblyVersion("0.0.0.0")]
[assembly: AssemblyFileVersion("0.0.0.0")]
