using JetBrains.Annotations;
using TestDescriber.LogTree;

namespace TestDescriber
{
    public static class Describe
    {
        private static readonly AllTestLog AllTestLog = new AllTestLog();

        internal static void BeginTestCase(string testName, string[] parametersNames, object[] arguments, string displayName)
        {
            AllTestLog.AddTestCase(testName, parametersNames, arguments, displayName);
        }

        internal static void EndTestCase(string testName)
        {
            AllTestLog.TestCaseEnd(testName);
        }
        
        internal static void OnEndOfAllTests()
        {
            AllTestLog.OnEndOfAllTests();
        }

        // ta metoda musi logować żeczy które są deterministyczne i nie zmienne
        [MustUseReturnValue]
        public static TestCasesLog.EndDescription Step(string description, params object[] args) // także wersja bez args by się przydała by oszczedzać na alokacjach array
        {
            return AllTestLog.CurrentTestLog.CurrentTestCase.AddLogEntry(string.Format(description, args));
        }

        public static TestCasesLog.EndTechnicalDescription TechnicalDetails(string description, params object[] args) // ta metoda powinna zwracać obiekt by można było logować dodatkowe informacje
        {
            return AllTestLog.CurrentTestLog.CurrentTestCase.AddTechnicalLogEntry(string.Format(description, args));
        }
    }
}