using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imglab
{
    class RectData
    {
        public int x;
        public int y;
        public int width;
        public int height;

        public int[] getData() { return new int[] { x, y, width, height }; }
        public void setData(int x,int y,int width,int height){
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public override string ToString()
        {
            return "Location("+ x +"," + y + ") Size(" + width + "," + height +")" ;
        }
    }
}
