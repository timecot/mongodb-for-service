using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using MongoDB.Bson;
//using MongoDB.Driver;
using Word = Microsoft.Office.Interop.Word;
//using System.Windows.Xps.Packaging;

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
            DB.Item item = new DB.Item();

            try
            {
                var aWord = new Word.Application();
                Word.Document doc = aWord.Documents.Add(Environment.CurrentDirectory.ToString() + "\\Doc1.dotx");

                doc.Bookmarks["Name"].Range.Text = textBoxFIO.Text;
                doc.Bookmarks["Tel"].Range.Text = textBoxTel.Text;
                doc.Bookmarks["Adr"].Range.Text = textBoxAdr.Text;
                doc.Bookmarks["IMEI"].Range.Text = textBoxImei.Text;
                doc.Bookmarks["Brand"].Range.Text = textBoxBrand.Text;
                doc.Bookmarks["Model"].Range.Text = textBoxModel.Text;
                doc.SaveAs2(Environment.CurrentDirectory + "\\1.xps", Word.WdSaveFormat.wdFormatXPS);
                aWord.Quit(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex);
            }

            item.Data = dateTimePicker1.Value;
            item.Name = textBoxFIO.Text;
            item.Tel = textBoxTel.Text;
            item.Adr = textBoxAdr.Text;
            item.Imei = textBoxImei.Text;
            item.Brand = textBoxBrand.Text;
            item.Model = textBoxModel.Text;

            DB.mongoCollection.InsertOneAsync(item);

            System.Diagnostics.Process.Start("1.xps");

        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Form1.Ref != null) { Form1.Ref.GetListCollection(""); };
        }
    }
}
