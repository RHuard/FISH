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

namespace GUIFish {
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow:Window {
        public HelpWindow() {
            InitializeComponent();
            HelpTextBox.IsReadOnly = true;
        }

        public void addText(string text) { 
        
            HelpTextBox.Text += text + "\n";
        }

        private void OKButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
