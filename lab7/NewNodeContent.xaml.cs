using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace PTLab7
{
    /// <summary>
    /// Interaction logic for NewNodeContent.xaml
    /// </summary>
    public partial class NewNodeContent : Window
    {
        public string NodeName { get; set; }
        public float NodeValue { get; set; }
        
        public NewNodeContent()
        {
            InitializeComponent();
            var rand = new Random();
            NodeValue = (float)(rand.NextDouble() * 100.0);
            NodeName = $"Child{rand.Next()}";
            ValueBox.Text = NodeValue.ToString();
            NameBox.Text = NodeName;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void AddChild_Click(object sender, RoutedEventArgs e)
        {
            NodeName = NameBox.Text;
            if (float.TryParse(ValueBox.Text, System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture, out float val))
            {
                NodeValue = val;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Podałeś niepoprawną wartość", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
