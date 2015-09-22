using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiSpamBot
{
    public class vars
    {
        public static bool Enable = true;
        public static bool EnableMessages = false;
        public static int Time = 6;
        public static string Message1 = "Please wait %time% seconds and try again!";
        public static string Message2 = "You can not write 2x the same.";
        public static string Permission = "antispambot.bypass";
        public static string cmdPermission = "antispambot.admin";

        public static Dictionary<string, int> lastMessageTime = new Dictionary<string, int>();
        public static Dictionary<string, string> lastMessage = new Dictionary<string, string>();

    }
}
