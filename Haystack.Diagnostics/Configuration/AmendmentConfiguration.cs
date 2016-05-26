using System.Collections.Generic;
using System.Xml.Serialization;
using Haystack.Core;
using Haystack.Diagnostics.Amendments;

namespace Haystack.Diagnostics.Configuration
{
    public sealed class AmendmentConfiguration : IAmendmentConfiguration
    {
        private readonly AmendmentConfigurationBuilder builder;

        public AmendmentConfiguration()
        {
            builder = new AmendmentConfigurationBuilder(this);
            AssembliesToAmend = new List<string>();
            HaystackPropertyAmendments = new List<TypeConfiguration>();
            HaystackConstructorAmendments = new List<TypeConfiguration>();
            HaystackMethodAmendments = new List<TypeConfiguration>();
            BeforePropertyGetAmendments = new List<TypeConfiguration>();
            AfterPropertyGetAmendments = new List<TypeConfiguration>();
            BeforePropertySetAmendments = new List<TypeConfiguration>();
            AfterPropertySetAmendments = new List<TypeConfiguration>();
            BeforeConstructorAmendments = new List<TypeConfiguration>();
            AfterConstructorAmendments = new List<TypeConfiguration>();
            CatchConstructorAmendments = new List<TypeConfiguration>();
            BeforeMethodAmendments = new List<TypeConfiguration>();
            AfterVoidMethodAmendments = new List<TypeConfiguration>();
            AfterMethodAmendments = new List<TypeConfiguration>();
            CatchVoidMethodAmendments = new List<TypeConfiguration>();
            CatchMethodAmendments = new List<TypeConfiguration>();
            FinallyMethodAmendments = new List<TypeConfiguration>();
        }

        [XmlArrayItem("Assembly")]
        public List<string> AssembliesToAmend { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<TypeConfiguration> HaystackPropertyAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<TypeConfiguration> HaystackConstructorAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<TypeConfiguration> HaystackMethodAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<TypeConfiguration> BeforePropertyGetAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<TypeConfiguration> AfterPropertyGetAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<TypeConfiguration> BeforePropertySetAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<TypeConfiguration> AfterPropertySetAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<TypeConfiguration> BeforeConstructorAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<TypeConfiguration> AfterConstructorAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<TypeConfiguration> CatchConstructorAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<TypeConfiguration> BeforeMethodAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<TypeConfiguration> AfterVoidMethodAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<TypeConfiguration> AfterMethodAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<TypeConfiguration> CatchVoidMethodAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<TypeConfiguration> CatchMethodAmendments { get; set; }

        [XmlArrayItem("AmendmentType")]
        public List<TypeConfiguration> FinallyMethodAmendments { get; set; }

        IEnumerable<string> IAmendmentConfiguration.AssembliesToAmend
        {
            get { return AssembliesToAmend; }
        }
        
        IEnumerable<IBeforePropertyGetAmender> IAmendmentConfiguration.BeforePropertyGetAmendments
        {
            get { return builder.BuildBeforePropertyGetAmenders(); }
        }

        IEnumerable<IAfterPropertyGetAmender> IAmendmentConfiguration.AfterPropertyGetAmendments
        {
            get { return builder.BuildAfterPropertyGetAmenders(); }
        }

        IEnumerable<IBeforePropertySetAmender> IAmendmentConfiguration.BeforePropertySetAmendments
        {
            get { return builder.BuildBeforePropertySetAmenders(); }
        }

        IEnumerable<IAfterPropertySetAmender> IAmendmentConfiguration.AfterPropertySetAmendments
        {
            get { return builder.BuildAfterPropertySetAmenders(); }
        }

        IEnumerable<IBeforeConstructorAmender> IAmendmentConfiguration.BeforeConstructorAmendments
        {
            get { return builder.BuildBeforeConstructorAmenders(); }
        }

        IEnumerable<IAfterConstructorAmender> IAmendmentConfiguration.AfterConstructorAmendments
        {
            get { return builder.BuildAfterConstructorAmenders(); }
        }

        IEnumerable<ICatchConstructorAmender> IAmendmentConfiguration.CatchConstructorAmendments
        {
            get { return builder.BuildCatchConstructorAmenders(); }
        }

        IEnumerable<IBeforeMethodAmender> IAmendmentConfiguration.BeforeMethodAmendments
        {
            get { return builder.BuildBeforeMethodAmenders(); }
        }

        IEnumerable<IAfterVoidMethodAmender> IAmendmentConfiguration.AfterVoidMethodAmendments
        {
            get { return builder.BuildAfterVoidMethodAmenders(); }
        }

        IEnumerable<IAfterMethodAmender> IAmendmentConfiguration.AfterMethodAmendments
        {
            get { return builder.BuildAfterMethodAmenders(); }
        }

        IEnumerable<ICatchVoidMethodAmender> IAmendmentConfiguration.CatchVoidMethodAmendments
        {
            get { return builder.BuildCatchVoidMethodAmenders(); }
        }

        IEnumerable<ICatchMethodAmender> IAmendmentConfiguration.CatchMethodAmendments
        {
            get { return builder.BuildCatchMethodAmenders(); }
        }

        IEnumerable<IFinallyMethodAmender> IAmendmentConfiguration.FinallyMethodAmendments
        {
            get { return builder.BuildFinallyMethodAmenders(); }
        }
    }
}
