using Microsoft.AspNetCore.Identity;
using Server.Models;
using System;
using System.Text.Json;

namespace Server.Services.HubSessionBridge
{
    public interface IBridge
    {
        public TimeSpan RegistrationTime { get; set; }
        public string RegisterUser(User user);
        public bool Join(string key, string userIdentity);
        public void Leave(string userIdentity);
        public bool GetUser(string userIdentity, out User user);
    }
    public class HSBridge : IBridge
    {
        public TimeSpan RegistrationTime { get; set; }

        private readonly List<ReadyToJoin> queue;
        private readonly Dictionary<string, User> dictionary;

        public HSBridge()
        {
            RegistrationTime = new TimeSpan(0, 0, 30);
            queue = new List<ReadyToJoin>();
            dictionary = new Dictionary<string, User>();
        }

        public string RegisterUser(User user)
        {
            RemoveOld();
            var item = queue.Find(i => i.User == user);
            if (item != null) return item.Key;
            string guid = Guid.NewGuid().ToString();
            queue.Add(new ReadyToJoin(user, guid));
            return guid;
        }

        public bool Join(string key, string userIdentity)
        {
            RemoveOld();
            var item = queue.Find(i => i.Key == key);
            if (item == null) return false;
            queue.Remove(item);
            dictionary.Remove(userIdentity);
            if (dictionary.Where(k => k.Value.IdUser == item.User.IdUser).Any())
                // There is another device logged in as the same user
                // Cancel this request or logout him
                return false; // change to logout second
            dictionary.Add(userIdentity, item.User);
            return true;
        }

        public void Leave(string userIdentity)
        {
            RemoveOld();
            dictionary.Remove(userIdentity);
        }

        public bool GetUser(string userIdentity, out User user)
        {
            RemoveOld();
            return dictionary.TryGetValue(userIdentity, out user);
        }

        private void RemoveOld()
        {
            DateTime old = DateTime.Now - RegistrationTime;
            for (int i = queue.Count - 1; i >= 0; i--)
            {
                if (queue[i].RegisterTime < old)
                {
                    queue.RemoveAt(i);
                }
            }
        }

        class ReadyToJoin
        {
            public User User { get; }
            public string Key { get; }
            public DateTime RegisterTime { get; }
            public ReadyToJoin(User user, string key)
            {
                User = user;
                Key = key;
                RegisterTime = DateTime.Now;
            }
        }
    }
}
