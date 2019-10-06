using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace FileSystemLogger.Behavior
{
    public class ListBoxDeleteActionBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.KeyDown += OnKeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.KeyDown -= OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            ListBox listBox = this.AssociatedObject;
            if (e.Key == Key.Delete)
            {
                if (null != listBox.SelectedItem)
                {
                    if (null == listBox.ItemsSource)
                    {
                        listBox.Items.Remove(listBox.SelectedItem);
                    }
                    else
                    {
                        IList list = listBox.ItemsSource as IList;
                        list.Remove(listBox.SelectedItem);
                    }
                }
            }
        }
    }
}
