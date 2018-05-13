using System;
using System.Collections.Generic;
using System.Diagnostics;
using TestDescriber.LogPrinters;

namespace TestDescriber.LogTree
{
    public sealed class AllTestLog
    {
        public readonly long StartTimestamp;
        public long StopTimestamp;
        public readonly Dictionary<string, TestLog> Tests;
        private TestLog _currentTestLog;

        public AllTestLog()
        {
            StartTimestamp = Stopwatch.GetTimestamp();
            Tests = new Dictionary<string, TestLog>();
        }

        public TestLog CurrentTestLog
        {
            get { return _currentTestLog; }
        }

        public void AddTestCase(string testName, string[] parametersNames, object[] arguments, string displayName)
        {
            if (_currentTestLog != null)
            {
                _currentTestLog = null; // todo - przemyśleć
                throw new InvalidOperationException("1");
            }

            if (Tests.TryGetValue(testName, out _currentTestLog) == false)
            {
                _currentTestLog = new TestLog(testName, parametersNames);
                Tests.Add(testName, _currentTestLog);
            }

            _currentTestLog.AddTestCase(arguments, displayName);
        }

        public void TestCaseEnd(string testName)
        {
            if (_currentTestLog == null)
                throw new InvalidOperationException("2");

            var tmpCurrentTestLog = _currentTestLog;
            _currentTestLog = null;

            if (tmpCurrentTestLog.Name != testName)
                throw new InvalidOperationException("3");

            try
            {
                tmpCurrentTestLog.CurrentTestCase.End();
            }
            finally // todo przymyśleć
            {
                tmpCurrentTestLog.EndTestCase();
            }
        }

        public void OnEndOfAllTests()
        {
            StopTimestamp = Stopwatch.GetTimestamp();

            if (_currentTestLog != null)
                throw new InvalidOperationException("5");


            //wypisywanie logó powinno być per test by nie czekać zbyt długo
            // tutaj moża wypisać conajwyżej listę testów w html by w przeglądrce iść w przód i w tył

            //JsonConvert.SerializeObject(Tests); // todo w ten sposób można testować że graf obiektów nie zmienia się - kolejny logPrinter poninien być do tego zrobiony
            var simpleFilePrinter = new SimpleTextFilePrinter();
            var htmlFilePrinter = new Html2FilePrinter();
            foreach (var testLog in Tests)
            {
                simpleFilePrinter.Print(@"C:\Users\Justyna\Desktop", testLog.Key, testLog.Value);
                htmlFilePrinter.Print(@"C:\Users\Justyna\Desktop", testLog.Key, testLog.Value);
            }

            //HtmlFilePrinter.Print(Tests);
        }
    }
}