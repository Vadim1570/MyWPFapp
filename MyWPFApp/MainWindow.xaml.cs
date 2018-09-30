using System;
using System.Windows;
using System.Windows.Media.Imaging;
using WPF.MDI;
using System.Windows.Controls;
using MyWPFapp.Forms;

namespace MyWPFapp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _original_title = Title;
            Container.Children.CollectionChanged += (o, e) => Menu_RefreshWindows();
            Container.MdiChildTitleChanged += Container_MdiChildTitleChanged;
        }

        #region Mdi-like title

        string _original_title;

        void Container_MdiChildTitleChanged(object sender, RoutedEventArgs e)
        {
            if (Container.ActiveMdiChild != null && Container.ActiveMdiChild.WindowState == WindowState.Maximized)
                Title = _original_title + " - [" + Container.ActiveMdiChild.Title + "]";
            else
                Title = _original_title;
        }

        #endregion

        #region Standart Menu Events

        private void Generic_Click(object sender, RoutedEventArgs e)
        {
            Generic.IsChecked = true;
            Luna.IsChecked = false;
            Aero.IsChecked = false;

            Container.Theme = ThemeType.Generic;
        }

        private void Luna_Click(object sender, RoutedEventArgs e)
        {
            Generic.IsChecked = false;
            Luna.IsChecked = true;
            Aero.IsChecked = false;

            Container.Theme = ThemeType.Luna;
        }

        private void Aero_Click(object sender, RoutedEventArgs e)
        {
            Generic.IsChecked = false;
            Luna.IsChecked = false;
            Aero.IsChecked = true;

            Container.Theme = ThemeType.Aero;
        }

        void Menu_RefreshWindows()
        {
            WindowsMenu.Items.Clear();
            MenuItem mi;
            for (int i = 0; i < Container.Children.Count; i++)
            {
                MdiChild child = Container.Children[i];
                mi = new MenuItem { Header = child.Title };
                mi.Click += (o, e) => child.Focus();
                WindowsMenu.Items.Add(mi);
            }
            WindowsMenu.Items.Add(new Separator());
            WindowsMenu.Items.Add(mi = new MenuItem { Header = "Cascade" });
            mi.Click += (o, e) => Container.MdiLayout = MdiLayout.Cascade;
            WindowsMenu.Items.Add(mi = new MenuItem { Header = "Horizontally" });
            mi.Click += (o, e) => Container.MdiLayout = MdiLayout.TileHorizontal;
            WindowsMenu.Items.Add(mi = new MenuItem { Header = "Vertically" });
            mi.Click += (o, e) => Container.MdiLayout = MdiLayout.TileVertical;

            WindowsMenu.Items.Add(new Separator());
            WindowsMenu.Items.Add(mi = new MenuItem { Header = "Close all" });
            mi.Click += (o, e) => Container.Children.Clear();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            StackPanel sp = new StackPanel { Orientation = Orientation.Vertical };
            sp.Children.Add(new TextBlock { Text = "WPF MDI Application", Margin = new Thickness(5) });
            sp.Children.Add(new TextBlock { Text = String.Format("Ver = {0}", System.Reflection.Assembly.GetEntryAssembly().GetName().Version), Margin = new Thickness(5) });
            Container.Children.Add(new MdiChild { Content = sp, Title = "About" });
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Menu Events

        private void RunningProcesses_Click(object sender, RoutedEventArgs e)
        {
            Container.Children.Add(new MdiChild
            {
                Title = "RunningProcesses",
                Content = new WinprocList(),
                Width = 800,
                Height = 600,
                Position = new Point(0, 0)
            });
        }

        #endregion
    }
}