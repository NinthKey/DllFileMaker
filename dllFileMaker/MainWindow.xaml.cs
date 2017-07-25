using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace dllFileMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (TypeComboBox.SelectedItem != null && !string.IsNullOrWhiteSpace(VariableNameTextBox.Text))
            {
                Debug.WriteLine("Enter Create Variable");
                string selectedType = TypeComboBox.SelectedItem.ToString().Split(' ')[1];

                string variableName = VariableNameTextBox.Text;

                
            }else
            {

            }
        }

        private void CreateClassButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
