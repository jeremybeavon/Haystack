﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Haystack.Diagnostics.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Haystack.Diagnostics.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;Project xmlns=&quot;http://schemas.microsoft.com/developer/msbuild/2003&quot; DefaultTargets=&quot;MakeAssemblyAmendments&quot;&gt;
        ///	&lt;Target Name=&quot;MakeAssemblyAmendments&quot;&gt;
        ///		&lt;Error Text=&quot;AssemblyPath property is not found.&quot; Condition=&quot;&apos;$(AssemblyPath)&apos; == &apos;&apos;&quot; /&gt;
        ///    &lt;Error Text=&quot;StrongNameKey property is not found.&quot; Condition=&quot;&apos;$(StrongNameKey)&apos; == &apos;&apos;&quot; /&gt;
        ///		&lt;PropertyGroup&gt;
        ///			&lt;IldasmExe&gt;&amp;quot;C:\Program Files (x86)\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools\ildasm.exe&amp;quot;&lt;/IldasmExe&gt;
        ///			&lt;IlasmExe&gt;&amp;quot;C:\Windows [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AmendmentsStrongNameSetup {
            get {
                return ResourceManager.GetString("AmendmentsStrongNameSetup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;Project DefaultTargets=&quot;ExecuteProcess&quot; xmlns=&quot;http://schemas.microsoft.com/developer/msbuild/2003&quot;&gt;
        ///  &lt;Target Name=&quot;ExecuteProcess&quot;&gt;
        ///    &lt;Error Text=&quot;Command property is not found&quot; Condition=&quot;&apos;$(Command)&apos; == &apos;&apos;&quot; /&gt;
        ///    &lt;Exec Command=&quot;$(Command)&quot; WorkingDirectory=&quot;$(WorkingDirectory)&quot; /&gt;
        ///  &lt;/Target&gt;
        ///&lt;/Project&gt;.
        /// </summary>
        internal static string ExecuteProcess {
            get {
                return ResourceManager.GetString("ExecuteProcess", resourceCulture);
            }
        }
    }
}
