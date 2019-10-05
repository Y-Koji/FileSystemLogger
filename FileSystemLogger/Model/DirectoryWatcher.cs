using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Threading;

namespace FileSystemLogger.Model
{
    public class DirectoryWatcher : IDisposable
    {
        private Dispatcher Dispatcher { get; } = Dispatcher.CurrentDispatcher;
        private FileSystemWatcher Watcher { get; } = null;
        public ObservableCollection<WatchLog> Logs { get; } = new ObservableCollection<WatchLog>();
        public string Path { get; private set; }
        public string Name { get; private set; }

        private DirectoryWatcher(string path)
        {
            Watcher = new FileSystemWatcher(path);
            Watcher.NotifyFilter =
                (NotifyFilters) Enum.Parse(
                    typeof(NotifyFilters),
                    string.Join(",",
                        typeof(NotifyFilters).GetEnumNames()));
            Watcher.Created += OnCreated;
            Watcher.Changed += OnChanged;
            Watcher.Renamed += OnRenamed;
            Watcher.Deleted += OnDeleted;
            Watcher.IncludeSubdirectories = true;
            Watcher.EnableRaisingEvents = true;

            Path = path;
            Name = System.IO.Path.GetFileName(path);
        }

        public static DirectoryWatcher Listen(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
            {
                throw new DirectoryNotFoundException();
            }

            return new DirectoryWatcher(path);
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            WatchLog log = WatchLog.Create(Path, e);

            Dispatcher.Invoke(() => Logs.Insert(0, log));
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            WatchLog log = WatchLog.Create(Path, e);

            Dispatcher.Invoke(() => Logs.Insert(0, log));
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            WatchLog log = WatchLog.Create(Path, e);

            Dispatcher.Invoke(() => Logs.Insert(0, log));
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            WatchLog log = WatchLog.Create(Path, e);

            Dispatcher.Invoke(() => Logs.Insert(0, log));
        }

        public void Dispose()
        {
            if (null != Watcher)
            {
                Watcher.EnableRaisingEvents = false;
                Watcher.Dispose();
            }
        }
    }
}
