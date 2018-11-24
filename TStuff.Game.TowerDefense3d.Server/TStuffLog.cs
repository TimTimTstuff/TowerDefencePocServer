using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TStuff.Game.TowerDefense3d.Server
{
    public enum LogLevel
    {
        Trace,
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }
   public static class TStuffLog
    {
       public static List<Action<string,string,LogLevel,string,string,int>> LogActions  = new List<Action<string, string, LogLevel, string,string,int>>();
       public static LogLevel LogLevel = LogLevel.Info;
       public static LogLevel LogFileLevel { get; set; } = LogLevel.Error;

       public static void Log(object msg, LogLevel level = LogLevel.Info, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
       {
           LogActions.ForEach(a=>a(msg.ToString(),JsonConvert.SerializeObject(msg),level,memberName,sourceFilePath,sourceLineNumber));
       }

       public static void Info(string msg,[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
       {
            LogActions.ForEach(a => a(msg.ToString(), JsonConvert.SerializeObject(msg), LogLevel.Info, memberName, sourceFilePath, sourceLineNumber));
        }
        public static void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
       [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
       [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            LogActions.ForEach(a => a(msg.ToString(), JsonConvert.SerializeObject(msg), LogLevel.Trace, memberName, sourceFilePath, sourceLineNumber));
        }

        public static void Debug(string msg, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
       [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
       [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            LogActions.ForEach(a => a(msg.ToString(), JsonConvert.SerializeObject(msg), LogLevel.Debug, memberName, sourceFilePath, sourceLineNumber));
        }

        public static void Warning(string msg, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
       [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
       [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            LogActions.ForEach(a => a(msg.ToString(), JsonConvert.SerializeObject(msg), LogLevel.Warning, memberName, sourceFilePath, sourceLineNumber));
        }

        public static void Error(string msg, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
       [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
       [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            LogActions.ForEach(a => a(msg.ToString(), JsonConvert.SerializeObject(msg), LogLevel.Error, memberName, sourceFilePath, sourceLineNumber));
        }

        public static void Fatal(string msg, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
       [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
       [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            LogActions.ForEach(a => a(msg.ToString(), JsonConvert.SerializeObject(msg), LogLevel.Fatal, memberName, sourceFilePath, sourceLineNumber));
        }
    }
}
