using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace imglab
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        [Serializable]
        public delegate void MouseEventHandler(object sender, MouseEventArgs e);

        List<PictureItem> picList= new List<PictureItem>();
        int selected = -1;
        string URL;

        private void button1_Click(object sender, EventArgs e)
        {
            URL = textBox1.Text;

            System.IO.FileInfo[] files = null;
            try
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(URL);
                files = di.GetFiles("*.jpg", System.IO.SearchOption.TopDirectoryOnly);
                int pw = pictureBox1.Size.Width;
                int ph = pictureBox1.Size.Height;
                foreach (System.IO.FileInfo f in files)
                {
                    listBox1.Items.Add(f.Name.Split('.')[0]);
                    PictureItem p = new PictureItem(this);
                    try
                    {
                        p.original = Image.FromFile(URL + @"\\" + f.Name);
                    }
                    catch (Exception)
                    {
                        p.original = null;
                    }
                    p.picName = f.Name.Split('.')[0];
                    picList.Add(p);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("入力を見直してください\r\n " + err.StackTrace,
                "エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selecting = false;
            p1 = new Point();
            p2 = new Point();
            selected = picList.FindIndex(x => x.picName.Equals(listBox1.SelectedItem));
            pictureBox1.Image = picList[selected].original;
            picList[selected].setCorrection(picList[selected].original.Width, picList[selected].original.Height, pictureBox1.Width, pictureBox1.Height);
        }

        bool selecting = false;
        Point p1 = new Point();
        Point p2 = new Point();

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //みじっそう
        }


        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (selected != -1)
            {
                if (!selecting)
                {
                    p1 = new Point(e.Location.X, e.Location.Y);
                    selecting = true;
                }
                else
                {
                    p2 = new Point(e.Location.X, e.Location.Y);
                    picList[selected].set(p1.X, p1.Y, p2.X, p2.Y);
                    pictureBox1.Image = picList[selected].DrawRect();
                    selecting = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MakingFileClass m = new MakingFileClass();
            m.start(picList,pictureBox1);
        }
    }
}
