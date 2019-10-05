using FileSystemLogger.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace FileSystemLogger.Command
{
    public class LoadCommand : Freezable, ICommand
    {
        public ObservableCollection<DirectoryWatcher> Watchers
        {
            get { return (ObservableCollection<DirectoryWatcher>)GetValue(WatchersProperty); }
            set { SetValue(WatchersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Watchers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatchersProperty =
            DependencyProperty.Register("Watchers", typeof(ObservableCollection<DirectoryWatcher>), typeof(LoadCommand), new PropertyMetadata(null));
        
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (File.Exists(Constant.SAVE_FILE_PATH))
            {
                try
                {
                    string json = File.ReadAllText(Constant.SAVE_FILE_PATH, Encoding.UTF8);

                    var loadObj = JSON.Deserialize<IDictionary<string, IList<WatchLog>>>(json);

                    foreach (var item in loadObj)
                    {
                        var watcher =  DirectoryWatcher.Listen(item.Key);
                        foreach (var log in item.Value)
                        {
                            watcher.Logs.Add(log);
                        }

                        Watchers.Add(watcher);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + "\r\n" + e.StackTrace);
                    File.AppendAllText("err.log", "[" + DateTime.Now + "] " + e.Message + "\r\n" + e.StackTrace);
                }
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new LoadCommand();
        }
    }
}
