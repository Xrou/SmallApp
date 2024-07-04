using DesktopSolution.Models;
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

namespace DesktopSolution
{
    /// <summary>
    /// Логика взаимодействия для CommentWindow.xaml
    /// </summary>
    public partial class CommentWindow : Window
    {
        public CommentWindow(Comment comment)
        {
            InitializeComponent();
            DataContext = new CommentWindowViewModel(this, comment);
        }

        public Comment GetComment()
        {
            return (DataContext as CommentWindowViewModel)!.Comment;
        }
    }
}
