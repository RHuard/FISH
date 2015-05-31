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
using Xceed.Wpf.Toolkit;
using System.IO;

namespace GUIFish {
    /// <summary>
    /// Interaction logic for MakeCSVWindow.xaml
    /// </summary>
    public partial class MakeCSVWindow:Window {

        private static CSVWriter _writer;
        System.Windows.Forms.DialogResult _result;
        System.Windows.Forms.FolderBrowserDialog _dialog;
        private static List<FishData> _data;

        public MakeCSVWindow() {
            InitializeComponent();

            _data = new List<FishData>();
            _dialog = new System.Windows.Forms.FolderBrowserDialog();
            OutputTextBox.TextWrapping = TextWrapping.Wrap;
            OutputTextBox.AcceptsReturn = true;
            OutputTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            OutputTextBox.IsReadOnly = true;
        }

        private void SelectFolderButton_Click(object sender, RoutedEventArgs e) {

            _result = _dialog.ShowDialog();

            if(System.Windows.Forms.DialogResult.OK == _result) {
                this.FolderNameLabel.Content = _dialog.SelectedPath.ToString();
            }
        }

        private void GoButton_Click(object sender, RoutedEventArgs e) {
            OutputTextBox.Text = "";

            if(System.Windows.Forms.DialogResult.OK != _result) {

                OutputTextBox.Text = "You must select a folder first";
                return;
            }

            if(0 > Convert.ToInt32(PointsUpDown.Text)) {

                OutputTextBox.Text = "Number of Points cannot be less than 0";
                return;
            }

            if(0 > Convert.ToInt32(STDDevUpDown.Text)) {

                OutputTextBox.Text = "Number of Standard Devations cannot be less than 0";
                return;
            }

            if(String.Empty == OutFileTextBox.Text) {

                OutputTextBox.Text = "You must have an outfile name";
                return;
            }

            //if we are here, we are able to start adding the tps files to the csv file
            string[] files = Directory.GetFiles(_dialog.SelectedPath, "*.TPS");

            int n = Convert.ToInt32(STDDevUpDown.Text);
            int np = Convert.ToInt32(PointsUpDown.Text);
            
            OutputTextBox.Text += "Grabbing Data From Files Now" + "\n";
            OutputTextBox.Text += "-----------------------------" + "\n";

            for(int i = 0; i < files.Count(); i++) {

                try {
                    System.IO.StreamReader file = new System.IO.StreamReader(files[i]);
                    GetData(file, files[i], np);
                } catch {

                    OutputTextBox.Text += "Could not get Data from: " + files[i] + "\n";
                }
            }
            OutputTextBox.Text += "Done Reading information" + "\n";
            OutputTextBox.Text += "\n\nWriting to .CSV file now" + "\n";
            OutputTextBox.Text += "Warning: any errors from this step may cause a corrupted .csv file" + "\n";
            OutputTextBox.Text += "---------------------------------------------------------------------" + "\n";
            try { 
            
                _writer = new CSVWriter(_dialog.SelectedPath.ToString() + "\\" + OutFileTextBox.Text + ".csv");
            } catch { 
            
                OutputTextBox.Text += "ERROR: COULD NOT OPEN OUTFILE";
                return;
            }
            WriteData(np);
            OutputTextBox.Text += "Done Writing to .CSV file" + "\n";

            OutputTextBox.Text += "\n\nChecking Standard Deviations" + "\n";
            OutputTextBox.Text += "----------------------------" + "\n";
            checkSTDDeveation(n);

            OutputTextBox.Text += "\n\nDone";

        }

        public void GetData(System.IO.StreamReader file, string filename, int n) {

            string line;

            FishData fd = new FishData();

            string[] parts = filename.Split('\\');
            parts = parts[parts.Count() - 1].Split('.');

            fd.Name = parts[0];

            line = file.ReadLine();
            if(null == line) {
                OutputTextBox.Text += "could not get the LM from: " + fd.Name + "\n";
                return;
            }
            parts = line.Split('=');

            try {
                fd.LM = Convert.ToInt32(parts[1]);
            } catch {

                OutputTextBox.Text += "could not get the LM from: " + fd.Name + "\n";
                return;
            }

            if(fd.LM != n) {

                OutputTextBox.Text += fd.Name + " does not have the correct LM" + "\n";
                return;
            }

            for(int i = 0; i < fd.LM; i++) {

                line = file.ReadLine();
                if(null == line) {
                    OutputTextBox.Text += "could not get the point " + i.ToString() + " from: " + fd.Name + "\n";
                    return;
                }
                parts = line.Split(' ');
                try {
                    fd.AddPoint(Convert.ToDouble(parts[0]), Convert.ToDouble(parts[1]));
                } catch {
                    OutputTextBox.Text += "could not get the point " + i.ToString() + " from: " + fd.Name + "\n";
                    return;
                }
            }

            line = file.ReadLine();
            parts = line.Split('=');
            fd.Image = parts[1];

            line = file.ReadLine();
            if(null == line) {
                OutputTextBox.Text += "could not get the ID from:  " + fd.Name + "\n";
                return;
            }
            parts = line.Split('=');
            try {
                fd.Id = Convert.ToInt32(parts[1]);
            } catch {

                OutputTextBox.Text += "could not get the ID from: " + fd.Name + "\n";
                return;
            }

            line = file.ReadLine();
            if(null == line) {
                OutputTextBox.Text += "could not get the scale from: " + fd.Name + "\n";
                return;
            }
            parts = line.Split('=');
            try {
                fd.Scale = Convert.ToDouble(parts[1]);
            } catch {
                OutputTextBox.Text += "could not get the scale from: " + fd.Name + "\n";
                return;
            }

            _data.Add(fd);
        }

        public void WriteData(int num) {

            for(int i = 0; i < _data.Count; i++) {//names 

                try {
                    _writer.addToCurrent(_data[i].Name);
                    _writer.addToCurrent("");
                } catch {

                    OutputTextBox.Text += "Error Writing the name: " + _data[i].Name + "\n";
                }
            }
            _writer.WriteLine();

            for(int i = 0; i < _data.Count; i++) {//X Y label

                try {
                    _writer.addToCurrent("X");
                    _writer.addToCurrent("Y");
                } catch {
                    OutputTextBox.Text += "Error with File: " + _data[i].Name + "\n";
                }

            }
            _writer.WriteLine();
            for(int n = 0; n < num; n++) {
                for(int i = 0; i < _data.Count; i++) {//Coordinates
                    try {
                        _writer.addToCurrent(_data[i].Points[n].X.ToString());
                        _writer.addToCurrent(_data[i].Points[n].Y.ToString());

                    } catch {
                        OutputTextBox.Text += "error, could not write number " + n.ToString() + " coordinates for: " + _data[i].Name + "\n";
                    }

                }
                _writer.WriteLine();
            }

            for(int i = 0; i < _data.Count; i++) {

                try {
                    _writer.addToCurrent("Scale: ");
                    _writer.addToCurrent(_data[i].Scale.ToString());
                } catch {

                    OutputTextBox.Text += "could not write scale for: " + _data[i].Name + "\n";
                }

            }
            _writer.WriteLine();

        }

        public void checkSTDDeveation(int num) {

            double mean = 0.0;

            foreach(FishData fd in _data) {

                mean += fd.Scale;
            }

            mean /= _data.Count;

            OutputTextBox.Text += "mean scale: " + mean.ToString() + "\n";

            double std_dev = 0.0;

            foreach(FishData fd in _data) {

                std_dev += Math.Pow((fd.Scale - mean), 2);
            }

            std_dev /= (_data.Count - 1);

            std_dev = Math.Sqrt(Math.Abs(std_dev));

            OutputTextBox.Text += "Standard Deviation: " + std_dev.ToString() + "\n";

            OutputTextBox.Text += "any files that are off by " + num.ToString() + " standard deviations will show up below:" + "\n";
            //check if number is more than num std_devs from the mean
            foreach(FishData fd in _data) {

                if((num * std_dev) < Math.Abs(fd.Scale - mean)) {

                    OutputTextBox.Text += fd.Name + "\n";
                }
            }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e) {
            HelpWindow helpwindow = new HelpWindow();
            
            helpwindow.addText("Puts Information From the .TPS files into a .CSV file");
            helpwindow.addText("");
            helpwindow.addText("First Make sure all target .TPS files are in the same directory");
            helpwindow.addText("");
            helpwindow.addText("Steps to use:");
            helpwindow.addText("1. Click on the 'Select Folder' button and navigate to the folder with your target .TPS files");
            helpwindow.addText("2. Set the number of points each of the .TPS files has [default is 15]");
            helpwindow.addText("3. Set the number of standard deviations to check for outlying scales [default is 3]");
            helpwindow.addText("4. Type in a name for an outfile. Do not type the '.CSV' that will be added. Just type the name");
            helpwindow.addText("            eg: 'out'");
            helpwindow.addText("5. Click go");
            helpwindow.addText("");
            helpwindow.addText("The Output Window will tell you what step the process is currently working on");
            helpwindow.addText("As well as an errors that occured on that step and the .TPS file that cuased that error");
            helpwindow.addText("");
            helpwindow.addText("When the process is done, you will see 'done' in the output window");
            helpwindow.addText("The .CSV file will be in the same folder as the .TPS files");

            helpwindow.Show();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
