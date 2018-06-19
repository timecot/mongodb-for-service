using System;
using System.Windows.Forms;
//using MongoDB.Bson;
//using MongoDB.Driver;
using Word = Microsoft.Office.Interop.Word;
//using System.Windows.Xps.Packaging;
//using System.Windows.Documents;

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
            var aWord = new Word.Application();
            Word.Document doc = aWord.Documents.Add(Environment.CurrentDirectory.ToString()+"\\Doc1.dotx");
            doc.Bookmarks["Name"].Range.Text = textBox1.Text;
            doc.SaveAs2(Environment.CurrentDirectory.ToString()+"\\1.xps", Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatXPS);
            System.Diagnostics.Process.Start("1.xps");
        }
    }
}
