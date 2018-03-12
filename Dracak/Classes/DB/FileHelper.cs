using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracak.Classes.DB
{
    class FileHelper
    {
        public bool DeleteDB()
        {
            if (File.Exists(GetDBfilePath()))
            {
                try
                {
                    File.Delete(GetDBfilePath());
                }
                catch
                {
                    App.PlayerViewModel.DBhelper.Connect();
                    App.LocationViewModel.DBhelper.Connect();
                    return false;
                }
            }
            return true;
        }
        public string GetDBfilePath()
        {
            string rootPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string rootDetails = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            string[] rootName = rootDetails.Split(',');

            string TruePath = rootPath.Substring(0, rootPath.IndexOf(rootName[0] + "."));
            string[] DBFile = Directory.GetFiles(TruePath, "database.db");

            if (DBFile.Length > 0)
            {
                if (File.Exists(DBFile[0]))
                    return DBFile[0];
                else
                    return null;
            }
            else 
                return null;
        }
    }
}
