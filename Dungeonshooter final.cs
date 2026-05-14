using System;
using System.Collections.Generic;
using System.Threading;

namespace Ahorcado
{
    public class DungeonShooter
    {
        private int _ancho = 60;
        private int _alto = 20;

        private int _playerX = 30;
        private int _playerY = 10;
        private int _playerVida = 100;
        private int _playerMunicion = 50;
        private int _playerPuntos = 0;

        private char[,] _mapa = new char[0, 0];

        private List<Enemigo> _enemigos = new List<Enemigo>();
        private List<Disparo> _disparos = new List<Disparo>();

        private bool _juegoActivo = true;

        public DungeonShooter()
        {
            InicializarMapa();
            GenerarEnemigos();
        }

        private void InicializarMapa()
        {
            _mapa = new char[_alto, _ancho];

            for (int y = 0; y < _alto; y++)
            {
                for (int x = 0; x < _ancho; x++)
                {
                    _mapa[y, x] = ' ';
                }
            }

            for (int x = 0; x < _ancho; x++)
            {
                _mapa[0, x] = '#';
                _mapa[_alto - 1, x] = '#';
            }
            for (int y = 0; y < _alto; y++)
            {
                _mapa[y, 0] = '#';
                _mapa[y, _ancho - 1] = '#';
            }

            for (int x = 10; x < 20; x++)
            {
                _mapa[5, x] = '#';
            }
            for (int x = 40; x < 50; x++)
            {
                _mapa[15, x] = '#';
            }
        }

        private void GenerarEnemigos()
        {
            _enemigos.Clear();
            var random = new Random();

            for (int i = 0; i < 3; i++)
            {
                int x = random.Next(10, 50);
                int y = random.Next(3, 17);
                _enemigos.Add(new Enemigo(x, y, 20));
            }
        }

        public void Jugar()
        {
            Console.Clear();
            Console.CursorVisible = false;

            while (_juegoActivo && _playerVida > 0)
            {
                MostrarMapa();
                ProcesarEntrada();
                ActualizarEnemigos();
                ActualizarDisparos();
                VerificarColisiones();

                if (_enemigos.Count == 0)
                {
                    GenerarEnemigos();
                }

                Thread.Sleep(100);
            }

            Console.Clear();
            MostrarPantallaPerdida();
            Console.CursorVisible = true;
        }

        private void MostrarMapa()
        {
            Console.Clear();

            for (int y = 0; y < _alto; y++)
            {
                for (int x = 0; x < _ancho; x++)
                {
                    char celda = _mapa[y, x];

                    if (x == _playerX && y == _playerY)
                        Console.Write('@');
                    else
                    {
                        bool esEnemigo = false;
                        foreach (var enemigo in _enemigos)
                        {
                            if (enemigo.X == x && enemigo.Y == y)
                            {
                                Console.Write('E');
                                esEnemigo = true;
                                break;
                            }
                        }

                        if (!esEnemigo)
                        {
                            bool esDisparo = false;
                            foreach (var disparo in _disparos)
                            {
                                if (disparo.X == x && disparo.Y == y)
                                {
                                    Console.Write('*');
                                    esDisparo = true;
                                    break;
                                }
                            }

                            if (!esDisparo)
                                Console.Write(celda);
                        }
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n=== DUNGEON SHOOTER ===");
            Console.WriteLine("Vida: {0}/100 | Municion: {1} | Puntos: {2}", _playerVida, _playerMunicion, _playerPuntos);
            Console.WriteLine("Enemigos: {0} | Controles: W/A/S/D=Mover, ESPACIO=Disparar, Q=Salir", _enemigos.Count);
        }

        private void ProcesarEntrada()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                int nuevoX = _playerX;
                int nuevoY = _playerY;

                switch (key)
                {
                    case ConsoleKey.W:
                        nuevoY--;
                        break;
                    case ConsoleKey.S:
                        nuevoY++;
                        break;
                    case ConsoleKey.A:
                        nuevoX--;
                        break;
                    case ConsoleKey.D:
                        nuevoX++;
                        break;
                    case ConsoleKey.Spacebar:
                        Disparar();
                        break;
                    case ConsoleKey.Q:
                        _juegoActivo = false;
                        break;
                }

                if (nuevoX > 0 && nuevoX < _ancho - 1 &&
                    nuevoY > 0 && nuevoY < _alto - 1 &&
                    _mapa[nuevoY, nuevoX] == ' ')
                {
                    _playerX = nuevoX;
                    _playerY = nuevoY;
                }
            }
        }

        private void Disparar()
        {
            if (_playerMunicion > 0)
            {
                _disparos.Add(new Disparo(_playerX, _playerY, 1, 0));
                _disparos.Add(new Disparo(_playerX, _playerY, -1, 0));
                _disparos.Add(new Disparo(_playerX, _playerY, 0, 1));
                _disparos.Add(new Disparo(_playerX, _playerY, 0, -1));
                _playerMunicion -= 4;
            }
        }

        private void ActualizarEnemigos()
        {
            var random = new Random();
            foreach (var enemigo in _enemigos)
            {
                int dx = random.Next(-1, 2);
                int dy = random.Next(-1, 2);

                int nuevoX = enemigo.X + dx;
                int nuevoY = enemigo.Y + dy;

                if (nuevoX > 1 && nuevoX < _ancho - 1 &&
                    nuevoY > 1 && nuevoY < _alto - 1 &&
                    _mapa[nuevoY, nuevoX] == ' ')
                {
                    enemigo.X = nuevoX;
                    enemigo.Y = nuevoY;
                }

                if (Math.Abs(enemigo.X - _playerX) < 5 &&
                    Math.Abs(enemigo.Y - _playerY) < 5)
                {
                    _playerVida -= 1;
                }
            }
        }

        private void ActualizarDisparos()
        {
            for (int i = _disparos.Count - 1; i >= 0; i--)
            {
                var disparo = _disparos[i];
                disparo.X += disparo.DX;
                disparo.Y += disparo.DY;

                if (disparo.X <= 0 || disparo.X >= _ancho - 1 ||
                    disparo.Y <= 0 || disparo.Y >= _alto - 1 ||
                    _mapa[disparo.Y, disparo.X] == '#')
                {
                    _disparos.RemoveAt(i);
                }
            }
        }

        private void VerificarColisiones()
        {
            for (int i = _disparos.Count - 1; i >= 0; i--)
            {
                var disparo = _disparos[i];
                for (int j = _enemigos.Count - 1; j >= 0; j--)
                {
                    var enemigo = _enemigos[j];
                    if (disparo.X == enemigo.X && disparo.Y == enemigo.Y)
                    {
                        enemigo.Vida -= 10;
                        _disparos.RemoveAt(i);

                        if (enemigo.Vida <= 0)
                        {
                            _enemigos.RemoveAt(j);
                            _playerPuntos += 100;
                        }
                        break;
                    }
                }
            }
        }

        private void MostrarPantallaPerdida()
        {
            Console.Clear();
            Console.WriteLine("\n");
            Console.WriteLine("     GAME OVER");
            Console.WriteLine("\nVida Final: {0}", _playerVida);
            Console.WriteLine("Puntos Totales: {0}", _playerPuntos);
            Console.WriteLine("Enemigos Eliminados: {0}", _playerPuntos / 100);
            Console.WriteLine("\nPresiona una tecla para volver al menu...");
            Console.ReadKey();
        }
    }
}