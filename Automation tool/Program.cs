using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Xaml.Media;


namespace Automation_tool
{
    internal class Program
    {
        static void Main(string[] args)
        {
         //get path of temp folder for user
         var temppath=Path.GetTempPath();
            //intialize size of deleted files
            long sizedeleted = 0;
            if (Directory.Exists(temppath))
            {
                //get all files in temp folder and directories
                var files = Directory.GetFiles(temppath);
                var dirs = Directory.GetDirectories(temppath);
                //accumulate size of deleted files
                foreach (var file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    sizedeleted += fi.Length;
                }
                //delete files in temp
                foreach (var dir in dirs)
                    try
                    {
                        Directory.Delete(dir, true);
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine($" Temp Directory {dir} can not delete now:{ex.Message} ");
                    }
                foreach (var file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine($"Temp file {file} can not delete now :{ex.Message}");
                    }
                }
                //get freespace in Drive (c)
                DriveInfo dinfo = new DriveInfo("C");
                long freespace = dinfo.AvailableFreeSpace;
                //create notification and show using UMP
                var text = $"Size of Deleted files Equals : {formatsize(sizedeleted)}";//text
                ToastContentBuilder notify = new ToastContentBuilder();
                notify.AddText("Temp Cleaner");//title
                //imagepath to show it at any Device and at tasksceduler
                var imagepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,".\\KAITECH.JFIF");
                notify.AddInlineImage(new Uri(Path.GetFullPath(imagepath)));
                notify.AddText(text);
                notify.AddText($" Freespace Drive [C] equals {formatsize(freespace)}");
                notify.Show();

            }
            else { Console.WriteLine("Temp folder does not exist"); }
            


        }
        // format size 
        #region function
        static string formatsize(long bytes)
        {
            const int kb = 1024;
            const int mb = 1024 * kb;
            const int gb = 1024 * mb;
            if (bytes >= gb)
            {
                return $"{bytes / (double)gb:.0##} GB";
            }
            if (bytes >= mb)
            {
                return $" {bytes / (double)mb:.0##} MB";
            }
            if (bytes >= kb)
            {
                return $"{bytes / (double)kb:.0##} KB";
            }
            return $" {bytes} bytes";
        }
        #endregion

    }
}
