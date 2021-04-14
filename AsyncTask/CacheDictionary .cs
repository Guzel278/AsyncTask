using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace AsyncTask
{
    class CacheDictionary
    {
        object locker = new object();
        private  string _path;
        DateTime _time;

        public ConcurrentDictionary<string, List<string>> directoryCache { get; set; }
        public CacheDictionary(string path)
        {
            _path = path;
            directoryCache = GetDirectoryCache(_path);
            _time = DateTime.Now;
        }     
        public  ConcurrentDictionary<string, List<string>> GetDirectoryCache(string _path)
        {
            lock (locker)
            {
                ConcurrentDictionary<string, List<string>> fileCache = new ConcurrentDictionary<string, List<string>>();
                string[] directory = Directory.GetDirectories(_path);
                foreach (string d in directory)
                {
                    string[] files = Directory.GetFiles(d);
                    List<string> files1 = Directory.GetFiles(d).ToList();
                    var listFiles = new List<string>();
                    foreach (string f in files)
                    {
                        listFiles.Add(f);
                    }
                    fileCache.TryAdd(d, listFiles);
                }
                _time = DateTime.Now;
                return fileCache;
            }
        }
       
        public List<string> GetFilies(ConcurrentDictionary<string, List<string>> directoryCache, string fileExtension)
        {
            lock (locker)
            {
                if ((_time - DateTime.Now).TotalMinutes < 1)
                {
                    List<string> listFilies = new List<string>();
                    foreach (string p in directoryCache.Keys)
                    {
                        foreach (string f in Directory.EnumerateFiles(p, fileExtension, SearchOption.AllDirectories))
                        {
                            listFilies.Add(f);
                        }
                    }
                    return listFilies;
                }
                else
                {
                    GetDirectoryCache(_path);
                    return null;
                }
            }
        }
    }
}
