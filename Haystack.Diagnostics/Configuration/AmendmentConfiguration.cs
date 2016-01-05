using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Haystack.Diagnostics.Amendments;

namespace Haystack.Diagnostics.Configuration
{
    public sealed class AmendmentConfiguration : IAmendmentConfiguration
    {
        public AmendmentConfiguration()
        {
            Assemblies = new List<string>();
            HaystackPropertyAmendments = new List<string>();
            HaystackConstructorAmendments = new List<string>();
            HaystackMethodAmendments = new List<string>();
            BeforePropertyGetAmendments = new List<string>();
            AfterPropertyGetAmendments = new List<string>();
            BeforePropertySetAmendments = new List<string>();
            AfterPropertySetAmendments = new List<string>();
            BeforeConstructorAmendments = new List<string>();
            AfterConstructorAmendments = new List<string>();
            CatchConstructorAmendments = new List<string>();
            BeforeMethodAmendments = new List<string>();
            AfterVoidMethodAmendments = new List<string>();
            AfterMethodAmendments = new List<string>();
            CatchVoidMethodAmendments = new List<string>();
            CatchMethodAmendments = new List<string>();
            FinallyMethodAmendments = new List<string>();
        }

        [XmlArrayItem("Assembly")]
        public List<string> Assemblies { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<string> HaystackPropertyAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<string> HaystackConstructorAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<string> HaystackMethodAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<string> BeforePropertyGetAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<string> AfterPropertyGetAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<string> BeforePropertySetAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<string> AfterPropertySetAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<string> BeforeConstructorAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<string> AfterConstructorAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<string> CatchConstructorAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<string> BeforeMethodAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<string> AfterVoidMethodAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<string> AfterMethodAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<string> CatchVoidMethodAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<string> CatchMethodAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<string> FinallyMethodAmendments { get; set; }

        IEnumerable<string> IAmendmentConfiguration.Assemblies
        {
            get { return Assemblies; }
        }

        IEnumerable<IPropertyAmender> IAmendmentConfiguration.HaystackPropertyAmendments
        {
            get { return TypeResolver.CreateInstances<IPropertyAmender>(HaystackPropertyAmendments); }
        }

        IEnumerable<IConstructorAmender> IAmendmentConfiguration.HaystackConstructorAmendments
        {
            get { return TypeResolver.CreateInstances<IConstructorAmender>(HaystackConstructorAmendments); }
        }

        IEnumerable<IMethodAmender> IAmendmentConfiguration.HaystackMethodAmendments
        {
            get { return TypeResolver.CreateInstances<IMethodAmender>(HaystackMethodAmendments); }
        }

        IEnumerable<IBeforePropertyGetAmender> IAmendmentConfiguration.BeforePropertyGetAmendments
        {
            get { return TypeResolver.CreateInstances<IBeforePropertyGetAmender>(BeforePropertyGetAmendments); }
        }

        IEnumerable<IAfterPropertyGetAmender> IAmendmentConfiguration.AfterPropertyGetAmendments
        {
            get { return TypeResolver.CreateInstances<IAfterPropertyGetAmender>(AfterPropertyGetAmendments); }
        }

        IEnumerable<IBeforePropertySetAmender> IAmendmentConfiguration.BeforePropertySetAmendments
        {
            get { return TypeResolver.CreateInstances<IBeforePropertySetAmender>(BeforePropertySetAmendments); }
        }

        IEnumerable<IAfterPropertySetAmender> IAmendmentConfiguration.AfterPropertySetAmendments
        {
            get { return TypeResolver.CreateInstances<IAfterPropertySetAmender>(AfterPropertySetAmendments); }
        }

        IEnumerable<IBeforeConstructorAmender> IAmendmentConfiguration.BeforeConstructorAmendments
        {
            get { return TypeResolver.CreateInstances<IBeforeConstructorAmender>(BeforeConstructorAmendments); }
        }

        IEnumerable<IAfterConstructorAmender> IAmendmentConfiguration.AfterConstructorAmendments
        {
            get { return TypeResolver.CreateInstances<IAfterConstructorAmender>(AfterConstructorAmendments); }
        }

        IEnumerable<ICatchConstructorAmender> IAmendmentConfiguration.CatchConstructorAmendments
        {
            get { return TypeResolver.CreateInstances<ICatchConstructorAmender>(CatchConstructorAmendments); }
        }

        IEnumerable<IBeforeMethodAmender> IAmendmentConfiguration.BeforeMethodAmendments
        {
            get { return TypeResolver.CreateInstances<IBeforeMethodAmender>(BeforeMethodAmendments); }
        }

        IEnumerable<IAfterVoidMethodAmender> IAmendmentConfiguration.AfterVoidMethodAmendments
        {
            get { return TypeResolver.CreateInstances<IAfterVoidMethodAmender>(AfterVoidMethodAmendments); }
        }

        IEnumerable<IAfterMethodAmender> IAmendmentConfiguration.AfterMethodAmendments
        {
            get { return TypeResolver.CreateInstances<IAfterMethodAmender>(AfterMethodAmendments); }
        }

        IEnumerable<ICatchVoidMethodAmender> IAmendmentConfiguration.CatchVoidMethodAmendments
        {
            get { return TypeResolver.CreateInstances<ICatchVoidMethodAmender>(CatchVoidMethodAmendments); }
        }

        IEnumerable<ICatchMethodAmender> IAmendmentConfiguration.CatchMethodAmendments
        {
            get { return TypeResolver.CreateInstances<ICatchMethodAmender>(CatchMethodAmendments); }
        }

        IEnumerable<IFinallyMethodAmender> IAmendmentConfiguration.FinallyMethodAmendments
        {
            get { return TypeResolver.CreateInstances<IFinallyMethodAmender>(FinallyMethodAmendments); }
        }
    }
}
