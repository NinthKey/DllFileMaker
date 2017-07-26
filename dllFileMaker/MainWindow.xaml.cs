using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using static dllFileMaker.DllClassmaker;

namespace dllFileMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ArrayList VariableList;
        
        public MainWindow()
        {
            InitializeComponent();
            VariableList = new ArrayList();

        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            bool HasValidVariableName = CheckNameRepeatedInList();
            bool HasValidInput = CheckInputValid();

            if (!string.IsNullOrWhiteSpace(VariableNameTextBox.Text) &&
                HasValidVariableName &&
                HasValidInput)
            {
                Debug.WriteLine("Enter Create Variable: ");

                string selectedType = TypeComboBox.SelectedItem.
                                      ToString().Split(' ')[1];
                string variableName = VariableNameTextBox.Text;

                string defaultValue = DefaultValueTextBox.Text;

                VariableList.Add(new VariableItem(variableName,selectedType,defaultValue));

                CurrentVariable.Items.Add( selectedType + " " +
                    variableName + " = " + defaultValue);      
            }
            else
            {
                string messg = "";

                if(!HasValidInput)
                {
                    messg += "The input is not valid. ";
                }
                if (!HasValidVariableName)
                {
                    messg += "The name had been used already. ";
                }

                messg += "Please check again!";
                MessageBox.Show(messg, "Warning");
            }

            CleanUpTextBox();
        }

        private void CreateClassButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ClassNameTextBox.Text))
            {
                Debug.WriteLine("Receive Command to Make Dll Class...");
                DllClassmaker.CreateDllFile(ClassNameTextBox.Text,
                                            VariableList);

                CleanUpTextBox();
                ClassNameTextBox.Text = "";
                CurrentVariable.Items.Clear();
            }
            else
            {
                MessageBox.Show("Please Enter the Class Name", "Warning");
            }
        }


        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            VariableList = new ArrayList();
            CurrentVariable.Items.Clear();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVariable.SelectedItem != null)
            {
                CurrentVariable.Items.RemoveAt(CurrentVariable.SelectedIndex);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVariable.SelectedItem != null)
            {
                string item = CurrentVariable.SelectedItem.ToString();

                string[] itemsArray = item.Split(' ');
                string type = itemsArray[0];
                string name = itemsArray[1];
                string value = itemsArray[3];

                VariableNameTextBox.Text = name;
                TypeComboBox.Text = type;
                Debug.WriteLine(TypeComboBox.Items.IndexOf(type));
                DefaultValueTextBox.Text = value;

                CurrentVariable.Items.Remove(CurrentVariable.SelectedItem);

                VariableItem itemToBeDelete = null;
                foreach (VariableItem x in VariableList)
                {
                    if (x.name.Equals(name))
                    {
                        itemToBeDelete = x;
                    }
                }
                VariableList.Remove(itemToBeDelete);
            }
        }

        private void CleanUpTextBox()
        {
            VariableNameTextBox.Text = "";
            DefaultValueTextBox.Text = "";
            TypeComboBox.SelectedIndex = TypeComboBox.Items.IndexOf(" ");
            //CurrentVariableTextBlock.Text = "";            
        }

        public bool CheckNameRepeatedInList()
        {
            bool IsNameRepeated = false;

            foreach (VariableItem x in VariableList)
            {
                if (x.name.Equals(VariableNameTextBox.Text))
                {
                    IsNameRepeated = true;
                }
            }

            return !IsNameRepeated;
        }

        public bool CheckInputValid()
        {
            bool IsInputInvalid = false;

            if (TypeComboBox.SelectedItem != null)
            {
                string typeName = TypeComboBox.SelectedItem.ToString().Split(' ')[1];
                string defaultValue = DefaultValueTextBox.Text.ToLower();
                switch (typeName)
                {
                    case "string":
                        break;
                    case "int":
                        int tempInt;
                        IsInputInvalid = !int.TryParse(defaultValue, out tempInt);
                        break;
                    case "double":
                        double tempDouble;
                        IsInputInvalid = !double.TryParse(defaultValue, out tempDouble);
                        break;
                    case "bool":
                        if (!(defaultValue.Equals("true") || defaultValue.Equals("false")))
                        {
                            IsInputInvalid = true;
                        }
                        break;
                }
            }

            return !IsInputInvalid;
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            Stream stream = null;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = true;

            if(dialog.ShowDialog() == true)
            {
                if((stream = dialog.OpenFile()) != null)
                {
                    using (stream)
                    {
                        string path = dialog.FileName;
                        DllFileLoader.loadDLLFile(path);
                    }
                }
            }
            
        }
    }
}
