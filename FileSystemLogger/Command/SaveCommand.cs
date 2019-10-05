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
    public class SaveCommand : Freezable, ICommand
    {
        public ObservableCollection<DirectoryWatcher> Watchers
        {
            get { return (ObservableCollection<DirectoryWatcher>)GetValue(WatchersProperty); }
            set { SetValue(WatchersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Watchers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatchersProperty =
            DependencyProperty.Register("Watchers", typeof(ObservableCollection<DirectoryWatcher>), typeof(SaveCommand), new PropertyMetadata(null));
        
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var saveObj = new Dictionary<string, IList<WatchLog>>();
            foreach (var watcher in Watchers)
            {
                saveObj.Add(watcher.Path, watcher.Logs);
            }

            string json = JSON.Serialize(saveObj);

            string savePath = Path.Combine(Environment.CurrentDirectory, Constant.SAVE_FILE_PATH);

            File.WriteAllText(savePath, json, Encoding.UTF8);
        }

        protected override Freezable CreateInstanceCore()
        {
            return new SaveCommand();
        }
    }
}
