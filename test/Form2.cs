using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
//using Word = Microsoft.Office.Interop.Word;
using MongoDB.Bson;
using MongoDB.Driver;
using iTextSharp.text;
using iTextSharp.text.pdf;
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

        private async void btnOk_Click(object sender, EventArgs e)
        {
            DB.Item item = new DB.Item();
            var filter = Builders<DB.Item>.Filter.Eq("","");

            switch (Form1.keyMode)
            {
                case "add":
                    push_item(item);
                    await DB.mongoCollection.InsertOneAsync(item);
                    print_pdf();
                    break;
                case "edit":
                    push_item(item);
                    item.Id = Form1.curItem.Id;
                    filter = Builders<DB.Item>.Filter.Eq("_id", ObjectId.Parse(Form1.curItem.Id.ToString()));
                    await DB.mongoCollection.ReplaceOneAsync(filter, item);
                    break;
                case "done":
                    push_item(item);
                    item.Status = 1;
                    item.Id = Form1.curItem.Id;
                    filter = Builders<DB.Item>.Filter.Eq("_id", ObjectId.Parse(Form1.curItem.Id.ToString()));
                    await DB.mongoCollection.ReplaceOneAsync(filter, item);
                    break;
                case "issue":
                    push_item(item);
                    item.Status = 2;
                    item.Id = Form1.curItem.Id;
                    filter = Builders<DB.Item>.Filter.Eq("_id", ObjectId.Parse(Form1.curItem.Id.ToString()));
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
            textBoxPrice.Text = item.Price;
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
            item.Price = textBoxPrice.Text;
        }

        private void print_pdf()
        {
            try
            {
                Document myDocument = new Document(PageSize.A4);
                PdfWriter.GetInstance(myDocument, new FileStream("1.pdf", FileMode.Create));
                myDocument.Open();

                string fg = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.TTF");
                BaseFont bfont = BaseFont.CreateFont(fg, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new Font(bfont, 12);

                myDocument.Add(new Paragraph("--------------------------Reports--------------------------", font));
                myDocument.Add(new Paragraph("Имя: " + textBoxFIO.Text, font));
                myDocument.Add(new Paragraph("Телефон: " + textBoxTel.Text, font));
                myDocument.Add(new Paragraph("Адрес: " + textBoxAdr.Text, font));
                myDocument.Add(new Paragraph("Фирма: " + textBoxBrand.Text, font));
                myDocument.Add(new Paragraph("Модель: " + textBoxModel.Text, font));

                myDocument.Close();
                System.Diagnostics.Process.Start("1.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex);
            }
        }
    }
}
