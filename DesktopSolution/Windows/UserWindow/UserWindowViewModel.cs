using DesktopSolution.Miscellaneous;
using DesktopSolution.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopSolution
{
    class UserWindowViewModel
    {
        private List<Comment> deletedComments = new List<Comment>();

        private bool IsNewUser = true;
        private Window windowInstance;

        private User user;
        public User User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }

        private Comment selectedComment;
        public Comment SelectedComment
        {
            get
            {
                return selectedComment;
            }
            set
            {
                selectedComment = value;
                OnPropertyChanged("SelectedComment");
            }
        }


        public UserWindowViewModel(Window windowInstance, User? user = null)
        {
            this.windowInstance = windowInstance;

            if (user != null)
            {
                IsNewUser = false;
                User = user;
            }
            else
            {
                User = new User();
            }
        }

        public RelayCommand DeleteCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (SelectedComment != null)
                    {
                        deletedComments.Add(SelectedComment);
                        User.Comments.Remove(SelectedComment);

                        SelectedComment = null;
                    }
                });
            }
        }

        public RelayCommand EditCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (SelectedComment != null)
                    {
                        CommentWindow window = new CommentWindow(SelectedComment);
                        window.ShowDialog();
                        SelectedComment = window.GetComment();
                    }
                });
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    CommentWindow window = new CommentWindow(new Comment() { IsNew = true });
                    window.ShowDialog();

                    Comment? newComment = window.GetComment();

                    if (newComment != null)
                    {
                        User.Comments.Add(newComment);
                    }
                });
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (IsNewUser)
                    {
                        var response = HttpClient.Send("api/users", RestSharp.Method.Post,
                            body: new Dictionary<string, object>()
                            {
                                { "name", User.Name}
                            }
                        );

                        if (response.code == System.Net.HttpStatusCode.OK)
                        {
                            long userId = long.Parse(response.response);

                            foreach (Comment comment in User.Comments)
                            {
                                response = HttpClient.Send("api/comments", RestSharp.Method.Post,
                                    body: new Dictionary<string, object>()
                                    {
                                        { "text", comment.Text},
                                        { "userId", userId }
                                    }
                                );
                            }

                            Notifications.CallUsersChanged();
                            windowInstance.Close();
                        }
                    }
                    else
                    {
                        var response = HttpClient.Send($"api/users/{User.Id}", RestSharp.Method.Put,
                            body: new Dictionary<string, object>()
                            {
                                { "name", User.Name}
                            }
                        );

                        if (response.code == System.Net.HttpStatusCode.OK)
                        {
                            long userId = long.Parse(response.response);

                            foreach (Comment comment in User.Comments)
                            {
                                if (comment.IsNew)
                                {
                                    response = HttpClient.Send("api/comments", RestSharp.Method.Post,
                                        body: new Dictionary<string, object>()
                                        {
                                            { "text", comment.Text},
                                            { "userId", userId }
                                        }
                                    );
                                }
                                else
                                {
                                    response = HttpClient.Send($"api/comments/{comment.Id}", RestSharp.Method.Put,
                                        body: new Dictionary<string, object>()
                                        {
                                            { "text", comment.Text},
                                        }
                                    );
                                }
                            }

                            foreach (Comment comment in deletedComments)
                            {
                                response = HttpClient.Send($"api/comments/{comment.Id}", RestSharp.Method.Delete);
                            }

                            windowInstance.Close();
                        }
                    }
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
