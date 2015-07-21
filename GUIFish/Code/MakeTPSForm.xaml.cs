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
using System.Windows.Shapes;
using System.IO;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;

namespace GUIFish {
    /// <summary>
    /// Interaction logic for MakeTPSForm.xaml
    /// </summary>
    public partial class MakeTPSForm:Window {

        System.Windows.Forms.DialogResult _result;
        System.Windows.Forms.FolderBrowserDialog _dialog;

        public System.Windows.Forms.DialogResult Result {

            get { return _result; }
        }

        public System.Windows.Forms.FolderBrowserDialog Dialog {

            get { return _dialog; }
        }

        public MakeTPSForm() {
            InitializeComponent();

            _dialog = new System.Windows.Forms.FolderBrowserDialog();
            _result = new System.Windows.Forms.DialogResult();
            OutputTextBox.TextWrapping = TextWrapping.Wrap;
            OutputTextBox.AcceptsReturn = true;
            OutputTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            OutputTextBox.IsReadOnly = true;

        }

        private void ProcChange(object sender, PropertyChangedEventArgs e) {

            string[] split = e.PropertyName.Split('#');

            if("TEXT" == split[0]) {

                OutputTextBox.Text += split[1];
            }
        }

        private void SelectFolderButton_Click(object sender, RoutedEventArgs e) {

            _result = _dialog.ShowDialog();

            if(System.Windows.Forms.DialogResult.OK == _result) {
                this.FolderNameLabel.Content = _dialog.SelectedPath.ToString();
            }
        }

        private void GoButton_Click(object sender, RoutedEventArgs e) {
            //disable buttons
            CloseButton.IsEnabled = false;
            GoButton.IsEnabled = false;
            HelpButton.IsEnabled = false;
            SelectFolderButton.IsEnabled = false;

            OutputTextBox.Text = "";

            if(System.Windows.Forms.DialogResult.OK == _result) {

                ThreadStart ts = delegate { Go(); };
                Thread t = new Thread(ts);
                t.Start();

                OutputTextBox.AppendText("working" + "\n");

            } else {

                OutputTextBox.Text = "You must select a folder first\n";
                Done();
            }

        }

        private void Done() {

            //enable buttons
            CloseButton.IsEnabled = true;
            GoButton.IsEnabled = true;
            HelpButton.IsEnabled = true;
            SelectFolderButton.IsEnabled = true;
            OutputTextBox.AppendText("Done!");
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e) {

            HelpWindow helpwindow = new HelpWindow();

            helpwindow.addText("Generates TPS files for JPEG files");
            helpwindow.addText("");
            helpwindow.addText("First make sure all target .JPG files are in the same folder.");
            helpwindow.addText("");
            helpwindow.addText("Steps to use:");
            helpwindow.addText("1. Click on the 'Select Folder' button and navigate to the folder with target .JPGs");
            helpwindow.addText("2. Click Go and the program will create all of the corresponding .TPS files");
            helpwindow.addText("");
            helpwindow.addText("The output box will tell you which files have been created");
            helpwindow.addText("");
            helpwindow.addText("When all .TPS files have been created, you will see 'done' in the output box");
            helpwindow.addText("All created .TPS files will be in the same folder as their corresponding .JPG files");

            helpwindow.Show();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        public void Go() {
            string[] files = Directory.GetFiles(Dialog.SelectedPath, "*.JPG");
            foreach(string file in files) {

                string[] parts = file.Split('\\');

                string name = parts[parts.Count() - 1];

                parts = name.Split('_');

                string tps_name = parts[1];

                parts = tps_name.Split('.');

                tps_name = parts[0];

                tps_name = tps_name + ".TPS";

                using(FileStream fs = new FileStream(Dialog.SelectedPath.ToString() + "\\" + tps_name, FileMode.OpenOrCreate))
                using(StreamWriter sw = new StreamWriter(fs)) {

                    //OutputTextBox.Text  += "creating: " + tps_name + "\n";
                    sw.WriteLine("LM=0");
                    sw.WriteLine("IMAGE=" + name);
                }

                string create_str = "creating: " + tps_name + "\n";
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => OutputTextBox.AppendText(create_str)));
            }

            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => Done()));
        }

    }

}
