using AlphaCodex.Data;
using AlphaCodex.Models;
using AlphaCodex.ViewModels;
using System;
using System.Windows;

namespace AlphaCodex
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var counter = DateTime.Now;
            MainRepository main = new MainRepository();
            base.OnStartup(e); MainWindow window = new MainWindow();
            // Create the ViewModel to which 
            // the main window binds. 
            //string path = "LyndisLibrary.sqlite;Version=3;";
            var viewModel = new MainViewModel(main);
            // Allow all controls in the window to 
            // bind to the ViewModel by setting the 
            // DataContext, which propagates down 
            // the element tree. 
            window.DataContext = viewModel;
            System.Diagnostics.Debug.WriteLine("[Data loaded]Elapsed time: {0} seconds.", (DateTime.Now - counter).Seconds);
            window.Show();
            System.Diagnostics.Debug.WriteLine("[Window ready]Elapsed time: {0} seconds.", (DateTime.Now - counter).Seconds);
        }
    }
}
