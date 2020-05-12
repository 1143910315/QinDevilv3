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

namespace QinDevilTest {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e) {
            Debug.WriteLine("TextBox_GotFocus");
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e) {
            Debug.WriteLine("TextBox_LostFocus");
        }
        private void Window_GotFocus(object sender, RoutedEventArgs e) {
            Debug.WriteLine("Window_GotFocus");
        }
        private void Window_LostFocus(object sender, RoutedEventArgs e) {
            Debug.WriteLine("Window_LostFocus");
        }
        private void TextBox_GotFocus_1(object sender, RoutedEventArgs e) {
            Debug.WriteLine("TextBox_GotFocus_1");
        }
        private void TextBox_LostFocus_1(object sender, RoutedEventArgs e) {
            Debug.WriteLine("TextBox_LostFocus_1");
        }
        private void Window_Activated(object sender, EventArgs e) {
            Debug.WriteLine("Window_Activated");
        }
        private void Window_Deactivated(object sender, EventArgs e) {
            Debug.WriteLine("Window_Deactivated");
        }
    }
}
