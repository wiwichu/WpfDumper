using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfDumper.View
{
    /// <summary>
    /// Interaction logic for WeakEventTableLeakView.xaml
    /// </summary>
    public partial class WeakEventTableLeakView : Window
    {
        public WeakEventTableLeakView()
        {
            InitializeComponent();

            this.DataContext = new DataObject() { Data = "test " };
            Unloaded += WeakEventTableLeakView_Unloaded;
        }

        private void WeakEventTableLeakView_Unloaded(object sender, RoutedEventArgs e)
        {
           // RemoveSourceFromValueChangedEventManager(this.DataContext);
            Unloaded += WeakEventTableLeakView_Unloaded;
        }

        public static void RemoveSourceFromValueChangedEventManager(object source)
        {
            // Remove the source from the ValueChangedEventManager.
            Assembly assembly = Assembly.GetAssembly(typeof(FrameworkElement));
            Type type = assembly.GetType("MS.Internal.Data.ValueChangedEventManager");
            PropertyInfo propertyInfo = type.GetProperty("CurrentManager",
                BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo currentManagerGetter = propertyInfo.GetGetMethod(true);
            object manager = currentManagerGetter.Invoke(null, null);
            MethodInfo remove = type.GetMethod("Remove", BindingFlags.NonPublic |
                BindingFlags.Instance);
            remove.Invoke(manager, new object[] { source });
            // The code above removes the instances of ValueChangedRecord from the
            // WeakEventTable, but they are still rooted by the property descriptors of
            // the source object. We need to clean them out of the property descriptors
            // as well, to allow them to be garbage collected. (Which is necessary
            // because they contain a hard reference to the source, which is what we
            // really want garbage collected.)
            FieldInfo valueChangedHandlersInfo = typeof(PropertyDescriptor).GetField
                ("valueChangedHandlers", BindingFlags.Instance | BindingFlags.NonPublic);
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(source);
            foreach (PropertyDescriptor pd in pdc)
            {
                Hashtable changeHandlers =
                    (Hashtable)valueChangedHandlersInfo.GetValue(pd);
                if (changeHandlers != null)
                {
                    changeHandlers.Remove(source);
                }
            }
        }
    }

    public class DataObject
    {
        private string data;

        public string Data
        {
            get { return this.data; }
            set { this.data = value; }
        }
    }

}
