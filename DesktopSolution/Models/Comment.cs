using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DesktopSolution.Models
{
    public class Comment : INotifyPropertyChanged
    {
        private long id;
        private long userId;
        private string text;
        private bool isNew;
        private bool isDeleted;

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
        public long UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
                OnPropertyChanged("UserId");
            }
        }
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }
        public bool IsNew
        {
            get
            {
                return isNew;
            }
            set
            {
                isNew = value;
                OnPropertyChanged("IsNew");
            }
        }
        public bool IsDeleted
        {
            get
            {
                return isDeleted;
            }
            set
            {
                isDeleted = value;
                OnPropertyChanged("IsDeleted");
            }
        }

        public static Comment ParseFromJson(JsonObject user)
        {
            if (!(
                user.ContainsKey("id") &&
                user.ContainsKey("userId") &&
                user.ContainsKey("text")
                ))
            {
                throw new Exception("cant find necessary fields in json");
            }

            long id = user["id"]!.GetValue<long>();
            long userId = user["userId"]!.GetValue<long>();
            string text = user["text"]!.GetValue<string>();

            return new Comment()
            {
                Id = id,
                UserId = userId,
                Text = text
            };
        }

        public static List<Comment> ParseArrayFromJson(string response)
        {
            List<Comment> parsed = new List<Comment>();
            var array = JsonArray.Parse(response);

            foreach (var t in array.AsArray())
            {
                var obj = t.AsObject();
                parsed.Add(ParseFromJson(obj));
            }

            return parsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
