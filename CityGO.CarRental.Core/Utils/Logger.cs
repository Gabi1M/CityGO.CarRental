using System;
using System.IO;
using System.Threading;

namespace CityGO.CarRental.Core.Utils
{
    public static class Logger
    {
        private static StreamWriter file;

        //===========================================================//
        public static void Log(string msg, LogType type)
        {
            OpenLogFile(3);
            var message = string.Empty;
            switch (type)
            {
                case LogType.Error:
                    {
                        message += "( E ) ";
                        break;
                    }
                case LogType.Warning:
                    {
                        message += "( W ) ";
                        break;
                    }
                case LogType.Info:
                    {
                        message += "( I ) ";
                        break;
                    }
            }

            message += DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": " + msg;
            file.WriteLine(message);
            file.Flush();
            CloseLogFile();
        }

        //===========================================================//
        public static void LogException(Exception ex, LogType type = LogType.Error)
        {
            OpenLogFile(3);
            var message = string.Empty; switch (type)
            {
                case LogType.Error:
                {
                    message += "( E ) ";
                    break;
                }
                case LogType.Warning:
                {
                    message += "( W ) ";
                    break;
                }
                case LogType.Info:
                {
                    message += "( I ) ";
                    break;
                }
            }

            message += DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": " + GetFullExceptionMessage(ex) + "\n" + ex.Source + "\n" + ex.StackTrace;
            file.WriteLine(message);
            file.Flush();
            CloseLogFile();
        }

        //===========================================================//
        private static void OpenLogFile(int numberOfTries)
        {
            for (var i = 0; i < numberOfTries; i++)
            {
                try
                {
                    const string filepath = "Log.log";
                    if (!File.Exists(filepath))
                    {
                        File.Create(filepath).Close();
                    }
                    file = new StreamWriter(filepath, true);
                    break;
                }
                catch (Exception)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        //===========================================================//
        private static void CloseLogFile()
        {
            file.Close();
        }

        //===========================================================//
        private static string GetFullExceptionMessage(Exception ex)
        {
            var message = ex.Message + "\n";
            if (ex.InnerException != null)
            {
                message += GetFullExceptionMessage(ex.InnerException);
            }

            return message.Trim('\n');
        }
    }

    //===========================================================//
    public enum LogType
    {
        Error,
        Warning,
        Info
    }
}
