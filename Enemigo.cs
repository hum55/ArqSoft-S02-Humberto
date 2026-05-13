using System;

namespace Ahorcado
{
    public class Enemigo
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Vida { get; set; }

        public Enemigo(int x, int y, int vida)
        {
            X = x;
            Y = y;
            Vida = vida;
        }
    }
}