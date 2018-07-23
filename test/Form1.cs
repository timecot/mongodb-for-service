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
        public static DB.Item curItem { get; set; }

        public Form1()
        {
            Ref = this;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetListCollection("");
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

        private async void edit_btn_Click(object sender, EventArgs e)
        {
            var filter = Builders<DB.Item>.Filter.Eq("_id", ObjectId.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString()));

            using (var cursor = await DB.mongoCollection.FindAsync(filter))
            {
                curItem = await cursor.FirstOrDefaultAsync();
                Form2 form2add = new Form2();
                form2add.ShowDialog();
                curItem = null;
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var filter = Builders<DB.Item>.Filter.Eq("_id", ObjectId.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString()));
            try
            {
                await DB.mongoCollection.DeleteOneAsync(filter);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            GetListCollection("");
        }

        public void dataGridInit()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 8;

            dataGridView1.Columns[0].Name = "ObjectID";
            dataGridView1.Columns[0].Visible = false;

            dataGridView1.Columns[1].Name = "Data";
            dataGridView1.Columns[2].Name = "Name";
            dataGridView1.Columns[3].Name = "Telephone";
            dataGridView1.Columns[4].Name = "Address";
            dataGridView1.Columns[5].Name = "Imei";
            dataGridView1.Columns[6].Name = "Brand";
            dataGridView1.Columns[7].Name = "Model";
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
                            dataGridView1.Rows.Add(listname.Id, listname.Data.ToShortDateString(), listname.Name, listname.Tel, listname.Adr, listname.Imei, listname.Brand, listname.Model);
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
            catch (Exception ex)
            {
                MessageBox.Show("Error: Not Conection To Server" + ex);
            }

        }

        private async void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            DB.Item item = new DB.Item();
            var filter = Builders<DB.Item>.Filter.Eq("_id", ObjectId.Parse(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString()));

            using (var cursor = await DB.mongoCollection.FindAsync(filter))
            {
                item = await cursor.FirstOrDefaultAsync();
                if (item != null)
                {
                    richTextBox3.Text = item.Name;
                    richTextBox4.Text = item.Imei;
                }
            }
        }
    }       
}
