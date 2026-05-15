using System;
using System.Collections.Generic;
using System.Linq;

namespace Ahorcado
{
    public class ConsolaUIViborita
    {
        private readonly MotorViborita _motor;

        public ConsolaUIViborita(MotorViborita motor)
        {
            _motor = motor ?? throw new ArgumentNullException(nameof(motor));
        }

        public void MostrarTablero()
        {
            Console.SetCursorPosition(0, 0);

            // Titulo
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ╔══════════════════════╗");
            Console.Write("  ║  ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("VIBORITA");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  PTS:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("{0,3}", _motor.Puntos);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ║");
            Console.WriteLine("  ╠══════════════════════╣");

            // Borde superior del tablero
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("  ║");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("╔");
            for (int x = 0; x < _motor.Ancho; x++)
                Console.Write("═");
            Console.Write("╗");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("║");

            // Obtener la cabeza
            var cuerpoList = _motor.Cuerpo.ToList();
            var cabeza = cuerpoList.Count > 0 ? cuerpoList[0] : (0, 0);

            // Tablero
            for (int y = 0; y < _motor.Alto; y++)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("  ║");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("║");

                for (int x = 0; x < _motor.Ancho; x++)
                {
                    if (x == cabeza.Item1 && y == cabeza.Item2)
                    {
                        // Cabeza
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("@");
                    }
                    else if (cuerpoList.Any(s => s.Item1 == x && s.Item2 == y))
                    {
                        // Cuerpo
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("█");
                    }
                    else if (_motor.Comida.Item1 == x && _motor.Comida.Item2 == y)
                    {
                        // Comida
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("◆");
                    }
                    else
                    {
                        // Vacio
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("·");
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("║");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("║");
            }

            // Borde inferior
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("  ║");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("╚");
            for (int x = 0; x < _motor.Ancho; x++)
                Console.Write("═");
            Console.Write("╝");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("║");

            // Barra de progreso
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("  ║ ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Meta: ");
            for (int i = 0; i < 10; i++)
            {
                if (i < _motor.Puntos)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("■ ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("□ ");
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("║");

            // Controles
            Console.Write("  ║ ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Flechas=Mover  Q=Salir");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(" ║");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ╚══════════════════════╝");

            Console.ResetColor();
        }

        public ConsoleKey LeerTecla()
        {
            if (Console.KeyAvailable)
                return Console.ReadKey(true).Key;
            return ConsoleKey.NoName;
        }

        public void MostrarMensaje(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(mensaje);
            Console.ResetColor();
            Thread.Sleep(2000);
            Console.ReadKey(true);
        }
    }
}