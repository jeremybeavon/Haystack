using System.Collections.Generic;
using System.Xml.Serialization;
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
        public List<string> AssembliesToAmend { get; set; }

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
