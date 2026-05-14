using System;

namespace Ahorcado
{
    /// <summary>
    /// Representa al jugador en el juego DOOM.
    /// Maneja posición, dirección, vida, munición y armas.
    /// </summary>
    public class Jugador
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Angulo { get; set; }
        public int Vida { get; set; }
        public int VidaMaxima { get; } = 100;
        public int Municion { get; set; }
        public int Puntos { get; set; }
        public int Nivel { get; set; }
        public string ArmaActual { get; set; }

        private readonly double _velocidadMovimiento = 0.3;
        private readonly double _velocidadRotacion = 0.15;

        public Jugador(double x, double y)
        {
            X = x + 0.5;
            Y = y + 0.5;
            Angulo = 0;
            Vida = 100;
            Municion = 50;
            Puntos = 0;
            Nivel = 1;
            ArmaActual = "Pistola";
        }

        /// <summary>
        /// Mueve al jugador hacia adelante si no hay muro
        /// </summary>
        public void MoverAdelante(MapaDoom mapa)
        {
            double nuevoX = X + Math.Cos(Angulo) * _velocidadMovimiento;
            double nuevoY = Y + Math.Sin(Angulo) * _velocidadMovimiento;

            if (!mapa.EsMuro((int)nuevoX, (int)Y))
                X = nuevoX;
            if (!mapa.EsMuro((int)X, (int)nuevoY))
                Y = nuevoY;
        }

        /// <summary>
        /// Mueve al jugador hacia atrás si no hay muro
        /// </summary>
        public void MoverAtras(MapaDoom mapa)
        {
            double nuevoX = X - Math.Cos(Angulo) * _velocidadMovimiento;
            double nuevoY = Y - Math.Sin(Angulo) * _velocidadMovimiento;

            if (!mapa.EsMuro((int)nuevoX, (int)Y))
                X = nuevoX;
            if (!mapa.EsMuro((int)X, (int)nuevoY))
                Y = nuevoY;
        }

        /// <summary>
        /// Movimiento lateral (strafe) a la izquierda
        /// </summary>
        public void MoverIzquierda(MapaDoom mapa)
        {
            double strafeAngle = Angulo - Math.PI / 2;
            double nuevoX = X + Math.Cos(strafeAngle) * _velocidadMovimiento;
            double nuevoY = Y + Math.Sin(strafeAngle) * _velocidadMovimiento;

            if (!mapa.EsMuro((int)nuevoX, (int)Y))
                X = nuevoX;
            if (!mapa.EsMuro((int)X, (int)nuevoY))
                Y = nuevoY;
        }

        /// <summary>
        /// Movimiento lateral (strafe) a la derecha
        /// </summary>
        public void MoverDerecha(MapaDoom mapa)
        {
            double strafeAngle = Angulo + Math.PI / 2;
            double nuevoX = X + Math.Cos(strafeAngle) * _velocidadMovimiento;
            double nuevoY = Y + Math.Sin(strafeAngle) * _velocidadMovimiento;

            if (!mapa.EsMuro((int)nuevoX, (int)Y))
                X = nuevoX;
            if (!mapa.EsMuro((int)X, (int)nuevoY))
                Y = nuevoY;
        }

        public void RotarIzquierda()
        {
            Angulo -= _velocidadRotacion;
        }

        public void RotarDerecha()
        {
            Angulo += _velocidadRotacion;
        }

        public bool EstaVivo()
        {
            return Vida > 0;
        }

        public void RecibirDano(int cantidad)
        {
            Vida = Math.Max(0, Vida - cantidad);
        }

        public void Curar(int cantidad)
        {
            Vida = Math.Min(VidaMaxima, Vida + cantidad);
        }

        /// <summary>
        /// Dispara y reduce la munición
        /// </summary>
        public bool Disparar()
        {
            if (Municion > 0)
            {
                Municion--;
                return true;
            }
            return false;
        }
    }
}