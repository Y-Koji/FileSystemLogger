using FileSystemLogger.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemLogger
{
    public class MainViewModel
    {
        public ObservableCollection<DirectoryWatcher> Watchers { get; } = new ObservableCollection<DirectoryWatcher>();
        public ObservableCollection<WatchLog> Logs { get; set; } = new ObservableCollection<WatchLog>();
    }
}
