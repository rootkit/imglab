using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace imglab
{
    class PictureItem
    {
        public Image original;
        public string origURL;
        public string picName;
        public Form1 parent;

        public bool selected { get { return rectDatas.Count > 0; } }
        public Point locate = new Point();
        public int width;
        public int height;
        public double corr = 1.0;
        public Point emp;
        public List<RectData> rectDatas = new List<RectData>();

        public PictureItem(Form1 f)
        {
            parent = f;
        }

        private Bitmap canvas;
        private Graphics g;
        private Pen p= new Pen(Color.Red, 3);
        public Image DrawRect(int x,int y,int wei,int hei)
        {
            canvas = new Bitmap(original);
            g = Graphics.FromImage(canvas);

            //ここらへんのメモリリリリリーク修正
            foreach (RectData r in rectDatas)
            {
                    g.DrawRectangle(p, tC(r.x - emp.X / 2), tC(r.y - emp.Y / 2), tC(r.width), tC(r.height));
            }
            g.Dispose();
            return canvas;
        }
        private int tC(int num)
        {
            return (int)((double)num / corr);
        }

        public Image DrawRect()
        {
            return DrawRect(locate.X, locate.Y, width, height);
        }

        public void set(int x,int y,int x2,int y2){
            locate = new Point(x,y);
            width = x2 - x;
            height = y2 - y;
            //locate.X += emp.X;
           // locate.Y += emp.Y;

            var r = new RectData();
            r.setData(locate.X, locate.Y, width, height);

            bool xOUT1 = r.x <= emp.X / 2 || r.x >= PictureBoxData.W - emp.X / 2;
            bool xOUT2 = r.x + r.width <= emp.X / 2 || r.x + r.width >= PictureBoxData.W - emp.X / 2;
            bool yOUT1 = r.y <= emp.Y / 2 || r.y >= PictureBoxData.H - emp.Y / 2;
            bool yOUT2 = r.y + r.height <= emp.Y / 2 || r.y + r.height >= PictureBoxData.H - emp.Y / 2;
            bool isnotM = r.height < 0 || r.width < 0;
            bool isOutofPicture = xOUT1 || xOUT2 || yOUT1 || yOUT2 || isnotM;
            if (!isOutofPicture)
            {
                rectDatas.Add(r);
            }
            else
            {
                MessageBox.Show("画像の範囲外を選択している可能性が高いです。やり直してください",
                  "エラー",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
                
            }

        }


        public void setCorrection(int originalX, int originalY, int pictureboxX, int pictureboxY)
        {
            double Xcorr = (double)pictureboxX / (double)originalX;
            double Ycorr = (double)pictureboxY / (double)originalY;
            corr = Math.Min(Xcorr, Ycorr);

            int x = 0, y = 0;
            if (corr == Xcorr) y = pictureboxY - (int)((double)originalY * corr);
            else if (corr == Ycorr) x = pictureboxX - (int)((double)originalX * corr);
            emp =  new Point(x, y);
        }


        public string toXML()
        {           
            string retVal = "";
            retVal += @"<image file='" + picName + @".jpg'>" + Environment.NewLine;
            if (selected == true)
            {
                foreach (RectData r in rectDatas)
                {
                    retVal += @"<box top='" + r.x + "' left='" + r.y + "' width='" + r.width + "' height='" + r.height + "'/>" + Environment.NewLine;
                }
            }
            retVal += @"</image>";
            return retVal;
        }

        public void rotateOriginal(RotateFlipType rft){
            rectDatas.Clear();
            original.RotateFlip(rft);
            
        }
    }
}
