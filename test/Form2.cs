﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
using Word = Microsoft.Office.Interop.Word;
using System.Windows.Xps.Packaging;

namespace test
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            var filter = Builders<DB.Item>.Filter.Regex("Name", "");
            long count = await DB.mongoCollection.CountAsync(filter);
            textBox1.Text = count.ToString();
            */
            var aWord = new Word.Application();
            Word.Document doc = aWord.Documents.Add(Environment.CurrentDirectory.ToString()+"\\Doc1.dotx");
            doc.Bookmarks["Name"].Range.Text = textBox1.Text;
            doc.SaveAs2("1.xps", Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatXPS);
            System.Diagnostics.Process.Start("1.xps");
            System.Diagnostics.Process.Start("1.xps");
        }
    }
}
