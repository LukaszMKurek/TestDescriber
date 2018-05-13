using System.Collections.Generic;
using System.Diagnostics;

namespace TestDescriber.LogTree
{
    public sealed class LogEntry // może logScope? ale dla tehnical było by dziwne bo to nie byłby scope
    {
        public readonly long StartTimestamp;
        public long EndTimestamp; // private set by się przydało
        internal readonly LogEntry Parent; // todo można null przypisać przy końcu?
        public readonly string Description;
        public readonly List<SubLogEntry> SubLogs;
        public readonly bool IsTechnical;
        public readonly List<LogEntry> Logs;

        private LogEntry(LogEntry parent, string description, bool isTechnical)
        {
            Parent = parent;
            Description = description;
            IsTechnical = isTechnical;
            StartTimestamp = Stopwatch.GetTimestamp();
            SubLogs = new List<SubLogEntry>();
            Logs = new List<LogEntry>();
        }

        public LogEntry(string description, bool isTechnical) : this(null, description, isTechnical)
        {}

        public LogEntry AddLogEntry(string description, bool isTechnical)
        {
            var logEntry = new LogEntry(this, description, isTechnical);
            Logs.Add(logEntry);

            return logEntry;
        }
    }

    public abstract class SubLogEntry { }

    public sealed class TextSubLogEntry : SubLogEntry
    {
        public readonly string MessagePart;

        public TextSubLogEntry(string messagePart)
        {
            MessagePart = messagePart;
        }
    }

    public sealed class TableSubLogEntry : SubLogEntry
    {
        public readonly object Object;

        public TableSubLogEntry(object obj)
        {
            Object = obj;
        }
    }

    public sealed class JsonSubLogEntry : SubLogEntry
    {
        public readonly object Object;

        public JsonSubLogEntry(object obj)
        {
            Object = obj;
        }
    }
}