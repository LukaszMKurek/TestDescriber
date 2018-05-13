using System;
using System.Collections.Generic;

namespace TestDescriber.LogTree
{
    public sealed class TestLog
    {
        public readonly string TestName;
        public readonly string[] ParametersNames;
        public readonly List<TestCasesLog> TestCases;
        private TestCasesLog _currentTestCasesLog;

        public TestLog(string testName, string[] parametersNames)
        {
            TestName = testName;
            ParametersNames = parametersNames;
            TestCases = new List<TestCasesLog>();
        }

        public string Name
        {
            get { return TestName; }
        }

        public TestCasesLog CurrentTestCase
        {
            get { return _currentTestCasesLog; }
        }

        public void AddTestCase(object[] arguments, string displayName)
        {
            if (_currentTestCasesLog != null)
            {
                _currentTestCasesLog = null; // todo ten problem można tak rozwiązać  by kolejne testy się nie wywalały - przemyśleć
                throw new InvalidOperationException("7");
            }

            _currentTestCasesLog = new TestCasesLog(arguments, displayName);
            TestCases.Add(_currentTestCasesLog);
        }

        public void EndTestCase()
        {
            if (_currentTestCasesLog == null)
                throw new InvalidOperationException("6");

            _currentTestCasesLog = null;
        }
    }
}