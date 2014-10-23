using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace Test
{
    public partial class CompositingTool : Form
    {
        public CompositingTool()
        {
            InitializeComponent();
        }

        private void folderPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.label1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                this.label2.Text = folderBrowserDialog2.SelectedPath;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            {
                string StartDirectory = this.label1.Text;
                string EndDirectory = this.label2.Text;

                foreach (string filename in Directory.EnumerateFiles(StartDirectory))
                {
                    using (FileStream SourceStream = File.Open(filename, FileMode.Open))
                    {
                        using (FileStream DestinationStream = File.Create(EndDirectory + filename.Substring(filename.LastIndexOf('\\'))))
                        {
                            await SourceStream.CopyToAsync(DestinationStream);
                        }
                    }
                }
                Directory.Delete(StartDirectory, true);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string EndDirectory = this.label2.Text;
            CreateXML(null);

        }

        public static XElement CreateXML(string[] args)
        {
            XmlWriter xmlWriter = XmlWriter.Create("C:/Users/windows/Desktop/2ndDir/test.xml");

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("users");

            xmlWriter.WriteStartElement("user");
            xmlWriter.WriteAttributeString("age", "42");
            xmlWriter.WriteString("John Doe");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("user");
            xmlWriter.WriteAttributeString("age", "39");
            xmlWriter.WriteString("Jane Doe");

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return null;
        }
    }
}
