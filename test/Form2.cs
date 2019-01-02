using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Word = Microsoft.Office.Interop.Word;
using MongoDB.Bson;
using MongoDB.Driver;
//using System.Windows.Xps.Packaging;

namespace test
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            if (Form1.curItem != null) { pull_item(Form1.curItem); };

            switch(Form1.keyMode)
            {
                case "add":
                    break;
                case "edit":
                    break;
                default:
                    break;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            DB.Item item = new DB.Item();
            var filter = Builders<DB.Item>.Filter.Eq("_id", ObjectId.Parse(Form1.curItem.Id.ToString()));

            switch (Form1.keyMode)
            {
                case "add":
                    push_item(item);
                    await DB.mongoCollection.InsertOneAsync(item);
                    break;
                case "edit":
                    push_item(item);
                    item.Id = Form1.curItem.Id;
                    await DB.mongoCollection.ReplaceOneAsync(filter, item);
                    break;
                case "done":
                    push_item(item);
                    item.Status = 1;
                    item.Id = Form1.curItem.Id;
                    await DB.mongoCollection.ReplaceOneAsync(filter, item);
                    break;
            }
            
            Close();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Form1.Ref != null) { Form1.Ref.GetListCollection(""); };
        }

        private void pull_item(DB.Item item)
        {
            dateTimePicker1.Value = item.Data;
            switch (item.Status)
            {
                case 0:
                    checkedListBox1.SetItemChecked(0, true);
                    break;
                case 1:
                    checkedListBox1.SetItemChecked(1, true);
                    break;
                case 2:
                    checkedListBox1.SetItemChecked(2, true);
                    break;
            }

            textBoxFIO.Text = item.Name;
            textBoxTel.Text = item.Tel;
            textBoxAdr.Text = item.Adr;
            textBoxImei.Text = item.Imei;
            textBoxBrand.Text = item.Brand;
            textBoxModel.Text = item.Model;
            richTextBox1.Text = item.Description;
        }

        private void push_item(DB.Item item)
        {
            item.Data = dateTimePicker1.Value.ToLocalTime();
            //item.Status = 0;
            item.Name = textBoxFIO.Text;
            item.Tel = textBoxTel.Text;
            item.Adr = textBoxAdr.Text;
            item.Imei = textBoxImei.Text;
            item.Brand = textBoxBrand.Text;
            item.Model = textBoxModel.Text;
            item.Description = richTextBox1.Text;
        }

        private void print_xps()
        {
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

            System.Diagnostics.Process.Start("1.xps");
        }
    }
}
