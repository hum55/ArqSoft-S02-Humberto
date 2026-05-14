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
<<<<<<< HEAD
                // Limpiar TODO (elimina residuos del DOOM)
                Console.Clear();
                Console.ResetColor();
                try
                {
                    Console.SetWindowSize(82, 35);
                    Console.SetBufferSize(82, 35);
                }
                catch { }

                Console.CursorVisible = false;
                MostrarMenuArcade();
                Console.Write("\n");
                Centrar(">> ");
                Console.ForegroundColor = ConsoleColor.White;

                Console.CursorVisible = true;
                string opcion = Console.ReadLine() ?? "";
                Console.CursorVisible = false;

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
                        MostrarDespedida();
                        salir = true;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Centrar("OPCION NO VALIDA");
                        Console.ResetColor();
                        Thread.Sleep(1000);
                        break;
                }
            }

            Console.ResetColor();
            Console.CursorVisible = true;
        }

        // ========================================
        //        MENU PRINCIPAL ARCADE
        // ========================================

        static void MostrarMenuArcade()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Centrar("════════════════════════════════════════════════════════");
            Console.WriteLine();

            // Título ASCII épico
            Console.ForegroundColor = ConsoleColor.Cyan;
            Centrar("  ██████╗  █████╗ ████████╗██╗     ███████╗");
            Centrar("  ██╔══██╗██╔══██╗╚══██╔══╝██║     ██╔════╝");
            Centrar("  ██████╔╝███████║   ██║   ██║     █████╗  ");
            Centrar("  ██╔══██╗██╔══██║   ██║   ██║     ██╔══╝  ");
            Centrar("  ██████╔╝██║  ██║   ██║   ███████╗███████╗");
            Centrar("  ╚═════╝ ╚═╝  ╚═╝   ╚═╝   ╚══════╝╚══════╝");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Centrar("  █████╗ ██████╗  ██████╗ █████╗ ██████╗ ███████╗");
            Centrar("  ██╔══██╗██╔══██╗██╔════╝██╔══██╗██╔══██╗██╔════╝");
            Centrar("  ███████║██████╔╝██║     ███████║██║  ██║█████╗  ");
            Centrar("  ██╔══██║██╔══██╗██║     ██╔══██║██║  ██║██╔══╝  ");
            Centrar("  ██║  ██║██║  ██║╚██████╗██║  ██║██████╔╝███████╗");
            Centrar("  ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝╚═════╝ ╚══════╝");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Centrar("════════════════════════════════════════════════════════");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Centrar("I N S E R T   C O I N");
            Console.WriteLine();

            // Opciones del menú
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Centrar("╔══════════════════════════════════════╗");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Centrar("║                                      ║");

            Console.ForegroundColor = ConsoleColor.Green;
            CentrarEnCaja("  1 ···· AHORCADO          ", 40);

            Console.ForegroundColor = ConsoleColor.Cyan;
            CentrarEnCaja("  2 ···· VIBORITA           ", 40);

            Console.ForegroundColor = ConsoleColor.Red;
            CentrarEnCaja("  3 ···· DOOM STYLE         ", 40);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Centrar("║                                      ║");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            CentrarEnCaja("  4 ···· SALIR              ", 40);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Centrar("║                                      ║");
            Centrar("╚══════════════════════════════════════╝");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Centrar("C R E D I T S :  9 9");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Centrar("════════════════════════════════════════════════════════");
        }

        // ========================================
        //           DESPEDIDA ARCADE
        // ========================================

        static void MostrarDespedida()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine();
            Centrar("╔══════════════════════════════════════╗");
            Centrar("║                                      ║");
            Centrar("║      G A M E    O V E R              ║");
            Centrar("║                                      ║");
            Centrar("║      GRACIAS POR JUGAR!              ║");
            Centrar("║                                      ║");
            Centrar("╚══════════════════════════════════════╝");
            Console.ResetColor();
            Thread.Sleep(2000);
        }

        // ========================================
        //         FUNCIONES DE CENTRADO
        // ========================================

        static void Centrar(string texto)
        {
            int ancho = 80;
            try { ancho = Console.WindowWidth; } catch { }
            int pad = Math.Max(0, (ancho - texto.Length) / 2);
            Console.WriteLine(new string(' ', pad) + texto);
        }

        static void CentrarEnCaja(string contenido, int anchoCaja)
        {
            int ancho = 80;
            try { ancho = Console.WindowWidth; } catch { }

            string linea = "║" + contenido.PadRight(anchoCaja - 2) + "║";
            int pad = Math.Max(0, (ancho - linea.Length) / 2);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(new string(' ', pad) + "║");

            // Color del contenido según el juego
            ConsoleColor colorOriginal = Console.ForegroundColor;
            if (contenido.Contains("AHORCADO")) Console.ForegroundColor = ConsoleColor.Green;
            else if (contenido.Contains("VIBORITA")) Console.ForegroundColor = ConsoleColor.Cyan;
            else if (contenido.Contains("DOOM")) Console.ForegroundColor = ConsoleColor.Red;
            else if (contenido.Contains("SALIR")) Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.Write(contenido.PadRight(anchoCaja - 2));

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("║");
        }

        // ========================================
        //          LANZAR JUEGOS
        // ========================================

        static void JugarAhorcado()
        {
            try
            {
                Console.Clear();
                Console.ResetColor();
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
                Console.Clear();
                Console.ResetColor();
                var motor = new MotorViborita();
                var ui = new ConsolaUIViborita(motor);

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
                Console.ReadKey(true);
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
                Console.Clear();
                Console.ResetColor();
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
            Console.ForegroundColor = ConsoleColor.Red;
            Centrar("ERROR en " + juego + ": " + mensaje);
            Console.ResetColor();
            Console.WriteLine("\n  Presiona una tecla...");
            Console.ReadKey(true);
=======
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
>>>>>>> FEAT/viborita3B
        }
    }
}