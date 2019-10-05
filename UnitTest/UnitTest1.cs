using FileSystemLogger.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void TestMethod1()
        {
            var watcher = DirectoryWatcher.Listen(@"C:\Users\PC user\Desktop\1");

            System.Threading.Tasks.Task.Delay(20000).Wait();

            watcher.Dispose();
        }

        [Fact]
        public void TestMethod2()
        {
            IList<WatchLog> logs = new List<WatchLog>();
            WatchLog log = new WatchLog();
            log.Name = "a";
            log.Date = DateTime.Now;

            logs.Add(log);
            string json = WatchLog.Save(logs);

            var obj = WatchLog.Load(json);
        }
    }
}
