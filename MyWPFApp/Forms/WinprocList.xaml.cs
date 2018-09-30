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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Management;
using System.ComponentModel;
using System.Data.Entity;

namespace MyWPFapp.Forms
{
    public partial class WinprocList : UserControl
    {
        ApplicationContext db;

        public WinprocList()
        {
            InitializeComponent();

            Width = double.NaN;
            Height = double.NaN;

            db = new ApplicationContext();
            db.Snapshots.Load();
            snapshotGrid.DataContext = db.Snapshots.Local.ToBindingList();
            winprocGrid.DataContext = db.Winprocs.Local.ToBindingList();

            snapshotGrid.SelectionChanged += snapshotGrid_SelectionChanged;
            snapshotGrid_SelectionChanged(this, null);
        }

        private void RefreshList_Click(object sender, RoutedEventArgs e)
        {
            List<Winproc> p = new List<Winproc>();
            try
            {
                string name = "";
                string fullpath = "";
                string args = "";

                string query = "SELECT Name, CommandLine, ProcessId, Caption, ExecutablePath FROM Win32_Process";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                foreach (ManagementObject mo in searcher.Get())
                {
                    try { name = mo["Name"].ToString(); } catch { }
                    try { fullpath = mo["ExecutablePath"].ToString(); } catch { }
                    try { args = mo["CommandLine"].ToString(); } catch { }
                    p.Add(new Winproc { Name = name, Fullpath = fullpath, Args = args });
                }

            }
            catch { }

            winprocGrid.DataContext = p;
        }

        private void snapshotGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Snapshot snap = snapshotGrid.SelectedItem as Snapshot;
            if (snap != null)
            {
                db.Winprocs.Where(wp => wp.IdSnapshot == snap.Id).Load();
                winprocGrid.DataContext = db.Winprocs.Local.Where(wp => wp.IdSnapshot == snap.Id).ToList();
            }
        }

        private void winprocGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void mi_ApplyDBChanges_Click(object sender, RoutedEventArgs e)
        {
            var dc = winprocGrid.DataContext;
            if(dc != null && dc.GetType() == typeof(List<Winproc>) && ((List<Winproc>)dc).Count > 0)
            {
                List<Winproc> wpList = (List<Winproc>)dc;
                if(wpList.First().IdSnapshot == 0)
                {
                    Snapshot snap = snapshotGrid.SelectedItem as Snapshot;
                    if (snap == null)
                    {
                        snap = new Snapshot() { Comment = "", CreatedDt = DateTime.Now };
                        db.Snapshots.Add(snap);
                    }
                    
                    db.SaveChanges();

                    foreach (Winproc proc in wpList)
                    {
                        proc.Snapshot = snap;
                        proc.IdSnapshot = snap.Id;
                        db.Winprocs.Add(proc);
                    }
                    db.SaveChanges();
                }
                else
                    db.SaveChanges();
            }
            else
                db.SaveChanges();
        }

        private void mi_EditWinproc_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (winprocGrid.SelectedItem == null) return;
            // получаем выделенный объект
            Winproc proc = winprocGrid.SelectedItem as Winproc;

            WinprocEdit form = new WinprocEdit(new Winproc
            {
                Id = proc.Id,
                Name = proc.Name,
                Fullpath = proc.Fullpath,
                Args = proc.Args
            });

            if (form.ShowDialog() == true)
            {
                // получаем измененный объект
                proc = db.Winprocs.Find(form.Winproc.Id);
                if (proc != null)
                {
                    proc.Name = form.Winproc.Name;
                    proc.Fullpath = form.Winproc.Fullpath;
                    proc.Args = form.Winproc.Args;
                    db.Entry(proc).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        private void mi_DeleteWinproc_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (winprocGrid.SelectedItem == null) return;
            // получаем выделенный объект
            Winproc proc = winprocGrid.SelectedItem as Winproc;
            db.Winprocs.Remove(proc);
            db.Winprocs.Local.Remove(proc);
            db.SaveChanges();
        }
    }
}
