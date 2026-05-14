using System;
using System.Collections.Generic;

namespace Ahorcado
{
    /// <summary>
    /// Representa el mapa del nivel tipo DOOM.
    /// Cada celda puede ser un muro, espacio vacío, enemigo, ítem, etc.
    /// </summary>
    public class MapaDoom
    {
        public int Ancho { get; }
        public int Alto { get; }
        private readonly int[,] _grilla;

        // Leyenda del mapa:
        // 0 = espacio vacío
        // 1 = muro tipo A (ladrillo)
        // 2 = muro tipo B (piedra)
        // 3 = muro tipo C (metal)
        // 4 = puerta
        // 9 = spawn del jugador

        public static readonly Dictionary<int, char> SimbolosMuro = new()
        {
            { 0, ' ' },
            { 1, '#' },
            { 2, '%' },
            { 3, '=' },
            { 4, '+' }
        };

        public MapaDoom(int nivel = 1)
        {
            var mapa = ObtenerNivel(nivel);
            Alto = mapa.GetLength(0);
            Ancho = mapa.GetLength(1);
            _grilla = mapa;
        }

        public int ObtenerCelda(int x, int y)
        {
            if (x < 0 || x >= Ancho || y < 0 || y >= Alto)
                return 1; // Fuera del mapa = muro

            return _grilla[y, x];
        }

        public bool EsMuro(int x, int y)
        {
            int celda = ObtenerCelda(x, y);
            return celda >= 1 && celda <= 4;
        }

        public (int x, int y) ObtenerSpawn()
        {
            for (int y = 0; y < Alto; y++)
            {
                for (int x = 0; x < Ancho; x++)
                {
                    if (_grilla[y, x] == 9)
                        return (x, y);
                }
            }
            return (1, 1);
        }

        private int[,] ObtenerNivel(int nivel)
        {
            return nivel switch
            {
                1 => Nivel1(),
                2 => Nivel2(),
                _ => Nivel1()
            };
        }

        private int[,] Nivel1()
        {
            return new int[,]
            {
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
                { 1,9,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
                { 1,0,0,2,2,2,0,0,0,0,3,3,3,0,0,1 },
                { 1,0,0,2,0,0,0,0,0,0,0,0,3,0,0,1 },
                { 1,0,0,2,0,0,0,0,0,0,0,0,3,0,0,1 },
                { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
                { 1,0,0,3,0,0,0,0,0,0,0,0,2,0,0,1 },
                { 1,0,0,3,0,0,0,0,0,0,0,0,2,0,0,1 },
                { 1,0,0,3,3,3,0,0,0,0,2,2,2,0,0,1 },
                { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,4,4,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 }
            };
        }

        private int[,] Nivel2()
        {
            return new int[,]
            {
                { 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2 },
                { 2,9,0,0,0,0,0,0,0,0,0,0,0,0,0,2 },
                { 2,0,0,0,3,3,3,3,0,0,0,0,0,0,0,2 },
                { 2,0,0,0,3,0,0,0,0,0,1,1,1,0,0,2 },
                { 2,0,0,0,3,0,0,0,0,0,1,0,0,0,0,2 },
                { 2,0,0,0,0,0,0,0,0,0,1,0,0,0,0,2 },
                { 2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2 },
                { 2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2 },
                { 2,0,0,1,1,0,0,0,0,0,0,3,3,0,0,2 },
                { 2,0,0,1,0,0,0,0,0,0,0,0,3,0,0,2 },
                { 2,0,0,0,0,0,0,0,0,0,0,0,3,0,0,2 },
                { 2,0,0,0,0,0,0,4,4,0,0,0,0,0,0,2 },
                { 2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2 },
                { 2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2 },
                { 2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2 },
                { 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2 }
            };
        }

        /// <summary>
        /// Muestra el minimapa en la esquina superior derecha
        /// </summary>
        public void DibujarMinimapa(int jugadorX, int jugadorY, double angulo, int offsetX, int offsetY)
        {
            for (int y = 0; y < Alto; y++)
            {
                Console.SetCursorPosition(offsetX, offsetY + y);
                for (int x = 0; x < Ancho; x++)
                {
                    if (x == jugadorX && y == jugadorY)
                        Console.Write('P');
                    else if (_grilla[y, x] >= 1 && _grilla[y, x] <= 4)
                        Console.Write('.');
                    else
                        Console.Write(' ');
                }
            }
        }
    }
}