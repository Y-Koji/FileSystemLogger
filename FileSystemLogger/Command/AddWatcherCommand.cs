using FileSystemLogger.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FileSystemLogger.Command
{
    public class AddWatcherCommand : Freezable, ICommand
    {
        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); CanExecuteChanged?.Invoke(this, new EventArgs()); }
        }

        // Using a DependencyProperty as the backing store for Path.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string), typeof(AddWatcherCommand), new PropertyMetadata(string.Empty));
        
        public ObservableCollection<DirectoryWatcher> Watchers
        {
            get { return (ObservableCollection<DirectoryWatcher>)GetValue(WatchersProperty); }
            set { SetValue(WatchersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Watchers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatchersProperty =
            DependencyProperty.Register("Watchers", typeof(ObservableCollection<DirectoryWatcher>), typeof(AddWatcherCommand), new PropertyMetadata(null));
        
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(Path) && Directory.Exists(Path))
            {
                return true;
            }

            return false;
        }

        public void Execute(object parameter)
        {
            // 購読済みなら追加しない
            foreach (var watcher in Watchers)
            {
                if (Path == watcher.Path)
                {
                    return;
                }
            }

            Watchers.Add(DirectoryWatcher.Listen(Path));
            Path = string.Empty;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new AddWatcherCommand();
        }
    }
}
