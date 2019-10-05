using FileSystemLogger.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FileSystemLogger.Command
{
    public class DisposeCommand : Freezable, ICommand
    {
        public ObservableCollection<DirectoryWatcher> Watchers
        {
            get { return (ObservableCollection<DirectoryWatcher>)GetValue(WatchersProperty); }
            set { SetValue(WatchersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Watchers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatchersProperty =
            DependencyProperty.Register("Watchers", typeof(ObservableCollection<DirectoryWatcher>), typeof(DisposeCommand), new PropertyMetadata(null));

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

            foreach (var watcher in Watchers)
            {
                watcher.Dispose();
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new DisposeCommand();
        }
    }
}
