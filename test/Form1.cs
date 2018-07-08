using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
//using System.Windows.Controls;
using MongoDB.Bson;
using MongoDB.Driver;

namespace test
{
    public partial class Form1 : Form
    {
        public static Form1 Ref { get; set; }
        public Form1()
        {
            Ref = this;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetListCollection("");
            //DB.mongoDatabase.CreateCollectionAsync("info");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2add = new Form2();
            //form2add.Show();
            form2add.ShowDialog();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            GetListCollection(richTextBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DB.mongoDatabase.CreateCollectionAsync(richTextBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DB.mongoDatabase.DropCollectionAsync(richTextBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //dataGridInit();

            //var filter = Builders<DB.Item>.Filter.Regex("Name", richTextBox1.Text);
            GetListCollection(richTextBox1.Text);
        }

        private void btnDelItem_Click(object sender, EventArgs e)
        {
            var filter = Builders<DB.Item>.Filter.Regex("Name", richTextBox1.Text);
            DB.mongoCollection.DeleteOne(filter);
            GetListCollection("");
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            DB.Item Item = new DB.Item();

            Item.Name = textBox1.Text;
            Item.Model = textBox2.Text;
            Item.Brand = textBox3.Text;

            //var doc = new BsonDocument { {"name", textBox1.Text}, {"id", textBox2.Text }, {"item1", textBox3.Text} };
            await DB.mongoCollection.InsertOneAsync(Item);
            GetListCollection("");
        }

        public void dataGridInit()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 7;

            dataGridView1.Columns[0].Name = "Data";
            dataGridView1.Columns[1].Name = "Name";
            dataGridView1.Columns[2].Name = "Telephone";
            dataGridView1.Columns[3].Name = "Address";
            dataGridView1.Columns[4].Name = "Imei";
            dataGridView1.Columns[5].Name = "Brand";
            dataGridView1.Columns[6].Name = "Model";
        }

        public async void GetListCollection(string nameFilter)
        {
            try
            {
                dataGridInit();
                var filter = Builders<DB.Item>.Filter.Regex("Name", nameFilter);
                using (var cursor = await DB.mongoCollection.FindAsync(filter))
                {
                    while (await cursor.MoveNextAsync())
                    {
                        var c = cursor.Current;
                        foreach (var listname in c)
                        {
                            dataGridView1.Rows.Add(listname.Data.ToShortDateString(), listname.Name, listname.Tel, listname.Adr, listname.Imei, listname.Brand, listname.Model);
                            //MessageBox.Show(listname.Foto.Length.ToString());
                            /*
                            if (listname.Foto.Length>1)
                            {
                                System.IO.File.WriteAllBytes("temp", listname.Foto);
                                using (var tmp=new Bitmap("temp"))
                                {
                                    pictureBox1.Image = new Bitmap(tmp);
                                }
                            }
                            */
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: Not Conection To Server"+ex);
            }
            
        }
    }       
}
