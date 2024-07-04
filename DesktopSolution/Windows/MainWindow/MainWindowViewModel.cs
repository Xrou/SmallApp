using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DesktopSolution.Miscellaneous;
using DesktopSolution.Models;

namespace DesktopSolution
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<User> users;
        public ObservableCollection<User> Users
        {
            get
            {
                return users;
            }
            set
            {
                users = value;
                OnPropertyChanged("Users");
            }
        }

        private User? selectedUser;
        public User? SelectedUser
        {
            get
            {
                return selectedUser;
            }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }
        public int Port
        {
            get { return ConnectionSettings.Port; }
            set
            {
                ConnectionSettings.Port = value;
                OnPropertyChanged("Port");
            }
        }

        public MainWindowViewModel()
        {
            Notifications.UsersChanged += UsersChangedOutside;
            Task.Run(() => LoadUsers());
        }

        public async Task LoadUsers()
        {
            var response = HttpClient.Send("api/users", RestSharp.Method.Get);

            if (response.code == System.Net.HttpStatusCode.OK)
            {
                Users = new ObservableCollection<User>(User.ParseArrayFromJson(response.response));
            }
        }

        public RelayCommand RefreshCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    Task.Run(() => LoadUsers());
                });
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    UserWindow window = new UserWindow();
                    window.ShowDialog();
                });
            }
        }

        public RelayCommand EditCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (SelectedUser != null)
                    {
                        UserWindow window = new UserWindow(SelectedUser);
                        window.ShowDialog();
                    }
                });
            }
        }

        public RelayCommand DeleteCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (SelectedUser != null)
                    {
                        Task.Run(() =>
                        {
                            var response = HttpClient.Send($"api/users/{SelectedUser.Id}", RestSharp.Method.Delete);

                            if (response.code == System.Net.HttpStatusCode.OK)
                            {
                                Task.Run(() => LoadUsers());
                                SelectedUser = null;
                            }
                        });
                    }
                });
            }
        }

        private void UsersChangedOutside()
        {
            Task.Run(() => LoadUsers());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
