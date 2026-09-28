using System;
using System.Collections.Generic;
using System.Text;

namespace HabitManager.Domain
{
    public class User
    {
        private string _id;

        public string Id
        {
            get
            {
                return _id;
            }
            init
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("ID cannot be empty.");

                _id = value;
            }
        }

        public User(string id)
        {
            Id = id;
        }
    }
}
