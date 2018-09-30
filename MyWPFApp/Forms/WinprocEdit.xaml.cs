using System.Windows;

namespace MyWPFapp
{
    public partial class WinprocEdit : Window
    {
        public Winproc Winproc { get; private set; }

        public WinprocEdit(Winproc p)
        {
            InitializeComponent();
            Winproc = p;
            this.DataContext = Winproc;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}