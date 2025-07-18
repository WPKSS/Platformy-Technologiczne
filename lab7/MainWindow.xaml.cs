using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Diagnostics;

namespace PTLab7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }
        private void MenuItem_Click_LINQ(object sender, RoutedEventArgs e)
        {

        }
        private void MenuItem_Click_XML(object sender, RoutedEventArgs e)
        {

        }

        //public ObservableCollection<RecursiveModel> Models { get; set; } = new();
        public SearchableSortableCollection<RecursiveModel> Models { get; set; } = new();
        private List<RecursiveModel> originalModels = new();
        public RecursiveModel SelectedModel { get; set; }
        private List<dynamic> queryResult = new List<dynamic>();
        public MainWindow()
        {
            InitializeComponent();
            //DataContext = this;
        }

        private void ModelTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedModel = e.NewValue as RecursiveModel;
            DataContext = null; // Refresh binding
            DataContext = this;
        }
        public void GenerateData(object sender, RoutedEventArgs e)
        {
            Models.Clear();
            var list = RecursiveModel.GenerateRandomModels(50, 10);
            //var list = RecursiveModel.GenerateRandomModels(0, 21, 37);
            foreach (var model in list)
            {
                Models.Add(model);
            }
            ModelTreeView.ItemsSource = Models;
        }
        public void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        public void ShowVersion(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("PT Lab7 v1.0\nAutorzy: Emilian Zawrotny, Konrad Cichosz, Mateusz Gwiaździński", "Version");
        }

        private int countModels()
        {
            int count = 0;
            foreach (var model in Models)
            {
                count++;
                count += model.countChilds();
            }
            return count;
        }



        public void AddChild(Object sender, RoutedEventArgs e)
        {
            var dialog = new NewNodeContent();
            if (dialog.ShowDialog() == true)
            {
                var name = dialog.NodeName;
                var val = dialog.NodeValue;
                var id = countModels() + 1;
                var newNode = new RecursiveModel(id, name, val);

                SelectedModel.AddInstance(newNode);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedModel != null)
                RemoveNode(SelectedModel);
        }

        private void RemoveNode(RecursiveModel target)
        {
            Models.Remove(target);
            foreach (var child in target.Instances)
            {
                RemoveNode(child);
            }
            foreach (var model in Models)
            {
                model.Instances.Remove(target);
            }
        }
        private void SerializeToXml(string filePath)
        {
            var toSerialize = new SerializableModels
            {
                Items = Models.Select(m => new SerializableModel
                {
                    Name = m.Name,
                    Value = m.Value,
                    AdditionalData = m.AdditionalData
                }).ToList()
            };

            var serializer = new XmlSerializer(typeof(SerializableModels));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, toSerialize);
            }
        }

        private void DeserializeFromXml(string filePath)
        {
            var serializer = new XmlSerializer(typeof(SerializableModels));
            using (var reader = new StreamReader(filePath))
            {
                var deserialized = (SerializableModels)serializer.Deserialize(reader);

                Models.Clear();
                foreach (var item in deserialized.Items)
                {
                    // Generujemy nowe ID na podstawie czasu Unix
                    var unixId = (int)DateTimeOffset.Now.ToUnixTimeSeconds();
                    Models.Add(new RecursiveModel(
                        unixId,
                        item.Name,
                        item.Value)
                    {
                        AdditionalData = item.AdditionalData
                    });
                }
            }
        }

        private void ProjectData(object sender, RoutedEventArgs e)
        {
            var oddID = Models
                .Where(model => model.Id % 2 == 1)
                .Select(model => new
                {
                    SUM_OF = model.Value + model.AdditionalData.Number,
                    UPPERCASE = model.Name.ToUpper()
                });
            MessageBox.Show($"Utworzono {oddID.Count()} elementów", "Pierwsze zapytanie");
        }
        private void GroupData(object sender, RoutedEventArgs e)
        {
            var oddID = Models
                .Where(model => model.Id % 2 == 1)
                .Select(model => new
                {
                    SUM_OF = model.Value + model.AdditionalData.Number,
                    UPPERCASE = model.Name.ToUpper()
                });


            var average = oddID
                .GroupBy(item => item.UPPERCASE)
                .Select(g => new
                {
                    Group = g.Key,
                    Average = g.Average(x => x.SUM_OF)
                });
            String text = "";
            foreach (var group in average)
            {
                Console.WriteLine($"Grupa: {group.Group} i srednia: {group.Average}");
                text += $"Grupa: {group.Group} i srednia: {group.Average} \n";
            }


            MessageBox.Show(text); // nie w konsoli ponieważ nie wiem czemu mi nie wyswietla w konsoli, mozna zmienic pozniej na konsole
        }
        void FindUniqueNumericalValues(string xmlPath)
        {
            var doc = new XPathDocument(xmlPath);
            var nav = doc.CreateNavigator();

            // XPath dla unikatowych wartości Number
            var expr = nav.Compile("//Model[not(AdditionalData/Number = preceding::Model/AdditionalData/Number)]");
            var iterator = nav.Select(expr);

            Console.WriteLine("Obiekty z unikatowymi wartościami Number:");
            String text = "";
            while (iterator.MoveNext())
            {
                Console.WriteLine($"- {iterator.Current.OuterXml}");
                text += $"- {iterator.Current.OuterXml} \n";
            }
            MessageBox.Show(text);
        }
        private void SerializeXML(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "XML files (*.xml)|*.xml",
                DefaultExt = ".xml"
            };

            if (dialog.ShowDialog() == true)
            {
                SerializeToXml(dialog.FileName);
                MessageBox.Show("Zapisano do XML!");
            }
        }

        private void DeserializeXML(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "XML files (*.xml)|*.xml",
                DefaultExt = ".xml"
            };

            if (dialog.ShowDialog() == true)
            {
                DeserializeFromXml(dialog.FileName);
                FindUniqueNumericalValues(dialog.FileName);
                MessageBox.Show("Wczytano z XML!");
            }
        }
        private void Generate_XHtml(object sender, RoutedEventArgs e)
        {
            var xhtml = new XDocument(
                new XDocumentType("html", null, null, null),
                new XElement(XNamespace.Get("http://www.w3.org/1999/xhtml") + "html",
                    new XElement("head",
                        new XElement("title", "Raport danych"),
                        new XElement("meta", new XAttribute("charset", "utf-8")),
                        new XElement("style", @"
                    table { border-collapse: collapse; width: 100%; margin: 20px 0; }
                    th, td { border: 1px solid #ddd; padding: 8px; }
                    th { background-color: #f2f2f2; text-align: left; }
                    tr:nth-child(even) { background-color: #f9f9f9; }")),
                    new XElement("body",
                        new XElement("h1", "Lista modeli"),
                        new XElement("table",
                            new XElement("thead",
                                new XElement("tr",
                                    new XElement("th", "ID"),
                                    new XElement("th", "Nazwa"),
                                    new XElement("th", "Wartość"),
                                    new XElement("th", "Number"),
                                    new XElement("th", "Category"))),
                            new XElement("tbody",
                                Models.Select(model =>
                                    new XElement("tr",
                                        new XElement("td", model.Id),
                                        new XElement("td", model.Name),
                                        new XElement("td", model.Value),
                                        new XElement("td", model.AdditionalData.Number),
                                        new XElement("td", model.AdditionalData.Category)
                                    )
                                )
                            )
                        )
                    )
                )
            );
            var tempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"ModelReport_{DateTime.Now:yyyyMMddHHmmss}.xhtml");

            try
            {
                xhtml.Save(tempFile);
                Process.Start(new ProcessStartInfo
                {
                    FileName = tempFile,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas generowania pliku: {ex.Message}",
                              "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectorLoad(object sender, RoutedEventArgs e)
        {

            PropertySelector.ItemsSource = new List<string>
            {
                "Id",
                "Name",
                "AdditionalData"
            };

            PropertySelector.SelectedIndex = 0;

        }


        private void SearchClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {

                if (originalModels.Count == 0)
                {

                    originalModels = Models.ToList();
                }

                string propertyName = PropertySelector.SelectedItem as string;

                var value = SearchBox.Text;

                if (typeof(RecursiveModel).GetProperty(propertyName).PropertyType == typeof(int))
                {
                    int intVal = int.Parse(value);
                    Models.Search(propertyName, intVal);
                }
                else
                {
                    Models.Search(propertyName, value);
                }

            }
        }

        private void ResetClick(object sender, RoutedEventArgs e)
        {

            if (originalModels.Count > 0)
            {
                Models.Reset(originalModels);
                originalModels.Clear();
            }

            SearchBox.Text = "";
        }

        private void SortClick(object sender, RoutedEventArgs e)
        {
            var propertyName = PropertySelector.SelectedItem as string;

            if (propertyName != null)
            {

                Models.SortBy(propertyName);
            }

        }

    }
}
