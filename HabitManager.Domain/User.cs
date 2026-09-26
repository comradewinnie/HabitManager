using System;
using System.Collections.Generic;
using System.Text;

namespace HabitManager.Domain
{
    public class User
    {
        public string Id { get; set; }
        public User(string id)
        {
            Id = id;
        }
    }
}
