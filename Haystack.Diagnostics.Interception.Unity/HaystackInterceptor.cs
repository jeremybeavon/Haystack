using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace Haystack.Diagnostics.Interception.Unity
{
    public sealed class HaystackInterceptor : UnityContainerExtension
    {
        public static void SetUp(IUnityContainer container)
        {
            container.AddExtension(new HaystackInterceptor());
        }
        
        protected override void Initialize()
        {
            Context.Strategies.Add(new HaystackInterceptionBuilderStrategy(), UnityBuildStage.PostInitialization);
        }
    }
}
