using Haystack.Analysis;
using Haystack.Analyzer.Web.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;

namespace Haystack.Analyzer.Web.Controllers
{
    public class HaystackController : ApiController
    {
        // GET: api/Haystack
        public IEnumerable<HaystackMethodItem> Get()
        {
            return HaystackAnalysisProvider
                .Load(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"App_Data\haystackAnalysis")).HaystackMethods
                .Select(method => new HaystackMethodItem(method));
        }
    }
}
