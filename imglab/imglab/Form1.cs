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
            getFiles();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(pictureBox1.Image != null)pictureBox1.Image.Dispose();
            //if (selected != -1) picList[selected].original.Dispose();
            selecting = false;
            p1 = new Point();
            p2 = new Point();
            selected = picList.FindIndex(x => x.picName.Equals(listBox1.SelectedItem));
            //picList[selected].original = Image.FromFile(picList[selected].origURL);
            //pictureBox1.Image = picList[selected].original;
            picList[selected].setCorrection(picList[selected].original.Width, picList[selected].original.Height, pictureBox1.Width, pictureBox1.Height);
            showRects();
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
                   // pictureBox1.Image = picList[selected].DrawRect();
                    selecting = false;
                }
            }
            showRects();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MakingFileClass m = new MakingFileClass();
            m.start(picList);
        }

        private void showRects(){
            listBox2.Items.Clear();
            for (int i = 0; i < picList[selected].rectDatas.Count; i++)
            {
                listBox2.Items.Add(picList[selected].rectDatas[i].ToString());
            }
            pictureBox1.Image = picList[selected].DrawRect();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                for (int i = 0; i < picList[selected].rectDatas.Count; i++)
                {
                    if (picList[selected].rectDatas[i].ToString().Equals(listBox2.SelectedItem.ToString()))
                    {
                        picList[selected].rectDatas.Remove(picList[selected].rectDatas[i]);
                        break;
                    }
                }
            }
            showRects();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            setRotate(RotateFlipType.Rotate180FlipY);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            setRotate(RotateFlipType.Rotate180FlipX);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            setRotate(RotateFlipType.Rotate90FlipXY);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            setRotate(RotateFlipType.Rotate270FlipXY);
        }

        private void setRotate(RotateFlipType r)
        {
            picList[selected].rotateOriginal(r);
            picList[selected].setCorrection(picList[selected].original.Width, picList[selected].original.Height, pictureBox1.Width, pictureBox1.Height);
            selecting = false;
            showRects();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (e.KeyData == Keys.Down)
                {
                    if (listBox1.SelectedIndex == 0)
                    {
                        listBox1.SetSelected(listBox1.Items.Count-1,true);
                    }
                    else
                    {
                        listBox1.SetSelected(listBox1.SelectedIndex - 1, true);
                    }
                }
                else if (e.KeyData == Keys.Up)
                {
                    if (listBox1.SelectedIndex == listBox1.Items.Count - 1)
                    {
                        listBox1.SetSelected(0, true);
                    }
                    else
                    {
                        listBox1.SetSelected(listBox1.SelectedIndex + 1, true);
                    }
                }

            }
        }

        private void getFiles()
        {
            string[] extent = new string[] { "*.jpg","*.png","*.jpg" }; 
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(URL);
            try
            {
                for (int i = 0; i < extent.Length; i++)
                {
                    files = di.GetFiles(extent[i], System.IO.SearchOption.TopDirectoryOnly);
                    int pw = pictureBox1.Size.Width;
                    int ph = pictureBox1.Size.Height;
                    foreach (System.IO.FileInfo f in files)
                    {
                        listBox1.Items.Add(f.Name.Split('.')[0]);
                        PictureItem p = new PictureItem(this);
                        try
                        {
                            p.original = Image.FromFile(URL + @"\\" + f.Name);
                            p.origURL = URL + @"\\" + f.Name;
                        }
                        catch (Exception)
                        {
                            p.original = null;
                        }
                        p.picName = f.Name.Split('.')[0];
                        picList.Add(p);
                    }
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
    }
}
