using System;
using System.Collections.Generic;
using System.IO;
using TShockAPI;
using Newtonsoft.Json;

namespace AntiSpamBot
{
    public class Config
    {
        public bool Enable = true;
        public bool EnableMessages = false;
        public int Time = 6;
        public string Message1 = "Please wait %time% seconds and try again!";
        public string Message2 = "You can not write 2x the same.";
        public string Permission = "antispambot.bypass";
        public string cmdPermission = "antispambot.admin"; 




        public void Write(string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
        public static Config Read(string path)
        {
            return File.Exists(path) ? JsonConvert.DeserializeObject<Config>(File.ReadAllText(path)) : new Config();
        }
    }
}