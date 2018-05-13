using System.IO;
using System.Linq;
using JetBrains.Annotations;
using TestDescriber.LogTree;

namespace TestDescriber.LogPrinters
{
    public sealed class SimpleTextFilePrinter : ILogPrinter
    {
        public void Print(string basePath, string testName, TestLog testLog)
        {
            int indent = 0;

            using (var file = File.CreateText(basePath + "\\" + testName + ".log.txt"))
            {
                Print(file, indent, "Test: {0}", testName);
                var indentInc = indent + 1;

                foreach (var testCase in testLog.TestCases)
                {
                    var args = testLog.ParametersNames.Select((parameterName, index) => parameterName + ": " + testCase.Arguments[index]);

                    Print(file, indentInc, "Args: ({0}) + {1}", string.Join("; ", args), testCase.DisplayName); // todo ten display name to może jakoś warunkowo bo często nie ma sensu. Atrybutem może to sterować?

                    foreach (var testCaseLogEntry in testCase.LogEntries)
                        PrintLogEntry(file, indentInc + 1, testCaseLogEntry);

                    Print(file, indentInc, "");
                }
            }
        }

        private void PrintLogEntry(StreamWriter file, int indent, LogEntry testCaseLogEntry)
        {
            if (testCaseLogEntry.IsTechnical)
                return;

            Print(file, indent, testCaseLogEntry.Description);

            foreach (var logEntry in testCaseLogEntry.Logs)
                PrintLogEntry(file, indent + 1, logEntry);
        }

        [StringFormatMethod("message")]
        private void Print(StreamWriter file, int indent, string message, params object[] args)
        {
            file.Write(new string(' ', indent * 4));
            file.WriteLine(message, args);
        }
    }
}