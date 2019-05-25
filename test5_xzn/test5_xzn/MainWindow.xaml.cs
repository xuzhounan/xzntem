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
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace test5_xzn
{
    public class Databinding
    {
        public DataTable Datatable { get; set; }
    }
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
        DataTable Datatable;
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
                Datatable = new DataTable();
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
                            Datatable.Columns.Add(property.Name);
                        }
                    }
                    string[] provalue = new string[properties.Count()];
                    int i = 0;
                    foreach (var property in properties)
                    {
                        provalue[i] = property.NominalValue.ToString();
                        i++;
                    }
                    Datatable.Rows.Add(provalue);
                }
                DataView.ItemsSource = Datatable.AsDataView();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            using (var model = IfcStore.Open(filename, null, null, null, XbimDBAccess.ReadWrite, -1))
            {
                var tem = model.IsTransactional;
                var allifcsite = model.Instances.OfType<IfcSite>();
                var thesite = allifcsite.First();
                var properties = thesite.IsDefinedBy
           .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
           .SelectMany(r => ((IIfcPropertySet)r.RelatingPropertyDefinition).HasProperties)
           .OfType<IIfcPropertySingleValue>();
                using (var txn = model.BeginTransaction())
                {

                    int u = 0;
                    foreach (var property in properties)
                    {
                        property.NominalValue = new IfcLabel((DataView.Columns[u].GetCellContent(DataView.Items[0]) as TextBlock).Text);
                        u++;
                    }
                    int t = DataView.Columns.Count();
                    for (int ttem = u; ttem < t; ttem++)
                    {
                        var pSetRel = model.Instances.New<IfcRelDefinesByProperties>(r =>
                        {
                            r.GlobalId = Guid.NewGuid();
                            r.RelatingPropertyDefinition = model.Instances.New<IfcPropertySet>(pSet =>
                            {
                                pSet.Name = "add" + ttem.ToString();
                                pSet.HasProperties.Add(model.Instances.New<IfcPropertySingleValue>(p =>
                                {
                                    p.Name = DataView.Columns[ttem].Header.ToString();
                                    p.NominalValue = new IfcLabel((DataView.Columns[ttem].GetCellContent(DataView.Items[0]) as TextBlock).Text); // Default Currency set on IfcProject
                                }));
                            });
                        });
                        pSetRel.RelatedObjects.Add(thesite);
                    }
                    thesite.Name = thesite.Name + "checked";
                    txn.Commit();
                    try { model.SaveAs(filename); }
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

        private void 添加属性_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Datatable.Columns.Add(属性.Text);
                属性.Text = "";
                DataView.ItemsSource = Datatable.AsDataView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public class BloggingContext : DbContext
        {
            public DbSet<Blog> Blogs { get; set; }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\v11.0;Database=tem;Trusted_Connection=True;");
            }
            
        }
        public class Blog
        {
            [Key]
            public string Url { get; set; }
            public string Sitekey { get; set; }
            public string Sitevalue { get; set; }
           
        }
        private void 导入数据库_Click(object sender, RoutedEventArgs e)
        {
            string sitevalue="";
            string sitekey="";
            for (int i = 0; i < DataView.Columns.Count(); i++)
            {
                sitekey = sitekey+DataView.Columns[i].Header.ToString()+",";
                sitevalue = sitevalue + (DataView.Columns[i].GetCellContent(DataView.Items[0]) as TextBlock).Text.ToString() + ",";
            }
            using (var db = new BloggingContext())
            {
                var blog = new Blog {
                    Url = filename,
                    Sitekey = sitekey ,
                    Sitevalue = sitevalue,
                };
                db.Blogs.Add(blog);
                db.SaveChanges();
                MessageBox.Show("ok");
            }
        }
    }
}


