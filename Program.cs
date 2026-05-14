using System;
using System.Threading;

namespace Ahorcado
{
    class Program
    {
        static void Main()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("  ╔══════════════════════════════════════════╗");
                Console.WriteLine("  ║        CENTRO DE JUEGOS v2.0             ║");
                Console.WriteLine("  ╠══════════════════════════════════════════╣");
                Console.WriteLine("  ║                                          ║");
                Console.WriteLine("  ║   1 - Ahorcado                           ║");
                Console.WriteLine("  ║   2 - Viborita                           ║");
                Console.WriteLine("  ║   3 - Doom Style (Pseudo-3D)             ║");
                Console.WriteLine("  ║   4 - Salir                              ║");
                Console.WriteLine("  ║                                          ║");
                Console.WriteLine("  ╚══════════════════════════════════════════╝");
                Console.Write("\n  Opcion: ");

                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1":
                        JugarAhorcado();
                        break;
                    case "2":
                        JugarViborita();
                        break;
                    case "3":
                        JugarDoom();
                        break;
                    case "4":
                        salir = true;
                        Console.WriteLine("\n  Hasta luego!");
                        Thread.Sleep(1000);
                        break;
                    default:
                        Console.WriteLine("  Opcion no valida");
                        Thread.Sleep(1000);
                        break;
                }
            }
        }

        static void JugarAhorcado()
        {
            try
            {
                var juego = new Juego();
                juego.Jugar();
            }
            catch (Exception ex)
            {
                MostrarError("Ahorcado", ex.Message);
            }
        }

        static void JugarViborita()
        {
            try
            {
                var motor = new MotorViborita();
                var ui = new ConsolaUIViborita(motor);

                Console.Clear();
                Console.CursorVisible = false;

                while (!motor.Ganado() && !motor.Perdido())
                {
                    ui.MostrarTablero();
                    var tecla = ui.LeerTecla();

                    if (tecla == ConsoleKey.Q) break;
                    if (tecla != ConsoleKey.NoName)
                        motor.CambiarDireccion(tecla);

                    motor.Avanzar();
                    Thread.Sleep(150);
                }

                Console.CursorVisible = true;
                Console.Clear();
                ui.MostrarTablero();
                Console.WriteLine(motor.Ganado()
                    ? "\n  GANASTE! Llegaste a 10 puntos."
                    : "\n  Game over.");
                Console.WriteLine("  Presiona una tecla...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                MostrarError("Viborita", ex.Message);
            }
        }

        static void JugarDoom()
        {
            try
            {
                var motor = new MotorDoom();
                motor.Jugar();
            }
            catch (Exception ex)
            {
                MostrarError("Doom Style", ex.Message);
            }
        }

        static void MostrarError(string juego, string mensaje)
        {
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine("  ERROR en {0}: {1}", juego, mensaje);
            Console.WriteLine("  Presiona una tecla...");
            Console.ReadKey();
        }
    }
}