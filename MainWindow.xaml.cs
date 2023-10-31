using ModdingTool.View.UserControls;
using ModdingTool.ViewModel;
using System.Windows;

namespace ModdingTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataListLive.ListChanged += OnListChanged;
        }

        private void OnListChanged(object? sender, ListChangedEventArgs e)
        {
            if (ToolBox.DataContext is ToolBoxViewModel toolboxViewModel)
            {
                toolboxViewModel.TabType = e.SelectedListType;
                toolboxViewModel.SortList = e.SelectedList;
            };
        }

        private void MenuBarCustom_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}