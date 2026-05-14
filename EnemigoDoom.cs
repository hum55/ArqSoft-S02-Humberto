using System;

namespace Ahorcado
{
    /// <summary>
    /// Representa un enemigo en el juego DOOM.
    /// Tiene IA básica para perseguir al jugador.
    /// </summary>
    public class EnemigoDoom
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int Vida { get; set; }
        public int VidaMaxima { get; }
        public string Tipo { get; }
        public int Dano { get; }
        public int PuntosAlMorir { get; }
        public char Simbolo { get; }
        public bool EstaVivo => Vida > 0;

        private static readonly Random _random = new Random();
        private double _velocidad;

        public EnemigoDoom(double x, double y, string tipo = "Demonio")
        {
            X = x + 0.5;
            Y = y + 0.5;
            Tipo = tipo;

            switch (tipo)
            {
                case "Zombi":
                    Vida = 20;
                    VidaMaxima = 20;
                    Dano = 5;
                    PuntosAlMorir = 50;
                    Simbolo = 'Z';
                    _velocidad = 0.05;
                    break;
                case "Demonio":
                    Vida = 40;
                    VidaMaxima = 40;
                    Dano = 10;
                    PuntosAlMorir = 100;
                    Simbolo = 'D';
                    _velocidad = 0.08;
                    break;
                case "Cacodemon":
                    Vida = 80;
                    VidaMaxima = 80;
                    Dano = 20;
                    PuntosAlMorir = 200;
                    Simbolo = 'C';
                    _velocidad = 0.04;
                    break;
                case "Baron":
                    Vida = 150;
                    VidaMaxima = 150;
                    Dano = 30;
                    PuntosAlMorir = 500;
                    Simbolo = 'B';
                    _velocidad = 0.03;
                    break;
                default:
                    Vida = 30;
                    VidaMaxima = 30;
                    Dano = 8;
                    PuntosAlMorir = 75;
                    Simbolo = 'E';
                    _velocidad = 0.06;
                    break;
            }
        }

        /// <summary>
        /// Mueve al enemigo hacia el jugador (IA básica)
        /// </summary>
        public void Perseguir(double jugadorX, double jugadorY, MapaDoom mapa)
        {
            if (!EstaVivo) return;

            double dx = jugadorX - X;
            double dy = jugadorY - Y;
            double distancia = Math.Sqrt(dx * dx + dy * dy);

            if (distancia > 0.5)
            {
                double dirX = (dx / distancia) * _velocidad;
                double dirY = (dy / distancia) * _velocidad;

                // Agregar algo de aleatoriedad
                dirX += (_random.NextDouble() - 0.5) * 0.02;
                dirY += (_random.NextDouble() - 0.5) * 0.02;

                double nuevoX = X + dirX;
                double nuevoY = Y + dirY;

                if (!mapa.EsMuro((int)nuevoX, (int)Y))
                    X = nuevoX;
                if (!mapa.EsMuro((int)X, (int)nuevoY))
                    Y = nuevoY;
            }
        }

        /// <summary>
        /// Calcula la distancia al jugador
        /// </summary>
        public double DistanciaA(double jugadorX, double jugadorY)
        {
            double dx = jugadorX - X;
            double dy = jugadorY - Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Intenta atacar al jugador si está cerca
        /// </summary>
        public bool PuedeAtacar(double jugadorX, double jugadorY)
        {
            return EstaVivo && DistanciaA(jugadorX, jugadorY) < 1.5;
        }

        public void RecibirDano(int cantidad)
        {
            Vida = Math.Max(0, Vida - cantidad);
        }

        /// <summary>
        /// Representación visual según la distancia
        /// </summary>
        public string ObtenerSprite(double distancia)
        {
            if (distancia < 3)
                return Tipo switch
                {
                    "Zombi" => " /Z\\ ",
                    "Demonio" => " {D} ",
                    "Cacodemon" => " (C) ",
                    "Baron" => " [B] ",
                    _ => " <E> "
                };
            else if (distancia < 6)
                return Simbolo.ToString();
            else
                return ".";
        }
    }
}