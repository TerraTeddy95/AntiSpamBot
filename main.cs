using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using TShockAPI;
using TerrariaApi.Server;
using System.Timers;
using Newtonsoft.Json;
using System.IO;

namespace AntiSpamBot
{
    [ApiVersion(1, 21)]
    public class main : TerrariaPlugin
    {

        public static Config Config;

        public main(Main game) : base(game) { Order -= 1; }

        public override Version Version { get { return new Version("1.0"); } }
        public override string Name { get { return "AntiSpamBot"; } }
        public override string Author { get { return "Teddy"; } }
        public override string Description { get { return "Block spam"; } }

        public override void Initialize()
        {
            string path = Path.Combine(TShock.SavePath, "AntiSpamBot.json");
            Config = Config.Read(path);
            if (!File.Exists(path))
            {
                Config.Write(path);
            }

            vars.Message1 = Config.Message1;
            vars.Message2 = Config.Message2;
            vars.Permission = Config.Permission;
            vars.Time = Config.Time;
            vars.Enable = Config.Enable;
            vars.EnableMessages = Config.EnableMessages;
            vars.cmdPermission = Config.cmdPermission;
            

            Commands.ChatCommands.Add(new Command(vars.cmdPermission, funcCmd, "antispambot"));
            ServerApi.Hooks.ServerChat.Register(this, onChat);

        }




        public void funcCmd(CommandArgs e)
        {
            if (e.Parameters.Count < 1)
            {
                e.Player.SendMessage("[AntiSpamBot] Please do /antispambot help", Color.Silver);
                return;
            }
            else
            {
                if (e.Parameters[0] == "help" || e.Parameters[0] == "reload")
                {
                    if (e.Parameters[0] == "help")
                    {
                        e.Player.SendMessage("[AntiSpamBot] Commands:", Color.Green);
                        e.Player.SendMessage("/antispambot reload", Color.Silver);
                        e.Player.SendMessage("/antispambot help", Color.Silver);
                        return;
                    }
                    if (e.Parameters[0] == "reload")
                    {
                        string path = Path.Combine(TShock.SavePath, "AntiSpamBot.json");
                        Config = Config.Read(path);
                        if (!File.Exists(path))
                        {
                            Config.Write(path);
                        }
                        vars.Message1 = Config.Message1;
                        vars.Message2 = Config.Message2;
                        vars.Permission = Config.Permission;
                        vars.Time = Config.Time;
                        vars.EnableMessages = Config.EnableMessages;
                        vars.Enable = Config.Enable;
                        vars.cmdPermission = Config.cmdPermission;
                        e.Player.SendMessage("[AntiSpamBot] Plugin has been successfully reloaded.", Color.Green);
                        return;
                    }
                }
                else
                {
                    e.Player.SendMessage("[AntiSpamBot] Please do /antispambot help", Color.Silver);
                    return;
                }
            }
        }


        public void onChat(ServerChatEventArgs e)
        {
            if (e.Text[0] == '/' || e.Text[0] == '.')
            {
                if (e.Text.Length > 3)
                {
                    if (!(e.Text[1] == 'm' && e.Text[2] == 'e' && e.Text[3] == ' '))
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            if(e.Handled == true)
            {
                return;
            }
            if(vars.Enable == false)
            {
                return;
            }
            String sendername = TShock.Players[e.Who].Name;
            TSPlayer sender = TShock.Players[e.Who];

            if (!(sender.Group.HasPermission(vars.Permission)))
            {
                return;
            }
            if (vars.lastMessageTime.ContainsKey(sendername))
            {
                string lastMessage = vars.lastMessage[sendername];

                if (lastMessage == e.Text)
                {
                    if (vars.EnableMessages == true)
                    {
                        sender.SendMessage("[AntiSpamBot] " + vars.Message2, Color.Silver);
                    }
                    e.Handled = true;
                    return;
                }

                int time = (int)(TimeSpan.FromMilliseconds(((int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)) - vars.lastMessageTime[sendername]).TotalSeconds);

                if (time < vars.Time)
                {
                    if (vars.EnableMessages == true)
                    {
                        string message = vars.Message1.Replace("%time%", "" + (vars.Time - time));
                        sender.SendMessage("[AntiSpamBot] " + message, Color.Silver);
                    }
                    e.Handled = true;
                    return;
                }
                if (e.Handled == false)
                {
                    vars.lastMessageTime[sendername] = (int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                    vars.lastMessage[sendername] = e.Text;
                }
                return;
            }
            else
            {
                vars.lastMessageTime.Add(sendername, (int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond));
                vars.lastMessage.Add(sendername, "" + e.Text);
            }
        }
    }
}