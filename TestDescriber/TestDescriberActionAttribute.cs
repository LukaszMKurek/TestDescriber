using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TestDescriber
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class TestDescriberActionAttribute : Attribute, ITestAction
    {
        public void BeforeTest(ITest test)
        {
            if (test.IsSuite == false)
            {
                var parametersNames = test.Method.GetParameters().Select(p => p.ParameterInfo.Name).ToArray();
                Describe.BeginTestCase(test.ClassName + "." + test.MethodName, parametersNames, test.Arguments, test.Name);
            }
        }

        public void AfterTest(ITest test)
        {
            if (test.IsSuite == false)
                Describe.EndTestCase(test.ClassName + "." + test.MethodName);
            else
                Describe.OnEndOfAllTests();
        }

        public ActionTargets Targets
        {
            get { return ActionTargets.Test | ActionTargets.Suite; }
        }
    }
}