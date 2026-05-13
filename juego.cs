using System;
using System.Collections.Generic;

namespace Ahorcado
{
    public class Juego
    {
        private List<string> _palabras = new()
        {
            "arquitectura", "interfaz", "polimorfismo",
            "encapsulamiento", "herencia", "abstraccion",
            "compilacion", "depuracion", "algoritmo", "variable",
            "constante", "funcion", "clase", "objeto", "parametro"
        };

        private Dictionary<string, string> _pistas = new()
        {
            { "arquitectura", "Diseño estructural de un sistema" },
            { "interfaz", "Contrato entre clases" },
            { "polimorfismo", "Mismos metodos, diferentes comportamientos" },
            { "encapsulamiento", "Ocultamiento de detalles internos" },
            { "herencia", "Relacion entre clases padre e hijo" },
            { "abstraccion", "Representacion simplificada de algo complejo" },
            { "compilacion", "Proceso de traducir codigo fuente" },
            { "depuracion", "Busqueda y correccion de errores" },
            { "algoritmo", "Secuencia de pasos para resolver un problema" },
            { "variable", "Espacio en memoria para almacenar datos" },
            { "constante", "Valor que no cambia durante la ejecucion" },
            { "funcion", "Bloque de codigo reutilizable" },
            { "clase", "Plantilla para crear objetos" },
            { "objeto", "Instancia de una clase" },
            { "parametro", "Dato que recibe una funcion" }
        };

        private string _palabraSecreta;
        private List<char> _letrasUsadas;
        private List<char> _letrasFallidas;
        private int _intentosRestantes;
        private bool _pistaMostrada;

        public Juego()
        {
            var random = new Random();
            _palabraSecreta = _palabras[random.Next(_palabras.Count)];
            _letrasUsadas = new List<char>();
            _letrasFallidas = new List<char>();
            _intentosRestantes = 6;
            _pistaMostrada = false;
        }

        public void Jugar()
        {
            bool jugarDeNuevo = true;

            while (jugarDeNuevo)
            {
                Console.Clear();
                Console.WriteLine("=== AHORCADO ===\n");

                while (_intentosRestantes > 0)
                {
                    MostrarTablero();

                    if (VerificarVictoria())
                    {
                        Console.WriteLine("\nGANASTE! La palabra era: " + _palabraSecreta);
                        Console.Write("\nJugar otra vez? (s/n): ");
                        string resp = Console.ReadLine() ?? "n";
                        jugarDeNuevo = (resp.ToLower() == "s");
                        return;
                    }

                    Console.Write("\nIngresa una letra: ");
                    string entrada = Console.ReadLine() ?? "";

                    if (entrada.Length == 0)
                    {
                        Console.WriteLine("Ingresa una letra.");
                        System.Threading.Thread.Sleep(800);
                        continue;
                    }

                    char letra = char.ToLower(entrada[0]);

                    if (!char.IsLetter(letra))
                    {
                        Console.WriteLine("Solo letras.");
                        System.Threading.Thread.Sleep(800);
                        continue;
                    }

                    if (_letrasUsadas.Contains(letra))
                    {
                        Console.WriteLine("Ya usaste esa letra.");
                        System.Threading.Thread.Sleep(800);
                        continue;
                    }

                    _letrasUsadas.Add(letra);

                    if (!_palabraSecreta.Contains(letra))
                    {
                        _letrasFallidas.Add(letra);
                        _intentosRestantes--;
                    }

                    System.Threading.Thread.Sleep(800);
                }

                MostrarTablero();
                Console.WriteLine("\nPERDISTE. La palabra era: " + _palabraSecreta);
                Console.Write("\nJugar otra vez? (s/n): ");
                string respuesta = Console.ReadLine() ?? "n";

                if (respuesta.ToLower() == "s")
                {
                    var nuevoJuego = new Juego();
                    nuevoJuego.Jugar();
                }
                return;
            }
        }

        private bool VerificarVictoria()
        {
            foreach (char c in _palabraSecreta)
            {
                if (!_letrasUsadas.Contains(c))
                    return false;
            }
            return true;
        }

        private void MostrarTablero()
        {
            Console.Clear();
            MostrarAhorcado();
            Console.WriteLine("\nIntentos restantes: {0}", _intentosRestantes);
            Console.WriteLine("Letras fallidas: {0}", string.Join(", ", _letrasFallidas));

            // Mostrar pista si tiene 3 o mas errores
            if (_letrasFallidas.Count >= 3 && !_pistaMostrada)
            {
                _pistaMostrada = true;
                Console.WriteLine("\nPISTA: {0}", _pistas[_palabraSecreta]);
            }
            else if (_letrasFallidas.Count >= 3 && _pistaMostrada)
            {
                Console.WriteLine("\nPISTA: {0}", _pistas[_palabraSecreta]);
            }

            Console.Write("\nPalabra: ");

            foreach (char c in _palabraSecreta)
            {
                if (_letrasUsadas.Contains(c))
                    Console.Write(c);
                else
                    Console.Write("_");
            }
            Console.WriteLine("\n");
        }

        private void MostrarAhorcado()
        {
            switch (6 - _intentosRestantes)
            {
                case 0:
                    Console.WriteLine("  +---+");
                    Console.WriteLine("  |   |");
                    Console.WriteLine("      |");
                    Console.WriteLine("      |");
                    Console.WriteLine("      |");
                    Console.WriteLine("      |");
                    Console.WriteLine("=========");
                    break;
                case 1:
                    Console.WriteLine("  +---+");
                    Console.WriteLine("  |   |");
                    Console.WriteLine("  O   |");
                    Console.WriteLine("      |");
                    Console.WriteLine("      |");
                    Console.WriteLine("      |");
                    Console.WriteLine("=========");
                    break;
                case 2:
                    Console.WriteLine("  +---+");
                    Console.WriteLine("  |   |");
                    Console.WriteLine("  O   |");
                    Console.WriteLine("  |   |");
                    Console.WriteLine("      |");
                    Console.WriteLine("      |");
                    Console.WriteLine("=========");
                    break;
                case 3:
                    Console.WriteLine("  +---+");
                    Console.WriteLine("  |   |");
                    Console.WriteLine("  O   |");
                    Console.WriteLine(" /|   |");
                    Console.WriteLine("      |");
                    Console.WriteLine("      |");
                    Console.WriteLine("=========");
                    break;
                case 4:
                    Console.WriteLine("  +---+");
                    Console.WriteLine("  |   |");
                    Console.WriteLine("  O   |");
                    Console.WriteLine(" /|\\  |");
                    Console.WriteLine("      |");
                    Console.WriteLine("      |");
                    Console.WriteLine("=========");
                    break;
                case 5:
                    Console.WriteLine("  +---+");
                    Console.WriteLine("  |   |");
                    Console.WriteLine("  O   |");
                    Console.WriteLine(" /|\\  |");
                    Console.WriteLine(" /    |");
                    Console.WriteLine("      |");
                    Console.WriteLine("=========");
                    break;
                case 6:
                    Console.WriteLine("  +---+");
                    Console.WriteLine("  |   |");
                    Console.WriteLine("  O   |");
                    Console.WriteLine(" /|\\  |");
                    Console.WriteLine(" / \\  |");
                    Console.WriteLine("      |");
                    Console.WriteLine("=========");
                    break;
            }
        }
    }
}