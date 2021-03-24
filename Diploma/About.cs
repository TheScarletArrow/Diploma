using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Diploma
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {

            label1.Text = "";
            label2.Text = "";
           // label5.Text = "";
            
            const string dir = @"F:\MySQL\MySQL Server 5.5\data\userdb";
           
           

           var files = Directory.EnumerateFiles(dir).Where(file=>file.EndsWith(".frm"));

          //label2.Text+=($"{"Filename",-18} {"Size (bytes)"}");
           foreach (var file in files)
           {
                   var info = new FileInfo(file);
                   var bytes = info.Length;
                   var kbytes = (float)bytes / 1024;
                   label1.Text += Path.GetFileNameWithoutExtension(file)+'\n';
                   label2.Text += $@"{kbytes:0.00}" + '\t'+@"Кб" + '\n';
                   //label5.Text += File.GetLastWriteTime(file).ToString()+'\n';

           }
           
        }
    }
}