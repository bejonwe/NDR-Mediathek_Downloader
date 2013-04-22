using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace NDR_Mediathek_Downloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = System.Windows.Forms.Clipboard.GetText();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Catching URL...";
            button1.Enabled = false;
            WebClient source = new WebClient();
            string downurl = "";
            string filename = "";
            try
            {
                string sourceCode = source.DownloadString(textBox1.Text);
                string[] lines = sourceCode.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("video/mp4"))
                    {
                        string urlline = lines[i];
                        string[] linesplit = urlline.Split('\'');
                        textBox2.Text = linesplit[1];
                        downurl = linesplit[1];
                    }
                    else if (lines[i].Contains("<title>"))
                    {
                        string line = lines[i];
                        string[] tmp1 = line.Split('>');
                        string[] tmp2 = tmp1[1].Split('|');
                        string tmp3 = tmp2[0].Substring(0, tmp2[0].Length - 1);
                        filename = tmp3.Replace("\"","") + ".mp4";
                        filename = filename.Replace(":", " -");
                        filename = filename.Replace("Ã¼", "ü");
                        filename = filename.Replace("Ã¶", "ö");
                        filename = filename.Replace("Ã¤", "ä");
                        filename = filename.Replace("Ã", "Ü");
                        filename = filename.Replace("Ã", "Ö");
                        filename = filename.Replace("Ã", "Ä");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Fehler in der URL!");
            }

            saveFileDialog1.Filter = "Mpeg 4 Video |*.mp4";
            saveFileDialog1.Title = "Wo soll das Video gespeichert werden?";
            saveFileDialog1.FileName = filename;
            saveFileDialog1.ShowDialog();

            textBox2.Text = "Downloading! - " + textBox2.Text;

            if (saveFileDialog1.FileName != "")
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(new Uri(downurl), @saveFileDialog1.FileName);
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            button1.Enabled = true;
            progressBar1.Value = 0;
            textBox1.Text = "";
            textBox2.Text = "Download completed! - " + textBox2.Text;
        }
    }
}
