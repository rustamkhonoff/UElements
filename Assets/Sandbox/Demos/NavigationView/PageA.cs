using System;
using System.IO;
using MessagePack;
using R3;
using TMPro;
using UElements;
using UElements.R3;
using UnityEngine;
using UnityEngine.UI;

namespace Demos.NavigationView
{
    public class PageA : Element
    {
        [SerializeField] private User _user;
        [SerializeField] private TMP_InputField _name, _age, _mail;
        [SerializeField] private Button _save, _load;

        [Serializable] [MessagePackObject]
        public class User
        {
            public User(string name, int age, string mail)
            {
                Name = name;
                Age = age;
                Mail = mail;
            }

            [Key(0)] public string Name { get; set; }
            [Key(1)] public int Age { get; set; }
            [Key(2)] public string Mail { get; set; }
        }

        protected override void Initialize()
        {
            _save.SubscribeClick(Save).AddTo(this);
            _load.SubscribeClick(Load).AddTo(this);
            _name.SubscribeEndEdit(a => _user.Name = a).AddTo(this);
            _age.SubscribeEndEdit(a => _user.Age = int.Parse(a)).AddTo(this);
            _mail.SubscribeEndEdit(a => _user.Mail = a).AddTo(this);
        
            Load();
        }

        private void Save()
        {
            byte[] bytes = MessagePackSerializer.Serialize(_user);
            File.WriteAllBytes(FilePath, bytes);

            User tempUser = MessagePackSerializer.Deserialize<User>(bytes);
            Debug.Log(tempUser.Age + ":" + tempUser.Name + ":" + tempUser.Mail);

            Debug.Log("Saved");
        }

        private void Load()
        {
            if (File.Exists(FilePath))
            {
                byte[] bytes = File.ReadAllBytes(FilePath);
                _user = MessagePackSerializer.Deserialize<User>(bytes);
            }
            else
            {
                _user = new User("default", 0, "default@gmail.com");
            }

            _name.text = _user.Name;
            _mail.text = _user.Mail;
            _age.text = _user.Age.ToString();
        }

        private static string FilePath => Path.Combine(Application.persistentDataPath, "user_save.bin");
    }
}