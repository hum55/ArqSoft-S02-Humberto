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
                Console.WriteLine("Que juego quieres jugar?");
                Console.WriteLine("  1 - Ahorcado");
                Console.WriteLine("  2 - Viborita");
                Console.WriteLine("  3 - Salir");
                Console.Write("Opcion: ");

                string opcion = Console.ReadLine();

                if (opcion == "1")
                {
                    try
                    {
                        var juego = new Juego();
                        juego.Jugar();
                    }
                    catch (Exception ex)
                    {
                        Console.Clear();
                        Console.WriteLine("ERROR: " + ex.Message);
                        Console.WriteLine("Presiona una tecla...");
                        Console.ReadKey();
                    }
                }
                else if (opcion == "2")
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

                            if (tecla == ConsoleKey.Q)
                                break;

                            if (tecla != ConsoleKey.NoName)
                                motor.CambiarDireccion(tecla);

                            motor.Avanzar();
                            Thread.Sleep(150);
                        }

                        Console.CursorVisible = true;
                        Console.Clear();
                        ui.MostrarTablero();

                        if (motor.Ganado())
                            Console.WriteLine("\nGANASTE! Llegaste a 10 puntos.");
                        else
                            Console.WriteLine("\nGame over.");

                        Console.WriteLine("\nPresiona una tecla...");
                        Console.ReadKey();
                    }
                    catch (Exception ex)
                    {
                        Console.Clear();
                        Console.WriteLine("ERROR en Viborita: " + ex.Message);
                        Console.WriteLine("Presiona una tecla...");
                        Console.ReadKey();
                    }
                }
                else if (opcion == "3")
                {
                    salir = true;
                }
                else
                {
                    Console.WriteLine("Opcion no valida");
                    Thread.Sleep(1000);
                }
            }
        }
    }
}