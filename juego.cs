using System;
using System.Collections.Generic;

namespace Ahorcado
{
    public class Juego
    {
        private Dictionary<string, List<string>> _categorias = new()
        {
            {
                "Programacion",
                new List<string>
                {
                    "arquitectura", "interfaz", "polimorfismo",
                    "encapsulamiento", "herencia", "abstraccion",
                    "compilacion", "depuracion", "algoritmo", "variable",
                    "constante", "funcion", "clase", "objeto", "parametro"
                }
            },
            {
                "Animales",
                new List<string>
                {
                    "gato", "perro", "elefante", "jirafa", "leon",
                    "tigre", "oso", "pajaro", "pez", "serpiente",
                    "caballo", "vaca", "oveja", "cerdo", "conejo"
                }
            },
            {
                "Frutas",
                new List<string>
                {
                    "manzana", "platano", "naranja", "fresa", "durazno",
                    "sandia", "melon", "pina", "uva", "kiwi",
                    "limon", "lima", "mora", "cereza", "papaya"
                }
            },
            {
                "Paises",
                new List<string>
                {
                    "argentina", "brasil", "canada", "dinamarca", "españa",
                    "francia", "grecia", "hungria", "italia", "japon",
                    "mexico", "noruega", "portugal", "rusia", "tailandia"
                }
            }
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
            { "parametro", "Dato que recibe una funcion" },

            { "gato", "Felino domestico" },
            { "perro", "Mejor amigo del hombre" },
            { "elefante", "Animal mas grande de tierra" },
            { "jirafa", "Animal con cuello muy largo" },
            { "leon", "Rey de la selva" },
            { "tigre", "Felino salvaje con rayas" },
            { "oso", "Animal grande y peligroso" },
            { "pajaro", "Animal con plumas y alas" },
            { "pez", "Animal acuatico con branquias" },
            { "serpiente", "Reptil sin patas" },

            { "manzana", "Fruta roja o verde" },
            { "platano", "Fruta amarilla alargada" },
            { "naranja", "Fruta citrica de color naranja" },
            { "fresa", "Fruta roja pequeña" },
            { "sandia", "Fruta grande con semillas negras" },
            { "melon", "Fruta dulce naranja" },
            { "pina", "Fruta tropical con corona" },
            { "uva", "Fruta pequeña en racimos" },
            { "kiwi", "Fruta verde con semillas" },

            { "argentina", "Pais de Sudamerica" },
            { "brasil", "Pais mas grande de Sudamerica" },
            { "canada", "Pais de Norteamerica" },
            { "españa", "Pais de Europa" },
            { "france", "Pais europeo conocido por la Torre Eiffel" },
            { "italia", "Pais con forma de bota" },
            { "japon", "Pais asiatico de oriente" },
            { "mexico", "Pais de Norteamerica" }
        };

        private string _palabraSecreta;
        private List<char> _letrasUsadas;
        private List<char> _letrasFallidas;
        private int _intentosRestantes;
        private bool _pistaMostrada;
        private string _categoriaActual;

        public Juego()
        {
            _letrasUsadas = new List<char>();
            _letrasFallidas = new List<char>();
            _intentosRestantes = 6;
            _pistaMostrada = false;
        }

        public void Jugar()
        {
            // Mostrar menu de categorias
            _categoriaActual = MostrarMenuCategorias();

            if (_categoriaActual == null)
                return; // Si cancela

            // Seleccionar palabra aleatoria de la categoria
            var palabrasCategoria = _categorias[_categoriaActual];
            var random = new Random();
            _palabraSecreta = palabrasCategoria[random.Next(palabrasCategoria.Count)];

            bool jugarDeNuevo = true;

            while (jugarDeNuevo)
            {
                Console.Clear();
                Console.WriteLine("=== AHORCADO ===");
                Console.WriteLine("Categoria: {0}\n", _categoriaActual);

                while (_intentosRestantes > 0)
                {
                    MostrarTablero();

                    if (VerificarVictoria())
                    {
                        Console.WriteLine("\nGANASTE! La palabra era: " + _palabraSecreta);
                        Console.Write("\nJugar otra vez? (s/n): ");
                        string resp = Console.ReadLine() ?? "n";
                        jugarDeNuevo = (resp.ToLower() == "s");

                        if (jugarDeNuevo)
                        {
                            // Reiniciar para nueva partida
                            var nuevoJuego = new Juego();
                            nuevoJuego.Jugar();
                        }
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

        private string MostrarMenuCategorias()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== SELECCIONA UNA CATEGORIA ===\n");

                int i = 1;
                foreach (var categoria in _categorias.Keys)
                {
                    Console.WriteLine("  {0} - {1}", i, categoria);
                    i++;
                }
                Console.WriteLine("  0 - Volver");

                Console.Write("\nOpcion: ");
                string opcion = Console.ReadLine();

                int.TryParse(opcion, out int numero);

                if (numero == 0)
                    return null;

                if (numero >= 1 && numero <= _categorias.Count)
                {
                    i = 1;
                    foreach (var categoria in _categorias.Keys)
                    {
                        if (i == numero)
                            return categoria;
                        i++;
                    }
                }

                Console.WriteLine("Opcion no valida");
                System.Threading.Thread.Sleep(1000);
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
            Console.WriteLine("=== AHORCADO ===");
            Console.WriteLine("Categoria: {0}\n", _categoriaActual);

            MostrarAhorcado();
            Console.WriteLine("\nIntentos restantes: {0}", _intentosRestantes);
            Console.WriteLine("Letras fallidas: {0}", string.Join(", ", _letrasFallidas));

            // Mostrar pista si tiene 3 o mas errores
            if (_letrasFallidas.Count >= 3)
            {
                if (_pistas.ContainsKey(_palabraSecreta))
                {
                    Console.WriteLine("\nPISTA: {0}", _pistas[_palabraSecreta]);
                }
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