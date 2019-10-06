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
    public class DragDropCommand : Freezable, ICommand
    {
        public ObservableCollection<DirectoryWatcher> Watchers
        {
            get { return (ObservableCollection<DirectoryWatcher>)GetValue(WatchersProperty); }
            set { SetValue(WatchersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Watchers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatchersProperty =
            DependencyProperty.Register("Watchers", typeof(ObservableCollection<DirectoryWatcher>), typeof(DragDropCommand), new PropertyMetadata(null));
        
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (null == Watchers)
            {
                return;
            }

            if (parameter is string[] files)
            {
                foreach (var file in files)
                {
                    // 購読済みなら追加しない
                    foreach (var watcher in Watchers)
                    {
                        if (file == watcher.Path)
                        {
                            goto SKIP;
                        }
                    }

                    if (Directory.Exists(file))
                    {
                        var watcher = DirectoryWatcher.Listen(file);

                        Watchers.Add(watcher);
                    }

                    SKIP:;
                }
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new DragDropCommand();
        }
    }
}
