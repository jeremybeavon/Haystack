using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;

namespace Haystack.Analyzer.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
