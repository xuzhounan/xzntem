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
using Xbim.IO;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.PropertyResource;
using Xbim.Ifc4.SharedBldgElements;
using Xbim.Ifc4.ProductExtension;
using Xbim.Common.Exceptions;
using Xbim.Common;

namespace test4_xzn
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
        string filename;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".ifc";
            dlg.Filter = " (.ifc)|*.ifc";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open document 
                filename = dlg.FileName;
                fileTextBox.Text = filename;
                var model = IfcStore.Open(filename);
                var allifcsite = model.Instances.OfType<IIfcSite>();
                var pEnumerable = allifcsite.GetEnumerator();
                var thesite = pEnumerable.Current;
                DataTable dataTable = new DataTable();
                 while (thesite != allifcsite.Last())
                {
                    pEnumerable.MoveNext();
                    thesite = pEnumerable.Current;
                    var properties = thesite.IsDefinedBy
           .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
           .SelectMany(r => ((IIfcPropertySet)r.RelatingPropertyDefinition).HasProperties)
           .OfType<IIfcPropertySingleValue>();
                    if (thesite == allifcsite.First())
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
                        provalue[i] = property.NominalValue.ToString();
                        i++;
                    }
                    dataTable.Rows.Add(provalue);
                }
                DataView.ItemsSource = dataTable.AsDataView();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            using (var model = IfcStore.Open(filename, null, null, null, XbimDBAccess.ReadWrite, -1))
            { 
                var tem = model.IsTransactional;
                var allifcsite = model.Instances.OfType<IIfcSite>();
                //var pEnumerable = allifcsite.GetEnumerator();
                //pEnumerable.MoveNext();
                //var thesite = pEnumerable.Current;
                var thesite = allifcsite.First();
                var properties = thesite.IsDefinedBy
           .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
           .SelectMany(r => ((IIfcPropertySet)r.RelatingPropertyDefinition).HasProperties)
           .OfType<IIfcPropertySingleValue>();
                using ( var txn = model.BeginTransaction())
                    { 
                   
                    int u = 0;
                    foreach (var property in properties)
                    {
                        property.NominalValue = new IfcLabel((DataView.Columns[u].GetCellContent(DataView.Items[0]) as TextBlock).Text);
                        u++;
                    }
                    thesite .Name = thesite.Name + "checked";
                    txn.Commit();
                    try { model.SaveAs(filename ); }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    MessageBox.Show("ok");
                    }
              
            }
                }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IfcStore.ModelProviderFactory.UseHeuristicModelProvider();
        }
    }
}
