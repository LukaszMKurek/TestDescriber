using TestDescriber.LogTree;

namespace TestDescriber.LogPrinters
{
    public interface ILogPrinter
    {
        void Print(string basePath, string testName, TestLog testLog);
    }
}