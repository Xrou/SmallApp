using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DesktopSolution.Models
{
    public class User : INotifyPropertyChanged
    {
        private long id;
        private string name;
        private ObservableCollection<Comment> comments = new ObservableCollection<Comment>();

        public long Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public ObservableCollection<Comment> Comments
        {
            get
            {
                return comments;
            }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public static User ParseFromJson(JsonObject user)
        {
            if (!(
                user.ContainsKey("id") &&
                user.ContainsKey("name") &&
                user.ContainsKey("comments")
                ))
            {
                throw new Exception("cant find necessary fields in json");
            }

            long id = user["id"]!.GetValue<long>();
            string name = user["name"]!.GetValue<string>();
            ObservableCollection<Comment> comments = new ObservableCollection<Comment>();

            foreach (var comment in user["comments"]!.AsArray())
            {
                comments.Add(Comment.ParseFromJson(comment.AsObject()));
            }

            return new User()
            {
                Id = id,
                Name = name,
                Comments = comments
            };
        }

        public static List<User> ParseArrayFromJson(string response)
        {
            List<User> parsed = new List<User>();
            var array = JsonArray.Parse(response);

            foreach (var t in array.AsArray())
            {
                var obj = t.AsObject();
                parsed.Add(ParseFromJson(obj));
            }

            return parsed;
        }
    }
}
