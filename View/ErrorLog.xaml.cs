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
using static ModdingTool.Globals;

namespace ModdingTool.View
{
    /// <summary>
    /// Interaction logic for ErrorLog.xaml
    /// </summary>
    public partial class ErrorLog : Window
    {
        private List<string> errorList;

        public ErrorLog()
        {
            InitializeComponent();
            errorList = ErrorDb.GetErrors();
        }

        public void WriteErrors()
        {
            ErrorLogBox.Text = "";
            errorList = ErrorDb.GetErrors();
            foreach (var error in errorList)
            {
                ErrorLogBox.Text += error + "\n";
            }
        }


    }
}
