using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace FileSystemLogger.Behavior
{
    public class DragDropBehavior : Behavior<Control>
    {
        public IEnumerable<string> DropList
        {
            get { return (IEnumerable<string>)GetValue(DropListProperty); }
            set { SetValue(DropListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DropList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DropListProperty =
            DependencyProperty.Register("DropList", typeof(IEnumerable<string>), typeof(DragDropBehavior), new PropertyMetadata(Enumerable.Empty<string>()));
        
        /// <summary>ファイルを受け取る</summary>
        public bool IsFileDrop
        {
            get { return (bool)GetValue(IsFileDropProperty); }
            set { SetValue(IsFileDropProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFileDrop.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFileDropProperty =
            DependencyProperty.Register("IsFileDrop", typeof(bool), typeof(DragDropBehavior), new PropertyMetadata(true));
        
        /// <summary>ディレクトリを受け取る</summary>
        public bool IsDirectoryDrop
        {
            get { return (bool)GetValue(IsDirectoryDropProperty); }
            set { SetValue(IsDirectoryDropProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDirectoryDrop.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDirectoryDropProperty =
            DependencyProperty.Register("IsDirectoryDrop", typeof(bool), typeof(DragDropBehavior), new PropertyMetadata(true));
        
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DragDropBehavior), new PropertyMetadata(null));
        
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(DragDropBehavior), new PropertyMetadata(null));
        
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.AllowDrop = true;
            this.AssociatedObject.PreviewDragOver += OnPreviewDragOver;
            this.AssociatedObject.Drop += OnDrop;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.AllowDrop = false;
            this.AssociatedObject.PreviewDragOver -= OnPreviewDragOver;
            this.AssociatedObject.Drop -= OnDrop;
        }

        private void OnPreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }

            e.Handled = true;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            
            IList<string> filter = new List<string>();
            foreach (var file in files)
            {
                if (IsDirectoryDrop)
                {
                    if (Directory.Exists(file))
                    {
                        filter.Add(file);
                    }
                }

                if (IsFileDrop)
                {
                    if (File.Exists(file))
                    {
                        filter.Add(file);
                    }
                }
            }

            DropList = filter.ToArray();

            if (Command?.CanExecute(CommandParameter) ?? false)
            {
                Command.Execute(CommandParameter);
            }
        }
    }
}
