using System;

namespace Ahorcado
{
    public class Disparo
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int DX { get; set; }
        public int DY { get; set; }

        public Disparo(int x, int y, int dx, int dy)
        {
            X = x;
            Y = y;
            DX = dx;
            DY = dy;
        }
    }
}