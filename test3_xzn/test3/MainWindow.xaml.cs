using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Data;
using System.Windows.Shapes;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace test3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".ifc";
            dlg.Filter = " (.ifc)|*.ifc";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open document 
                string filename  = dlg.FileName;
                fileTextBox.Text = filename;
                var model = IfcStore.Open(filename);
                var allDoors = model.Instances.OfType<Xbim.Ifc4.Interfaces.IIfcSite>();
                //IIfcDoor thedoor = allDoors.First<IIfcDoor>();
                var pEnumerable= allDoors.GetEnumerator();
                var thedoor = pEnumerable.Current;
                DataTable dataTable = new DataTable();
                while ( thedoor !=allDoors .Last ())
                {
                    pEnumerable.MoveNext();
                    thedoor = pEnumerable.Current;
                    var properties = thedoor.IsDefinedBy
           .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
           .SelectMany(r => ((IIfcPropertySet)r.RelatingPropertyDefinition).HasProperties)
           .OfType<IIfcPropertySingleValue>();
                    if (thedoor ==allDoors .First ())
                    {
                        foreach (var property in properties)
                        {
                            dataTable.Columns.Add(property.Name);
                        }
                    }
                    string[] provalue = new string[properties.Count()];
                    int i = 0;
                    foreach (var property in properties)
                    {
                        provalue[i] = property.NominalValue .ToString ();
                        i++;
                    }
                    dataTable.Rows.Add(provalue);  
                }
                DataView.ItemsSource =dataTable.AsDataView ();
            }
        }

    }
}
