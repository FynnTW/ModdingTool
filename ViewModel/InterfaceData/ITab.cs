using CommunityToolkit.Mvvm.Input;
using ModdingTool.View.UserControls;
using ModdingTool.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ModdingTool.ViewModel.InterfaceData;

 public abstract class Tab : ObservableObject
 {
     public Tab()
     {
         TabType = Globals.OpenTabType;
         CloseCommand = new RelayCommand(Close);
     }

     public Tab(string name)
     {
         TabType = Globals.OpenTabType;
         CloseCommand = new RelayCommand(Close);
         Title = name;
     }

     public Tab(string name, string tabType)
     {
         TabType = tabType;
         CloseCommand = new RelayCommand(Close);
         Title = name;
     }
    
     public string TabType { get; set; } = "";
     
     private string _title;

     public string Title
     {
         get { return _title; }
         set { SetProperty(ref _title, value); }
     }
     
     public ICommand CloseCommand { get; private set; }
     public event EventHandler CloseRequested;

     private void Close()
     {

         MainWindow window = (ModdingTool.MainWindow)App.Current.MainWindow;
         var datatab = window?.FindName("DataTabLive") as DataTab;
         // Remove the tab from the collection of tabs
         var dataViewModel = datatab?.DataContext as DataTabViewModel;
         dataViewModel?.Tabs.Remove(this);

         // Trigger the CloseRequested event
         CloseRequested?.Invoke(this, EventArgs.Empty);
     }
 }