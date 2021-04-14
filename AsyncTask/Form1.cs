using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;


namespace AsyncTask
{
    public partial class Form1 : Form
    {
       
        private  string path { get; set; }
        private ConcurrentDictionary<string, List<string>> directoryCache;
        CacheDictionary cacheDictionary;
        bool _restart = false;

        public Form1()
        {
            InitializeComponent();        
        }

        private void Form1_Load(object sender, EventArgs e)
        {          

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                path = FBD.SelectedPath;
                cacheDictionary = new CacheDictionary(path);
                directoryCache = cacheDictionary.GetDirectoryCache(path);
                textBox4.Text = FBD.SelectedPath;  
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(directoryCache);
            backgroundWorker2.RunWorkerAsync(directoryCache);
            _restart = true;
        }

        private void BackgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {          
            e.Result = cacheDictionary.GetFilies(directoryCache, "*.xml");
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (var i in ((List<string>)e.Result))
            {
                textBox1.Text += i + Environment.NewLine;
            }
            System.Threading.Thread.Sleep(500);
            if (_restart)
            {
                backgroundWorker1.RunWorkerAsync(directoryCache);
                _restart = false;
            }
        }
        private void BackgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            e.Result = cacheDictionary.GetFilies(directoryCache, "*.dll");          
           
        }
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (var i in ((List<string>)e.Result))
            {
                textBox3.Text += i + Environment.NewLine;
            }
            System.Threading.Thread.Sleep(500);
            if (_restart)
            {
                backgroundWorker2.RunWorkerAsync(directoryCache);
                _restart = false;
            }
        }

       
    }
}