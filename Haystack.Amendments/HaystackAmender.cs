using Afterthought;
using Haystack.Diagnostics.Amendments;
using Haystack.Diagnostics.Configuration;
using System;

namespace Haystack.Amendments
{
    public class HaystackAmender<T> : Amendment<T, T>
    {
        public HaystackAmender()
        {
            InitializeConfiguration();
            AmendProperties();
            AmendConstructors();
            AmendMethods();
        }

        private void InitializeConfiguration()
        {
            string configurationText = (string)AppDomain.CurrentDomain.GetData(AmendmentSetupProvider.ConfigurationKey);
            AmendmentRepository.Initialize(configurationText);
        }

        private void AmendProperties()
        {
            Properties
                .Where(AmendmentRepository.BeforePropertyGetAmenders)
                .BeforeGet(PropertyAmendments<T>.BeforePropertyGet);
            Properties
                .Where(AmendmentRepository.AfterPropertyGetAmenders)
                .AfterGet(PropertyAmendments<T>.AfterPropertyGet);
            Properties
                .Where(AmendmentRepository.BeforePropertySetAmenders)
                .BeforeSet(PropertyAmendments<T>.BeforePropertySet);
            Properties
                .Where(AmendmentRepository.AfterPropertySetAmenders)
                .AfterSet(PropertyAmendments<T>.AfterPropertySet);
        }

        private void AmendConstructors()
        {
            Constructors
                .Where(AmendmentRepository.BeforeConstructorAmenders)
                .Before(ConstructorAmendments<T>.BeforeConstructor);
            Constructors
                .Where(AmendmentRepository.AfterConstructorAmenders)
                .After(ConstructorAmendments<T>.AfterConstructor);
            Constructors
                .Where(AmendmentRepository.CatchConstructorAmenders)
                .Catch(ConstructorAmendments<T>.CatchConstructor);
        }

        private void AmendMethods()
        {
            Methods
                .Where(AmendmentRepository.BeforeMethodAmenders)
                .Before(MethodAmendments<T>.BeforeMethod);
            Methods
                .Where(AmendmentRepository.AfterVoidMethodAmenders)
                .Where(method => method.ReturnType == typeof(void))
                .After(MethodAmendments<T>.AfterVoidMethod);
            Methods
                .Where(AmendmentRepository.AfterMethodAmenders)
                .Where(method => method.ReturnType != typeof(void))
                .After(MethodAmendments<T>.AfterMethod);
            /*Methods
                .Where(AmendmentRepository.CatchVoidMethodAmenders)
                .Catch(MethodAmendments<T>.CatchVoidMethod);
            Methods
                .Where(AmendmentRepository.CatchMethodAmenders)
                .Catch(MethodAmendments<T>.CatchMethod);*/
            Methods
                .Where(AmendmentRepository.FinallyMethodAmenders)
                .Finally(MethodAmendments<T>.Finally);
        }
    }
}
