using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyWPFapp
{
    public class Snapshot : INotifyPropertyChanged
    {        
        private DateTime createddt;
        private string comment;

        public int Id { get; set; }

        public DateTime CreatedDt
        {
            get { return createddt; }
            set
            {
                createddt = value;
                OnPropertyChanged("Createddt");
            }
        }

        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged("Comment");
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