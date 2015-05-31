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
using System.Windows.Shapes;
using System.Windows.Forms;

namespace GUIFish {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow:Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void Help_Button_Click(object sender, RoutedEventArgs e) {
            HelpWindow helpwindow = new HelpWindow();

            helpwindow.addText("Fish Sticks Automation Tools v 2.0.0");
            helpwindow.addText("    Ryan Huard");

            helpwindow.addText("");
            helpwindow.addText("Intended Purpse:");
            helpwindow.addText("This program automates creating .TPS files corresponding to .JPEG files\n and creating a .CSV file out of .TPS files");
            helpwindow.addText("");
            helpwindow.addText("To Start:");
            helpwindow.addText("Click on the task you wish to automate, another window will pop where you can complete your task");
            helpwindow.addText("click on the help button on that form if you need help");
            helpwindow.addText("");
            helpwindow.addText("");
            helpwindow.addText("Contact:");
            helpwindow.addText("If you have any questions, sugestions or bugs, please contact me at:");
            helpwindow.addText("ryan.huard@email.wsu.edu");


            helpwindow.Show();
        }

        private void Tps_To_Cvs_Button_Click(object sender, RoutedEventArgs e) {
            MakeCSVWindow csvform = new MakeCSVWindow();
            csvform.Show();
        }

        private void Make_TPS_Buttun_Click(object sender, RoutedEventArgs e) {
            MakeTPSForm tpsform = new MakeTPSForm();
            tpsform.Show();
        }
    }
}
