using DesktopSolution.Miscellaneous;
using DesktopSolution.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopSolution
{
    public class CommentWindowViewModel : INotifyPropertyChanged
    {
        private Window windowInstance;

        private Comment comment;
        public Comment Comment
        {
            get
            {
                return comment;
            }
            set
            {
                comment = value;
                OnPropertyChanged("Comment");
            }
        }

        public CommentWindowViewModel(Window windowInstance, Comment comment)
        {
            this.windowInstance = windowInstance;
            Comment = comment;
        }

        public RelayCommand OkCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    windowInstance.Close();
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
