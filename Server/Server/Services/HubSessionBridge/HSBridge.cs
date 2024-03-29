﻿using Microsoft.AspNetCore.Identity;
using Server.Models;
using Server.Services.CarcassoneGame;
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
        public void Ping(string userIdentity);
    }
    public class HSBridge : IBridge
    {
        public TimeSpan RegistrationTime { get; set; }

        private readonly List<ReadyToJoin> queue;
        private readonly Dictionary<string, UserData> dictionary;
        private readonly ICarcassonneGame _game;

        private readonly CancellationTokenSource cancellationToken;
        private readonly ILogger log;

        public HSBridge(ICarcassonneGame game, ILogger<HSBridge> logger)
        {
            RegistrationTime = new TimeSpan(0, 0, 30);
            queue = new List<ReadyToJoin>();
            dictionary = new Dictionary<string, UserData>();
            _game = game;
            log = logger;

            cancellationToken = new();
            var token = cancellationToken.Token;
            Task.Run(() => {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    DateTime old = DateTime.Now - TimeSpan.FromSeconds(3);
                    var toDelete = dictionary.Where(k => k.Value.LastPing < old).ToList();
                    toDelete.ForEach(k =>
                    {
                        log.LogInformation("Logout user {Nick} with id {Id}", k.Value.User.Nick, k.Key);
                        _game.Disconnect(k.Value.User);
                        dictionary.Remove(k.Key);
                    });
                }
            }, token);
        }

        ~HSBridge()
        {
            cancellationToken.Cancel();
        }

        public string RegisterUser(User user)
        {
            log.LogInformation("Register user {Nick}", user.Nick);
            RemoveOld();
            var item = queue.Find(i => i.User == user);
            if (item != null) return item.Key;
            string guid = Guid.NewGuid().ToString();
            queue.Add(new ReadyToJoin(user, guid));
            
            /*
             * No remove the same user from register but change connection id on join
             * 
            var list = dictionary.Where(k => k.Value.User.IdUser == user.IdUser).ToList();
            foreach (var i in list)
            {
                dictionary.Remove(i.Key);
                _game.Disconnect(i.Value.User);
                log.LogInformation(" + Remove him from dictionary");
            }*/
            return guid;
        }

        public bool Join(string key, string userIdentity)
        {
            RemoveOld();
            var item = queue.Find(i => i.Key == key);
            if (item == null) return false;
            log.LogInformation("Join user {Nick} with id {Id}", item.User.Nick, userIdentity);
            queue.Remove(item);
            if (dictionary.TryGetValue(userIdentity, out UserData? data))
            {
                _game.Disconnect(data.User);
                dictionary.Remove(userIdentity);
            }
            var list = dictionary.Where(k => k.Value.User.IdUser == item.User.IdUser);
            if (list.Any())
            {
                // There is another device logged in as the same user
                foreach (var k in list.ToList())
                {
                    dictionary.Remove(k.Key);
                }
                _game.ReAddConnection(userIdentity, item.User);
            }
            dictionary.Add(userIdentity, new UserData(item.User));
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
            var value = dictionary.TryGetValue(userIdentity, out UserData data);
            if (value) user = data.User; else user = new User();
            return value;
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

        public void Ping(string userIdentity)
        {
            dictionary.Where(k => k.Key == userIdentity).ToList().ForEach(k => k.Value.Ping());
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

        class UserData
        {
            public User User { get; set; }
            public DateTime LastPing { get; set; }
            public UserData(User user)
            {
                User = user;
                Ping();
            }
            public void Ping() => LastPing = DateTime.Now;
        }
    }
}
