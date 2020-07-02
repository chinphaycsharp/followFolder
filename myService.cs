using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace followFolder
{
    public partial class myService : ServiceBase
    {
        public DataSet dt = new DataSet();
        static string file_Origin;
        static string file_Desstination;

        public myService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            using (StreamReader r = new StreamReader(@"D:\mySQL\config.json"))
            {
                string json = r.ReadToEnd();
                List<Read> reads = JsonConvert.DeserializeObject<List<Read>>(json);
                foreach (var item in reads)
                {
                    file_Origin = item.file_Origin;
                    file_Desstination = item.file_Desstination;
                }
            }

            if (file_Origin == "" || file_Desstination == "")
            {
                writeLog.write("Error: file configure.json", "", "", DateTime.Now);
                return;
            }

            FileSystemWatcher watcher = new FileSystemWatcher(file_Desstination);
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = true;

            //xu ly su thay doi cua file
            watcher.Created += watcher_Created;

            copyFolder(file_Origin, file_Desstination);
            Console.Read();
        }

        protected override void OnStop()
        {
        }

        public static void RunCmd(myService obj)
        {
            obj.OnStart(null);
            obj.OnStop();
        }

        public static void copyFolder(string sourceDirectory, string targetDirectory)
        {
            var fileOrigin = new DirectoryInfo(sourceDirectory);
            var fileDestiation = new DirectoryInfo(targetDirectory);
            CopyAll(fileOrigin, fileDestiation);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }


        private static void watcher_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(e.FullPath);
            writeLog.write("Bạn vừa tạo file : ", e.Name, null, DateTime.Now);
        }
    }

    public class writeLog
    {
        public static void write(string text, string name, string oldname, DateTime date)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "log.txt";

            if (System.IO.File.Exists(path))
            {
                Console.WriteLine(text + " " + name + " " + oldname + " " + DateTime.Now.ToString() + Environment.NewLine);
                System.IO.File.AppendAllText(path, text + " " + name + " " + oldname + " " + DateTime.Now.ToString() + Environment.NewLine);
            }
        }
    }

    public class Read
    {
        public string file_Origin;
        public string file_Desstination;
    }
}

