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
using MongoDB.Bson;
using MongoDB.Driver;

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetListCollection("");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            Form2 form2add = new Form2();
            form2add.Show();
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

        private async void button4_Click(object sender, EventArgs e)
        {
            //dataGridInit();
            
            //var filter = Builders<DB.Item>.Filter.Regex("Name", richTextBox1.Text);
            GetListCollection(richTextBox1.Text);
        }

        private void btnDelItem_Click(object sender, EventArgs e)
        {
            var filter = Builders<DB.Item>.Filter.Regex("Name",richTextBox1.Text);
            DB.mongoCollection.DeleteOne(filter);
            GetListCollection("");
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            DB.Item Item = new DB.Item();

            Item.Name = textBox1.Text;
            Item.Model = textBox2.Text;
            Item.Item1 = textBox3.Text;

            //var doc = new BsonDocument { {"name", textBox1.Text}, {"id", textBox2.Text }, {"item1", textBox3.Text} };
            await DB.mongoCollection.InsertOneAsync(Item);
            GetListCollection("");
        }

        private void dataGridInit()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 3;

            dataGridView1.Columns[0].Name = "Name";
            dataGridView1.Columns[1].Name = "Model";
            dataGridView1.Columns[2].Name = "Other";
        }

        private async void GetListCollection(string Filter)
        {
            dataGridInit();
            var filter = Builders<DB.Item>.Filter.Regex("Name", Filter);

            using (var cursor = await DB.mongoCollection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var c = cursor.Current;
                    foreach (var listname in c)
                    {
                        dataGridView1.Rows.Add(listname.Name, listname.Model, listname.Item1);
                    }
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
    }
}
