using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using System.Drawing.Drawing2D;

namespace imglab
{
    class MakingFileClass
    {
        public string FILENAME;
        public string URL;
        public string fileLocate = "";


        public void start(List<PictureItem> pL,PictureBox pB)
        {
            openSaveFileDialog();
            extendXML(pL);
            saveToJPG(pL,pB);

        }

        private void openSaveFileDialog()
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.FileName = "hogehoge";
            sfd.InitialDirectory = @"C:\";
            sfd.Filter = "XMLファイル(*.xml)|*.xml|すべてのファイル(*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.Title = "保存先のファイルを選択してください";
            sfd.RestoreDirectory = true;
            //ダイアログを表示する
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                URL = sfd.FileName;
                FILENAME = Path.GetFileName(sfd.FileName);
                fileLocate = sfd.FileName.TrimEnd(FILENAME.ToCharArray());
                try
                {
                    FileSystem.DeleteFile(URL);
                }
                catch (Exception e)
                {

                }
            }
        }

        public void extendXML(List<PictureItem> picList)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(URL, true,
            System.Text.Encoding.GetEncoding("Shift_Jis"));

            sw.WriteLine(@"<?xml version='1.0' encoding='ISO-8859-1'?>");
            sw.WriteLine(@"<?xml-stylesheet type='text/xsl' href='image_metadata_stylesheet.xsl'?>");
            sw.WriteLine(@"<dataset>");
            sw.WriteLine(@"<name>imglab dataset</name>");
            sw.WriteLine(@"<comment>Created by OWN imglab tool.</comment>");
            sw.WriteLine(@"<images>");
            for (int i = 0; i < picList.Count; i++)
            {
                sw.WriteLine(picList[i].toXML());
            }
            sw.WriteLine(@"</images>");
            sw.WriteLine(@"</dataset>");

            sw.Close();
        }

        public void createDir(string name)
        {
            //みじっそう たぶん使わない
        }

        public void saveToJPG(List<PictureItem> picList, PictureBox p)
        {
            Bitmap bmp;
            Bitmap BLUEBmp = new Bitmap(PictureBoxData.W, PictureBoxData.H);
            Graphics g = Graphics.FromImage(BLUEBmp);
            Rectangle rect1 = new Rectangle(0, 0, PictureBoxData.W, PictureBoxData.H);

            Rectangle rect2;
            foreach (var l in picList)
            {
                if (l.selected)
                {
                    g.FillRectangle(Brushes.Blue, rect1);
                    bmp = new Bitmap(l.original);
                    g.DrawImage(bmp, l.emp.X/2, l.emp.Y/2, (int)((double)l.original.Width*l.corr), (int)((double)l.original.Height*l.corr));

                    BLUEBmp.Save(fileLocate + "\\" + l.picName + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }
    }
}
