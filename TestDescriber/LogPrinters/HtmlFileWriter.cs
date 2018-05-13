using System.IO;
using System.Linq;
using JetBrains.Annotations;
using TestDescriber.LogTree;

namespace TestDescriber.LogPrinters
{
    public sealed class Html2FilePrinter : ILogPrinter
    {
        private int _id;

        public void Print(string basePath, string testName, TestLog testLog)
        {
            int indent = 0;

            using (var file = File.CreateText(basePath + "\\" + testName + ".log.html"))
            {
                BeginHtml(file, indent, testName);

                Print(file, indent, "<h1>Test: {0}</h1>", testName);
                var indentInc = indent + 1;

                foreach (var testCase in testLog.TestCases)
                {
                    var args = testLog.ParametersNames.Select((parameterName, index) => parameterName + ": " + testCase.Arguments[index]);

                    var header = string.Format("Args: ({0}) + {1}", string.Join("; ", args), testCase.DisplayName); // todo ten display name to może jakoś warunkowo bo często nie ma sensu. Atrybutem może to sterować?
                    BeginPrintCollapse(file, indentInc, (_id++).ToString(), header, false);

                    foreach (var testCaseLogEntry in testCase.LogEntries)
                        PrintLogEntry(file, indentInc + 1, testCaseLogEntry);

                    EndPrintCollapse(file, indentInc);
                }

                EndHtml(file, indent);
            }
        }

        private void PrintLogEntry(StreamWriter file, int indent, LogEntry testCaseLogEntry)
        {
            var header = (testCaseLogEntry.IsTechnical ? "Details: " : "") + testCaseLogEntry.Description;
            BeginPrintCollapse(file, indent, (_id++).ToString(), header, testCaseLogEntry.Logs.Any(i => i.IsTechnical == false));

            foreach (var logEntry in testCaseLogEntry.Logs)
                PrintLogEntry(file, indent + 1, logEntry);

            EndPrintCollapse(file, indent);
        }

        private void BeginPrintCollapse(StreamWriter file, int indent, string id, string header, bool isExpanded)
        {
            file.Write(@"
    <div>
        <a class=""btn btn-primary"" data-toggle=""collapse"" href=""#collapse{0}"" role=""button"" aria-expanded=""false"" aria-controls=""collapse{0}"">+</a>
        {1}
    </div>
    <div class=""collapse{2}"" id=""collapse{0}"">
        <div class=""card card-body"">
", id, header, isExpanded ? " show" : "");
        }

        private void EndPrintCollapse(StreamWriter file, int indent)
        {
            file.Write(@"
        </div>
    </div>
");
        }

        public void BeginHtml(StreamWriter file, int indent, string testName)
        {
            Print(file, indent, @"
<!doctype html>
<html lang=""en"">
<head>
    <meta charset=""utf-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1, shrink-to-fit=no"">

    <link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css"" integrity=""sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm"" crossorigin=""anonymous"">

    <title>{0}</title>
</head>
<body>
",
                testName);
        }

        public void EndHtml(StreamWriter file, int indent)
        {
            Print(file, indent, @"
<!-- Optional JavaScript -->
    <!-- jQuery first, then Popper.js, then Bootstrap JS -->
    <script src=""https://code.jquery.com/jquery-3.2.1.slim.min.js"" integrity=""sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN"" crossorigin=""anonymous""></script>
    <script src=""https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js"" integrity=""sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q"" crossorigin=""anonymous""></script>
    <script src=""https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"" integrity=""sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl"" crossorigin=""anonymous""></script>
</body>
</html>
");
        }

        [StringFormatMethod("message")]
        private void Print(StreamWriter file, int indent, string message, params object[] args)
        {
            file.Write(new string(' ', indent * 4));
            file.WriteLine(message, args);
        }
    }
}