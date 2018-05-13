using NUnit.Framework;
using TestDescriber;

[assembly: TestDescriberAction]

namespace TestDescriber.Tests
{
    [TestFixture]
    public sealed class Tests
    {
        // todo może atrubut xDescription?
        [Test]
        public void T1()
        {
            SomeWork();
        }

        [TestCase(1, "11")]
        [TestCase(2, "22")]
        [TestCase(3, "33")]
        public void T2(int x, string y)
        {
            SomeWork();
        }

        private static void SomeWork()
        {
            using (Describe.Step("T0 action"))
            {
                Describe.TechnicalDetails("details 0");
            }
            
            using (Describe.Step("T1 action"))
            {
                Describe.TechnicalDetails("details 1");
                Describe.TechnicalDetails("details 1 and2");

                using (Describe.Step("sub 1 T1 action"))
                {
                    using (Describe.Step("sub 11 T11 action"))
                    {
                        Describe.TechnicalDetails("details 22");
                        Describe.TechnicalDetails("details 222");
                        Describe.TechnicalDetails("details 2222");
                    }

                    Describe.TechnicalDetails("details 2");
                }

                Describe.TechnicalDetails("details 3")
                    .PrintJson(new { xxx = "abc", yyy = "xyz" });

                using (Describe.Step("sub 2 T1 action"))
                {
                    Describe.TechnicalDetails("details 4");
                }

                Describe.TechnicalDetails("details 5");
            }
        }

    }
}
