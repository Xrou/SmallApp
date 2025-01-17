﻿using DesktopSolution.Miscellaneous;
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
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow(User? user = null)
        {
            InitializeComponent();
            DataContext = new UserWindowViewModel(this, user);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Notifications.CallUsersChanged();
        }
    }
}
