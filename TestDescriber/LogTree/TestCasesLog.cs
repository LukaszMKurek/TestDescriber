using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestDescriber.LogTree
{
    public sealed class TestCasesLog
    {
        public readonly long StartTimestamp;
        public long StopTimestamp;
        public readonly object[] Arguments;
        public readonly string DisplayName;
        public readonly List<LogEntry> LogEntries;
        private LogEntry _currentLogEntry;

        public TestCasesLog(object[] arguments, string displayName)
        {
            StartTimestamp = Stopwatch.GetTimestamp();
            Arguments = arguments;
            DisplayName = displayName;
            LogEntries = new List<LogEntry>();
        }

        [JetBrains.Annotations.MustUseReturnValue]
        public EndDescription AddLogEntry(string description)
        {
            if (_currentLogEntry == null)
            {
                _currentLogEntry = new LogEntry(description, false);
                LogEntries.Add(_currentLogEntry);
            }
            else
            {
                _currentLogEntry = _currentLogEntry.AddLogEntry(description, false);
            }

            return new EndDescription(this, _currentLogEntry);
        }

        public EndTechnicalDescription AddTechnicalLogEntry(string description)
        {
            LogEntry technicalLogEntry;

            if (_currentLogEntry == null)
            {
                LogEntries.Add(technicalLogEntry = new LogEntry(description, true));
            }
            else
            {
                technicalLogEntry = _currentLogEntry.AddLogEntry(description, true);
            }

            return new EndTechnicalDescription(technicalLogEntry);
        }

        public void End()
        {
            StopTimestamp = Stopwatch.GetTimestamp();

            if (_currentLogEntry != null)
                throw new InvalidOperationException(_currentLogEntry.Description + " - zapomniano dispose zawołać!");
        }

        public struct EndDescription : IDisposable
        {
            private readonly TestCasesLog _logEntry;
            private readonly LogEntry _currentLogEntry;

            public EndDescription(TestCasesLog logEntry, LogEntry currentLogEntry)
            {
                _logEntry = logEntry;
                _currentLogEntry = currentLogEntry;
            }

            public EndDescription Print(string message)
            {
                _currentLogEntry.SubLogs.Add(new TextSubLogEntry(message));

                return this;
            }

            public EndDescription PrintTable(object obj)
            {
                _currentLogEntry.SubLogs.Add(new TableSubLogEntry(obj));

                return this;
            }

            public EndDescription PrintJson(object obj)
            {
                _currentLogEntry.SubLogs.Add(new JsonSubLogEntry(obj));

                return this;
            }

            public void Dispose()
            {
                if (_logEntry._currentLogEntry != _currentLogEntry)
                    throw new InvalidOperationException("8");

                _logEntry._currentLogEntry = _currentLogEntry.Parent;
                _currentLogEntry.EndTimestamp = Stopwatch.GetTimestamp();
            }
        }

        public struct EndTechnicalDescription
        {
            private readonly LogEntry _currentLogEntry;

            public EndTechnicalDescription(LogEntry currentLogEntry)
            {
                _currentLogEntry = currentLogEntry;
            }

            public EndTechnicalDescription Print(string message)
            {
                _currentLogEntry.SubLogs.Add(new TextSubLogEntry(message));

                return this;
            }

            public EndTechnicalDescription PrintTable(object obj)
            {
                _currentLogEntry.SubLogs.Add(new TableSubLogEntry(obj));

                return this;
            }

            public EndTechnicalDescription PrintJson(object obj)
            {
                _currentLogEntry.SubLogs.Add(new JsonSubLogEntry(obj));

                return this;
            }
        }
    }
}