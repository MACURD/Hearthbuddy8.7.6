using log4net;
using Triton.Common.LogUtilities;

namespace HREngine.Bots
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///     The helpfunctions.
    /// </summary>

    public class Helpfunctions
    {
        /// <summary>The logger for this type.</summary>
        //private static readonly ILog Log = Logger.GetLoggerInstanceForType(); // Todo:  为了能够run Test不用Triton里面组件
        public List<Playfield> storedBoards = new List<Playfield>();


        public static List<T> TakeList<T>(IEnumerable<T> source, int limit)
        {
            List<T> retlist = new List<T>();
            int i = 0;

            foreach (T item in source)
            {
                retlist.Add(item);
                i++;

                if (i >= limit) break;
            }
            return retlist;
        }


        public bool runningbot = false;

        private static Helpfunctions instance;

        public static Helpfunctions Instance
        {
            get { return instance ?? (instance = new Helpfunctions()); }
        }

        private Helpfunctions()
        {

            //System.IO.File.WriteAllText(Settings.Instance.logpath + Settings.Instance.logfile, "");
        }

        public bool writelogg = true;

        public void loggonoff(bool onoff)
        {
            //writelogg = onoff;
        }

        public void createNewLoggfile()
        {
            //System.IO.File.WriteAllText(Settings.Instance.logpath + Settings.Instance.logfile, "");
        }

        public void logg(string s)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(Settings.Instance.logpath + Settings.Instance.logfile))
                {
                    sw.WriteLine(s);
                }
                if (!writelogg) return;
                // 打印结果设置
                if (printUtils.detailFilePath != null)
                {
                    using (StreamWriter sw = File.AppendText(printUtils.detailFilePath))
                    {
                        sw.WriteLine(s);
                    }
                }
                // 打印结果设置
                if (printUtils.printResult && printUtils.outputFilePath != null)
                {
                    using (StreamWriter sw1 = File.AppendText(printUtils.outputFilePath)) 
                    {
                        sw1.WriteLine(s);
                    }
                }
                // 打印对局日志
                if (printUtils.printRecord && printUtils.recordPath != null && printUtils.recordRounds != null)
                {
                    if (Directory.Exists(printUtils.recordPath) == false)
                    {
                        Directory.CreateDirectory(printUtils.recordPath);
                    }
                    using (StreamWriter sw2 = File.AppendText(printUtils.recordPath + printUtils.recordRounds))
                    {
                        sw2.WriteLine(s);
                    }
                }

            }
            catch
            {
            }
            //Console.WriteLine(s);
        }

        public DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public void ErrorLog(string s)
        {
            if (!writelogg) return;
            // Helpfunctions.instance.logg(s);
            try
            {
                using (StreamWriter sw = File.AppendText(@".\myLog.txt"))
                {
                    sw.WriteLine(s);
                }
            }
            catch
            {
            }
        }

        private string sendbuffer = "";

        public void resetBuffer()
        {
            this.sendbuffer = "";
        }

        public void writeToBuffer(string data)
        {
            this.sendbuffer += "\r\n" + data;
        }

        public void writeBufferToFile()
        {
            bool writed = true;
            this.sendbuffer += "<EoF>";
            while (writed)
            {
                try
                {
                    System.IO.File.WriteAllText(Settings.Instance.path + "crrntbrd.txt", this.sendbuffer);
                    writed = false;
                }
                catch
                {
                    writed = true;
                }
            }
            this.sendbuffer = "";
        }

        public void writeBufferToActionFile()
        {
            bool writed = true;
            this.sendbuffer += "<EoF>";
            this.ErrorLog("write to action file: " + sendbuffer);
            while (writed)
            {
                try
                {
                    System.IO.File.WriteAllText(Settings.Instance.path + "actionstodo.txt", this.sendbuffer);
                    writed = false;
                }
                catch
                {
                    writed = true;
                }
            }
            this.sendbuffer = "";
        }



    }
}
