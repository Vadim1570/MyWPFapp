using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace MyWPFapp
{
    public class Winproc : INotifyPropertyChanged
    {
        private string name;
        private string fullpath;
        private string args;

        public int Id { get; set; }

        public int IdSnapshot { get; set; }

        [ForeignKey("IdSnapshot")]
        public Snapshot Snapshot { get; set; }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Fullpath
        {
            get { return fullpath; }
            set
            {
                fullpath = value;
                OnPropertyChanged("Fullpath");
            }
        }
        public string Args
        {
            get { return args; }
            set
            {
                args = value;
                OnPropertyChanged("args");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}