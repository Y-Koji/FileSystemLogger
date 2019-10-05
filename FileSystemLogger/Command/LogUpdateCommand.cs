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
    public class LogUpdateCommand : Freezable, ICommand
    {
        public DirectoryWatcher Watcher
        {
            get { return (DirectoryWatcher)GetValue(WatcherProperty); }
            set { SetValue(WatcherProperty, value); CanExecuteChanged?.Invoke(this, new EventArgs()); }
        }

        // Using a DependencyProperty as the backing store for Watcher.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatcherProperty =
            DependencyProperty.Register("Watcher", typeof(DirectoryWatcher), typeof(LogUpdateCommand), new PropertyMetadata(null));
        
        public ObservableCollection<WatchLog> Logs
        {
            get { return (ObservableCollection<WatchLog>)GetValue(LogsProperty); }
            set { SetValue(LogsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Logs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LogsProperty =
            DependencyProperty.Register("Logs", typeof(ObservableCollection<WatchLog>), typeof(LogUpdateCommand), new PropertyMetadata(null));
        
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (null == Watcher || null == Logs)
            {
                return false;
            }

            return true;
        }

        public void Execute(object parameter)
        {
            Logs = Watcher.Logs;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new LogUpdateCommand();
        }
    }
}
