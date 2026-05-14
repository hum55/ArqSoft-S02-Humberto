using System;
using System.Collections.Generic;
using System.Threading;

namespace Ahorcado
{
    public class Juego
    {
        private Dictionary<string, List<string>> _categorias = new()
        {
            { "Programacion", new List<string> { "arquitectura", "interfaz", "polimorfismo", "encapsulamiento", "herencia", "abstraccion", "compilacion", "depuracion", "algoritmo", "variable", "constante", "funcion", "clase", "objeto", "parametro" } },
            { "Animales", new List<string> { "gato", "perro", "elefante", "jirafa", "leon", "tigre", "oso", "pajaro", "pez", "serpiente", "caballo", "vaca", "oveja", "cerdo", "conejo" } },
            { "Frutas", new List<string> { "manzana", "platano", "naranja", "fresa", "durazno", "sandia", "melon", "pina", "uva", "kiwi", "limon", "lima", "mora", "cereza", "papaya" } },
            { "Paises", new List<string> { "argentina", "brasil", "canada", "dinamarca", "espana", "francia", "grecia", "hungria", "italia", "japon", "mexico", "noruega", "portugal", "rusia", "suecia" } }
        };

        private Dictionary<string, string> _pistas = new()
        {
            { "arquitectura", "Diseno estructural de un sistema" }, { "interfaz", "Contrato entre clases" },
            { "polimorfismo", "Mismos metodos, diferentes comportamientos" }, { "encapsulamiento", "Ocultar detalles internos" },
            { "herencia", "Relacion padre e hijo" }, { "abstraccion", "Simplificar lo complejo" },
            { "compilacion", "Traducir codigo fuente" }, { "depuracion", "Buscar y corregir errores" },
            { "algoritmo", "Secuencia de pasos" }, { "variable", "Espacio en memoria" },
            { "constante", "Valor que no cambia" }, { "funcion", "Bloque reutilizable" },
            { "clase", "Plantilla para objetos" }, { "objeto", "Instancia de una clase" },
            { "parametro", "Dato que recibe funcion" },
            { "gato", "Felino domestico" }, { "perro", "Mejor amigo del hombre" },
            { "elefante", "El mas grande en tierra" }, { "jirafa", "Cuello muy largo" },
            { "leon", "Rey de la selva" }, { "tigre", "Felino con rayas" },
            { "manzana", "Fruta roja o verde" }, { "platano", "Fruta amarilla" },
            { "naranja", "Citrica color naranja" }, { "fresa", "Fruta roja pequena" },
            { "argentina", "Sudamerica, tango" }, { "brasil", "El mas grande de Sudamerica" },
            { "mexico", "Pais de Norteamerica" }, { "japon", "Pais del sol naciente" },
            { "francia", "Pais de la torre Eiffel" }, { "italia", "Pais con forma de bota" },
        };

        private string _palabraSecreta = "";
        private List<char> _letrasUsadas = new();
        private List<char> _letrasFallidas = new();
        private int _intentosRestantes = 6;
        private string _categoriaActual = "";

        public Juego() { }

        public void Jugar()
        {
            _categoriaActual = MostrarMenuCategorias();
            if (_categoriaActual == "") return;

            var palabras = _categorias[_categoriaActual];
            var random = new Random();
            _palabraSecreta = palabras[random.Next(palabras.Count)];

            Console.Clear();

            while (_intentosRestantes > 0)
            {
                MostrarTablero();

                if (VerificarVictoria())
                {
                    MostrarVictoria();
                    PreguntarJugarDeNuevo();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\n  Ingresa una letra: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                string entrada = Console.ReadLine() ?? "";

                if (entrada.Length == 0) continue;
                char letra = char.ToLower(entrada[0]);
                if (!char.IsLetter(letra)) continue;

                if (_letrasUsadas.Contains(letra))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("  Ya usaste esa letra.");
                    Thread.Sleep(800);
                    continue;
                }

                _letrasUsadas.Add(letra);

                if (!_palabraSecreta.Contains(letra))
                {
                    _letrasFallidas.Add(letra);
                    _intentosRestantes--;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("  X Incorrecta!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("  > Correcta!");
                }
                Thread.Sleep(600);
            }

            MostrarTablero();
            MostrarDerrota();
            PreguntarJugarDeNuevo();
        }

        private string MostrarMenuCategorias()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine();
            Console.WriteLine("  ╔══════════════════════════════════════════╗");
            Console.WriteLine("  ║                                          ║");
            Console.WriteLine("  ║     ███ SELECCIONA CATEGORIA ███         ║");
            Console.WriteLine("  ║                                          ║");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ║     1 ···· Programacion                  ║");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  ║     2 ···· Animales                      ║");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ║     3 ···· Frutas                        ║");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("  ║     4 ···· Paises                        ║");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  ║     0 ···· Volver                        ║");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("  ║                                          ║");
            Console.WriteLine("  ╚══════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n  Opcion: ");

            string op = Console.ReadLine() ?? "0";
            var keys = new List<string>(_categorias.Keys);

            if (int.TryParse(op, out int num) && num >= 1 && num <= keys.Count)
                return keys[num - 1];
            return "";
        }

        private bool VerificarVictoria()
        {
            foreach (char c in _palabraSecreta)
                if (!_letrasUsadas.Contains(c)) return false;
            return true;
        }

        private void MostrarTablero()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"
     █████╗ ██╗  ██╗ ██████╗ ██████╗  ██████╗ █████╗ ██████╗  ██████╗ 
    ██╔══██╗██║  ██║██╔═══██╗██╔══██╗██╔════╝██╔══██╗██╔══██╗██╔═══██╗
    ███████║███████║██║   ██║██████╔╝██║     ███████║██║  ██║██║   ██║
    ██╔══██║██╔══██║██║   ██║██╔══██╗██║     ██╔══██║██║  ██║██║   ██║
    ██║  ██║██║  ██║╚██████╔╝██║  ██║╚██████╗██║  ██║██████╔╝╚██████╔╝
    ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝╚═════╝  ╚═════╝ ");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("    Categoria: {0}", _categoriaActual);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    ─────────────────────────────────────────────────────");

            // Dibujar ahorcado con arte mejorado
            MostrarAhorcadoGrafico();

            // Palabra
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n    Palabra:  ");
            foreach (char c in _palabraSecreta)
            {
                if (_letrasUsadas.Contains(c))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" " + char.ToUpper(c) + " ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" _ ");
                }
            }

            // Intentos
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n    Vidas:    ");
            for (int i = 0; i < _intentosRestantes; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("♥ ");
            }
            for (int i = _intentosRestantes; i < 6; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("♡ ");
            }

            // Letras fallidas
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("\n    Fallidas: ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(string.Join(" ", _letrasFallidas));

            // Pista
            if (_letrasFallidas.Count >= 3)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (_pistas.ContainsKey(_palabraSecreta))
                    Console.Write("\n\n    ★ PISTA: {0}", _pistas[_palabraSecreta]);
            }

            Console.ResetColor();
        }

        private void MostrarAhorcadoGrafico()
        {
            int errores = 6 - _intentosRestantes;
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            switch (errores)
            {
                case 0:
                    Console.WriteLine("        ╔═══════╗     ");
                    Console.WriteLine("        ║       ║     ");
                    Console.WriteLine("        ║             ");
                    Console.WriteLine("        ║             ");
                    Console.WriteLine("        ║             ");
                    Console.WriteLine("        ║             ");
                    Console.WriteLine("    ════╩════════     ");
                    break;
                case 1:
                    Console.WriteLine("        ╔═══════╗     ");
                    Console.WriteLine("        ║       ║     ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("        ║      (O)    ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("        ║             ");
                    Console.WriteLine("        ║             ");
                    Console.WriteLine("        ║             ");
                    Console.WriteLine("    ════╩════════     ");
                    break;
                case 2:
                    Console.WriteLine("        ╔═══════╗     ");
                    Console.WriteLine("        ║       ║     ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("        ║      (O)    ");
                    Console.WriteLine("        ║       |     ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("        ║             ");
                    Console.WriteLine("        ║             ");
                    Console.WriteLine("    ════╩════════     ");
                    break;
                case 3:
                    Console.WriteLine("        ╔═══════╗     ");
                    Console.WriteLine("        ║       ║     ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("        ║      (O)    ");
                    Console.WriteLine("        ║      /|     ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("        ║             ");
                    Console.WriteLine("        ║             ");
                    Console.WriteLine("    ════╩════════     ");
                    break;
                case 4:
                    Console.WriteLine("        ╔═══════╗     ");
                    Console.WriteLine("        ║       ║     ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("        ║      (O)    ");
                    Console.WriteLine("        ║      /|\\   ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("        ║             ");
                    Console.WriteLine("        ║             ");
                    Console.WriteLine("    ════╩════════     ");
                    break;
                case 5:
                    Console.WriteLine("        ╔═══════╗     ");
                    Console.WriteLine("        ║       ║     ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("        ║      (O)    ");
                    Console.WriteLine("        ║      /|\\   ");
                    Console.WriteLine("        ║      /      ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("        ║             ");
                    Console.WriteLine("    ════╩════════     ");
                    break;
                case 6:
                    Console.WriteLine("        ╔═══════╗     ");
                    Console.WriteLine("        ║       ║     ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("        ║      (X)    ");
                    Console.WriteLine("        ║      /|\\   ");
                    Console.WriteLine("        ║      / \\   ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("        ║             ");
                    Console.WriteLine("    ════╩════════     ");
                    break;
            }
            Console.ResetColor();
        }

        private void MostrarVictoria()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
    ╔══════════════════════════════════════════╗
    ║                                          ║
    ║   ██╗   ██╗██╗ ██████╗████████╗██╗       ║
    ║   ██║   ██║██║██╔════╝╚══██╔══╝██║       ║
    ║   ██║   ██║██║██║        ██║   ██║       ║
    ║   ╚██╗ ██╔╝██║██║        ██║   ██║       ║
    ║    ╚████╔╝ ██║╚██████╗   ██║   ███████╗  ║
    ║     ╚═══╝  ╚═╝ ╚═════╝   ╚═╝   ╚══════╝  ║
    ║                                          ║
    ╚══════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n    La palabra era: {0}", _palabraSecreta.ToUpper());
            Console.ResetColor();
            Thread.Sleep(2000);
        }

        private void MostrarDerrota()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\n    ╔══════════════════════════════════╗");
            Console.WriteLine("    ║          GAME OVER                ║");
            Console.WriteLine("    ╚══════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n    La palabra era: {0}", _palabraSecreta.ToUpper());
            Console.ResetColor();
            Thread.Sleep(2000);
        }

        private void PreguntarJugarDeNuevo()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n    Jugar otra vez? (s/n): ");
            string r = Console.ReadLine() ?? "n";
            if (r.ToLower() == "s")
            {
                var nuevo = new Juego();
                nuevo.Jugar();
            }
        }
    }
}