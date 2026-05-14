using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Ahorcado
{
    public class MotorDoom : iMotorjuego
    {
        // Pantalla
        private readonly int _anchoVista = 80;
        private readonly int _altoVista = 30;
        private readonly double _fov = Math.PI / 3.0;
        private readonly double _profundidadMax = 16.0;

        // Componentes
        public MapaDoom Mapa { get; private set; } = null!;
        public Jugador Player { get; private set; } = null!;
        public List<EnemigoDoom> Enemigos { get; private set; } = new();

        // Estado
        private bool _juegoActivo = true;
        private bool _ganado = false;
        private int _nivelActual = 1;
        private int _totalEnemigosEliminados = 0;

        // Buffer
        private char[,] _buffer;
        private double[] _profundidadBuffer;

        // Efectos visuales
        private int _flashDisparo = 0;
        private int _flashDano = 0;
        private int _animRecarga = 0;
        private int _animMuerte = 0;
        private string _mensajeKill = "";
        private int _mensajeKillTimer = 0;
        private int _frameArma = 0;
        private bool _recargando = false;

        public MotorDoom()
        {
            _buffer = new char[_altoVista, _anchoVista];
            _profundidadBuffer = new double[_anchoVista];
            InicializarNivel(_nivelActual);
        }

        private void InicializarNivel(int nivel)
        {
            Mapa = new MapaDoom(nivel);
            var spawn = Mapa.ObtenerSpawn();
            Player = new Jugador(spawn.x, spawn.y);
            Player.Nivel = nivel;
            Enemigos = GenerarEnemigos(nivel);
        }

        private List<EnemigoDoom> GenerarEnemigos(int nivel)
        {
            var enemigos = new List<EnemigoDoom>();
            var random = new Random();
            int cantidad = 3 + (nivel * 2);
            string[] tipos = { "Zombi", "Demonio", "Cacodemon", "Baron" };

            for (int i = 0; i < cantidad; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(2, Mapa.Ancho - 2);
                    y = random.Next(2, Mapa.Alto - 2);
                } while (Mapa.EsMuro(x, y));

                int tipoIndex = Math.Min(i / 2, tipos.Length - 1);
                if (nivel >= 2) tipoIndex = Math.Min(tipoIndex + 1, tipos.Length - 1);
                enemigos.Add(new EnemigoDoom(x, y, tipos[tipoIndex]));
            }
            return enemigos;
        }

        // ========================================
        //          GAME LOOP PRINCIPAL
        // ========================================

        public void Jugar()
        {
            Console.CursorVisible = false;
            Console.Clear();

            try
            {
                Console.SetWindowSize(
                    Math.Min(_anchoVista + 2, Console.LargestWindowWidth),
                    Math.Min(_altoVista + 8, Console.LargestWindowHeight));
                Console.SetBufferSize(
                    Math.Min(_anchoVista + 2, Console.LargestWindowWidth),
                    Math.Min(_altoVista + 8, Console.LargestWindowHeight));
            }
            catch { }

            // Pantalla de inicio
            MostrarPantallaInicio();

            while (_juegoActivo && Player.EstaVivo())
            {
                Renderizar();
                ProcesarEntrada();
                ActualizarEnemigos();
                ActualizarEfectos();
                VerificarEstado();
                Thread.Sleep(50);
            }

            if (!Player.EstaVivo())
                AnimacionMuerte();

            MostrarPantallaFinal();
            Console.CursorVisible = true;
        }

        // ========================================
        //       PANTALLA DE INICIO EPICA
        // ========================================

        private void MostrarPantallaInicio()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
  ____    ___    ___   __  __     ____ _____  ____  _     _____ 
 |  _ \  / _ \  / _ \ |  \/  |   / ___|_   _||  _ \| |   | ____|
 | | | || | | || | | || |\/| |   \___ \ | |  | |_) | |   |  _|  
 | |_| || |_| || |_| || |  | |    ___) || |  |  __/| |___| |___ 
 |____/  \___/  \___/ |_|  |_|   |____/ |_|  |_|   |_____|_____|
                                                                  ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("  ═══════════════════════════════════════════════════════════");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n                    NIVEL {0} - PREPARATE", _nivelActual);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n   Controles:");
            Console.WriteLine("   W/S = Adelante/Atras    A/D = Rotar");
            Console.WriteLine("   Q/E = Strafe            ESPACIO = Disparar");
            Console.WriteLine("   R = Recargar            X = Salir");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n              Elimina a todos los enemigos para avanzar");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n          >>> Presiona cualquier tecla para comenzar <<<");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        // ========================================
        //        RENDERIZADO PRINCIPAL
        // ========================================

        private void Renderizar()
        {
            // Limpiar buffer
            for (int y = 0; y < _altoVista; y++)
                for (int x = 0; x < _anchoVista; x++)
                    _buffer[y, x] = ' ';

            // Raycasting
            for (int x = 0; x < _anchoVista; x++)
            {
                double anguloRayo = (Player.Angulo - _fov / 2.0) + ((double)x / _anchoVista) * _fov;
                double distancia = LanzarRayo(anguloRayo, out int tipoMuro);
                _profundidadBuffer[x] = distancia;

                int alturaLinea = (int)(_altoVista / distancia);
                int techoInicio = (_altoVista / 2) - (alturaLinea / 2);
                int pisoInicio = (_altoVista / 2) + (alturaLinea / 2);

                for (int y = 0; y < _altoVista; y++)
                {
                    if (y < techoInicio)
                    {
                        // Techo con estrellas
                        double techoD = 1.0 - ((double)y / (_altoVista / 2));
                        _buffer[y, x] = techoD < 0.3 ? '.' : ' ';
                    }
                    else if (y >= techoInicio && y < pisoInicio)
                    {
                        _buffer[y, x] = ObtenerCaracterMuro(distancia, tipoMuro);
                    }
                    else
                    {
                        // Piso con textura
                        double pisoD = 1.0 - ((double)(y - _altoVista / 2) / (_altoVista / 2));
                        if (pisoD < 0.15) _buffer[y, x] = '#';
                        else if (pisoD < 0.3) _buffer[y, x] = 'x';
                        else if (pisoD < 0.5) _buffer[y, x] = '-';
                        else if (pisoD < 0.7) _buffer[y, x] = '.';
                        else _buffer[y, x] = ' ';
                    }
                }
            }

            // Renderizar enemigos
            RenderizarEnemigos();

            // Crosshair (mira)
            int centroX = _anchoVista / 2;
            int centroY = _altoVista / 2;
            if (centroY >= 0 && centroY < _altoVista)
            {
                if (centroX - 2 >= 0) _buffer[centroY, centroX - 2] = '-';
                if (centroX - 1 >= 0) _buffer[centroY, centroX - 1] = '-';
                _buffer[centroY, centroX] = '+';
                if (centroX + 1 < _anchoVista) _buffer[centroY, centroX + 1] = '-';
                if (centroX + 2 < _anchoVista) _buffer[centroY, centroX + 2] = '-';
                if (centroY - 1 >= 0) _buffer[centroY - 1, centroX] = '|';
                if (centroY + 1 < _altoVista) _buffer[centroY + 1, centroX] = '|';
            }

            // Dibujar arma en el buffer
            DibujarArmaEnBuffer();

            // Flash de disparo
            if (_flashDisparo > 0)
                AplicarFlashDisparo();

            // Flash de daño
            if (_flashDano > 0)
                AplicarFlashDano();

            // Construir salida
            Console.SetCursorPosition(0, 0);
            var sb = new StringBuilder();

            for (int y = 0; y < _altoVista; y++)
            {
                for (int x = 0; x < _anchoVista; x++)
                    sb.Append(_buffer[y, x]);
                sb.AppendLine();
            }

            // HUD
            DibujarHUD(sb);

            Console.Write(sb.ToString());
        }

        // ========================================
        //         ARMA ASCII EN PANTALLA
        // ========================================

        private void DibujarArmaEnBuffer()
        {
            string[] arma;

            if (_flashDisparo > 3)
            {
                // Muzzle flash - DISPARANDO
                arma = new string[]
                {
                    "          \\\\|//          ",
                    "          -***-          ",
                    "          //|\\\\          ",
                    "           ||            ",
                    "        ___||___         ",
                    "       |  ____  |        ",
                    "       | |    | |        ",
                    "       | |____| |        ",
                    "       |________|        ",
                    "          /  \\           ",
                };
            }
            else if (_recargando && _animRecarga > 0)
            {
                // Animación de recarga
                if (_animRecarga > 15)
                {
                    arma = new string[]
                    {
                        "                         ",
                        "                         ",
                        "                         ",
                        "        ___  ___         ",
                        "       |  ____  |        ",
                        "       | |    |/         ",
                        "       | |____/          ",
                        "       |_______          ",
                        "          /  \\           ",
                        "    [RECARGANDO...]      ",
                    };
                }
                else
                {
                    arma = new string[]
                    {
                        "                         ",
                        "                         ",
                        "                         ",
                        "           ||            ",
                        "        ___||___         ",
                        "       |  ____  |  *clic*",
                        "       | |    | |        ",
                        "       | |____| |        ",
                        "       |________|        ",
                        "          /  \\           ",
                    };
                }
            }
            else
            {
                // Arma normal - idle con ligero movimiento
                if (_frameArma % 40 < 20)
                {
                    arma = new string[]
                    {
                        "                         ",
                        "                         ",
                        "                         ",
                        "           ||            ",
                        "        ___||___         ",
                        "       |  ____  |        ",
                        "       | |    | |        ",
                        "       | |____| |        ",
                        "       |________|        ",
                        "          /  \\           ",
                    };
                }
                else
                {
                    arma = new string[]
                    {
                        "                         ",
                        "                         ",
                        "                         ",
                        "           ||            ",
                        "        ___||___         ",
                        "       |  ____  |        ",
                        "       | |    | |        ",
                        "       | |____| |        ",
                        "        |______|         ",
                        "          /  \\           ",
                    };
                }
            }

            // Posicionar arma en la parte inferior central
            int armaX = (_anchoVista / 2) - 12;
            int armaY = _altoVista - arma.Length;

            for (int y = 0; y < arma.Length; y++)
            {
                for (int x = 0; x < arma[y].Length; x++)
                {
                    int bx = armaX + x;
                    int by = armaY + y;
                    if (bx >= 0 && bx < _anchoVista && by >= 0 && by < _altoVista)
                    {
                        if (arma[y][x] != ' ')
                            _buffer[by, bx] = arma[y][x];
                    }
                }
            }

            _frameArma++;
        }

        // ========================================
        //         SPRITES DE ENEMIGOS
        // ========================================

        private void RenderizarEnemigos()
        {
            // Ordenar enemigos por distancia (lejanos primero)
            var enemigosOrdenados = new List<(EnemigoDoom e, double dist)>();
            foreach (var enemigo in Enemigos)
            {
                if (!enemigo.EstaVivo) continue;
                double dist = enemigo.DistanciaA(Player.X, Player.Y);
                enemigosOrdenados.Add((enemigo, dist));
            }
            enemigosOrdenados.Sort((a, b) => b.dist.CompareTo(a.dist));

            foreach (var (enemigo, distancia) in enemigosOrdenados)
            {
                double dx = enemigo.X - Player.X;
                double dy = enemigo.Y - Player.Y;

                double anguloEnemigo = Math.Atan2(dy, dx);
                double anguloRelativo = anguloEnemigo - Player.Angulo;

                while (anguloRelativo > Math.PI) anguloRelativo -= 2 * Math.PI;
                while (anguloRelativo < -Math.PI) anguloRelativo += 2 * Math.PI;

                if (Math.Abs(anguloRelativo) < _fov / 2 && distancia < _profundidadMax)
                {
                    int columna = (int)((_anchoVista / 2) + (anguloRelativo / _fov) * _anchoVista);

                    // Verificar que no esté detrás de un muro
                    if (columna >= 0 && columna < _anchoVista && distancia < _profundidadBuffer[columna])
                    {
                        string[] sprite = ObtenerSpriteEnemigo(enemigo, distancia);
                        int spriteAltura = sprite.Length;
                        int spriteAncho = sprite[0].Length;

                        int centroY = _altoVista / 2;
                        int escala = (int)(6.0 / distancia);
                        int startY = centroY - escala;

                        for (int sy = 0; sy < spriteAltura; sy++)
                        {
                            int by = startY + sy;
                            if (by < 0 || by >= _altoVista) continue;

                            for (int sx = 0; sx < spriteAncho; sx++)
                            {
                                int bx = columna - (spriteAncho / 2) + sx;
                                if (bx < 0 || bx >= _anchoVista) continue;

                                if (sprite[sy][sx] != ' ')
                                    _buffer[by, bx] = sprite[sy][sx];
                            }
                        }
                    }
                }
            }
        }

        private string[] ObtenerSpriteEnemigo(EnemigoDoom enemigo, double distancia)
        {
            // Sprites diferentes según tipo y distancia
            if (distancia < 3) // MUY CERCA
            {
                return enemigo.Tipo switch
                {
                    "Zombi" => new string[]
                    {
                        " ,---, ",
                        " |o.o| ",
                        " |___| ",
                        "  /|\\ ",
                        " / | \\",
                        "  / \\  ",
                    },
                    "Demonio" => new string[]
                    {
                        " /vvv\\ ",
                        " {O O} ",
                        " |/\\/\\|",
                        "  /|\\  ",
                        " //|\\\\ ",
                        "  / \\  ",
                    },
                    "Cacodemon" => new string[]
                    {
                        " /----\\ ",
                        "| O  O |",
                        "|  \\/  |",
                        "| \\~~/ |",
                        " \\----/ ",
                    },
                    "Baron" => new string[]
                    {
                        " /VVVV\\ ",
                        " |@  @| ",
                        " | /\\ | ",
                        " |/\\/\\| ",
                        "  /||\\ ",
                        " //||\\\\ ",
                        "  /  \\  ",
                    },
                    _ => new string[] { " <E> " }
                };
            }
            else if (distancia < 6) // MEDIO
            {
                return enemigo.Tipo switch
                {
                    "Zombi" => new string[] { " ,-,", " |o|", " /|\\", " / \\" },
                    "Demonio" => new string[] { " /v\\", " {O}", " /|\\", " / \\" },
                    "Cacodemon" => new string[] { " /--\\", "| OO|", " \\--/" },
                    "Baron" => new string[] { " /VV\\", " |@@|", " /||\\", " /  \\" },
                    _ => new string[] { " <E>" }
                };
            }
            else if (distancia < 10) // LEJOS
            {
                return enemigo.Tipo switch
                {
                    "Zombi" => new string[] { " Z ", "/|\\" },
                    "Demonio" => new string[] { " D ", "/|\\" },
                    "Cacodemon" => new string[] { "(C)" },
                    "Baron" => new string[] { " B ", "/|\\" },
                    _ => new string[] { " E " }
                };
            }
            else // MUY LEJOS
            {
                return new string[] { enemigo.Simbolo.ToString() };
            }
        }

        // ========================================
        //         EFECTOS VISUALES
        // ========================================

        private void AplicarFlashDisparo()
        {
            // Flash amarillo alrededor de la mira
            int cx = _anchoVista / 2;
            int cy = _altoVista / 2;

            for (int y = cy - 3; y <= cy + 3; y++)
            {
                for (int x = cx - 4; x <= cx + 4; x++)
                {
                    if (y >= 0 && y < _altoVista && x >= 0 && x < _anchoVista)
                    {
                        if (_buffer[y, x] == ' ')
                            _buffer[y, x] = '.';
                    }
                }
            }
        }

        private void AplicarFlashDano()
        {
            // Efecto rojo en los bordes cuando recibes daño
            for (int y = 0; y < _altoVista; y++)
            {
                if (y < 2 || y >= _altoVista - 2)
                {
                    for (int x = 0; x < _anchoVista; x++)
                        _buffer[y, x] = '!';
                }
                else
                {
                    if (0 < _anchoVista) _buffer[y, 0] = '!';
                    if (1 < _anchoVista) _buffer[y, 1] = '!';
                    if (_anchoVista - 1 >= 0) _buffer[y, _anchoVista - 1] = '!';
                    if (_anchoVista - 2 >= 0) _buffer[y, _anchoVista - 2] = '!';
                }
            }
        }

        private void ActualizarEfectos()
        {
            if (_flashDisparo > 0) _flashDisparo--;
            if (_flashDano > 0) _flashDano--;
            if (_mensajeKillTimer > 0) _mensajeKillTimer--;

            if (_recargando)
            {
                _animRecarga--;
                if (_animRecarga <= 0)
                {
                    _recargando = false;
                    Player.Municion += 20;
                }
            }
        }

        // ========================================
        //            HUD COMPLETO
        // ========================================

        private void DibujarHUD(StringBuilder sb)
        {
            int enemigosVivos = 0;
            foreach (var e in Enemigos)
                if (e.EstaVivo) enemigosVivos++;

            // Barra separadora
            sb.AppendLine("╔══════════════════════════════════════════════════════════════════════════════╗");

            // Barra de vida
            int barraVidaLen = 20;
            int vidaLlena = (int)((double)Player.Vida / Player.VidaMaxima * barraVidaLen);
            string vidaChar = new string('█', Math.Max(0, vidaLlena));
            string vidaVacia = new string('░', barraVidaLen - vidaChar.Length);

            // Barra de munición
            int barraMuniLen = 10;
            int muniLlena = Math.Min(barraMuniLen, Player.Municion / 5);
            string muniChar = new string('█', Math.Max(0, muniLlena));
            string muniVacia = new string('░', barraMuniLen - muniChar.Length);

            sb.AppendFormat("║ VIDA [{0}{1}] {2,3}/100  MUNI [{3}{4}] {5,2}  PTS: {6,5}  NIVEL: {7}  ENEMIGOS: {8} ║",
                vidaChar, vidaVacia, Player.Vida,
                muniChar, muniVacia, Player.Municion,
                Player.Puntos, _nivelActual, enemigosVivos);
            sb.AppendLine();

            // Arma actual y estado
            string estadoArma = _recargando ? "RECARGANDO..." : Player.ArmaActual;
            sb.AppendFormat("║ ARMA: {0,-15} W/S=Mover  A/D=Rotar  Q/E=Strafe  ESPACIO=Disparar  R=Recargar ║", estadoArma);
            sb.AppendLine();

            // Mensaje de kill
            if (_mensajeKillTimer > 0 && _mensajeKill.Length > 0)
            {
                sb.AppendFormat("║ >>> {0,-72} ║", _mensajeKill);
                sb.AppendLine();
            }

            sb.AppendLine("╚══════════════════════════════════════════════════════════════════════════════╝");
        }

        // ========================================
        //         MUROS CON TEXTURA
        // ========================================

        private double LanzarRayo(double angulo, out int tipoMuro)
        {
            tipoMuro = 1;
            double rayX = Player.X;
            double rayY = Player.Y;
            double stepX = Math.Cos(angulo) * 0.05;
            double stepY = Math.Sin(angulo) * 0.05;

            for (double d = 0; d < _profundidadMax; d += 0.05)
            {
                rayX += stepX;
                rayY += stepY;

                int mapX = (int)rayX;
                int mapY = (int)rayY;

                if (Mapa.EsMuro(mapX, mapY))
                {
                    tipoMuro = Mapa.ObtenerCelda(mapX, mapY);
                    return d;
                }
            }
            return _profundidadMax;
        }

        private char ObtenerCaracterMuro(double distancia, int tipoMuro)
        {
            if (tipoMuro == 1) // Ladrillo
            {
                if (distancia < 1.5) return '\u2588';  // █
                if (distancia < 3) return '\u2593';  // ▓
                if (distancia < 5) return '\u2592';  // ▒
                if (distancia < 7) return '\u2591';  // ░
                if (distancia < 10) return ':';
                return '.';
            }
            else if (tipoMuro == 2) // Piedra
            {
                if (distancia < 1.5) return '%';
                if (distancia < 3) return '&';
                if (distancia < 5) return '#';
                if (distancia < 7) return '+';
                if (distancia < 10) return ':';
                return '.';
            }
            else if (tipoMuro == 3) // Metal
            {
                if (distancia < 1.5) return '=';
                if (distancia < 3) return '|';
                if (distancia < 5) return 'I';
                if (distancia < 7) return '!';
                if (distancia < 10) return ':';
                return '.';
            }
            else // Puerta
            {
                if (distancia < 2) return '[';
                if (distancia < 4) return '+';
                if (distancia < 6) return '/';
                return '.';
            }
        }

        // ========================================
        //         ENTRADA DEL JUGADOR
        // ========================================

        private void ProcesarEntrada()
        {
            if (!Console.KeyAvailable) return;
            var tecla = Console.ReadKey(true).Key;

            if (_recargando && tecla != ConsoleKey.X) return;

            switch (tecla)
            {
                case ConsoleKey.W:
                    Player.MoverAdelante(Mapa);
                    break;
                case ConsoleKey.S:
                    Player.MoverAtras(Mapa);
                    break;
                case ConsoleKey.A:
                    Player.RotarIzquierda();
                    break;
                case ConsoleKey.D:
                    Player.RotarDerecha();
                    break;
                case ConsoleKey.Q:
                    Player.MoverIzquierda(Mapa);
                    break;
                case ConsoleKey.E:
                    Player.MoverDerecha(Mapa);
                    break;
                case ConsoleKey.Spacebar:
                    IntentarDisparar();
                    break;
                case ConsoleKey.R:
                    Recargar();
                    break;
                case ConsoleKey.X:
                    _juegoActivo = false;
                    break;
            }
        }

        private void IntentarDisparar()
        {
            if (_recargando) return;
            if (!Player.Disparar())
            {
                // Sin munición - auto recargar
                Recargar();
                return;
            }

            _flashDisparo = 6;

            EnemigoDoom enemigoMasCercano = null;
            double menorDistancia = _profundidadMax;

            foreach (var enemigo in Enemigos)
            {
                if (!enemigo.EstaVivo) continue;

                double dx = enemigo.X - Player.X;
                double dy = enemigo.Y - Player.Y;
                double distancia = Math.Sqrt(dx * dx + dy * dy);

                double anguloEnemigo = Math.Atan2(dy, dx);
                double anguloRelativo = anguloEnemigo - Player.Angulo;

                while (anguloRelativo > Math.PI) anguloRelativo -= 2 * Math.PI;
                while (anguloRelativo < -Math.PI) anguloRelativo += 2 * Math.PI;

                if (Math.Abs(anguloRelativo) < 0.2 && distancia < menorDistancia)
                {
                    menorDistancia = distancia;
                    enemigoMasCercano = enemigo;
                }
            }

            if (enemigoMasCercano != null)
            {
                int dano = (int)(30 / menorDistancia * 3);
                enemigoMasCercano.RecibirDano(dano);

                if (!enemigoMasCercano.EstaVivo)
                {
                    Player.Puntos += enemigoMasCercano.PuntosAlMorir;
                    _totalEnemigosEliminados++;
                    _mensajeKill = string.Format("ELIMINASTE A {0}! (+{1} pts)",
                        enemigoMasCercano.Tipo.ToUpper(), enemigoMasCercano.PuntosAlMorir);
                    _mensajeKillTimer = 30;
                }
            }
        }

        private void Recargar()
        {
            if (_recargando) return;
            _recargando = true;
            _animRecarga = 25;
        }

        // ========================================
        //         ACTUALIZAR ENEMIGOS
        // ========================================

        private void ActualizarEnemigos()
        {
            foreach (var enemigo in Enemigos)
            {
                if (!enemigo.EstaVivo) continue;

                enemigo.Perseguir(Player.X, Player.Y, Mapa);

                if (enemigo.PuedeAtacar(Player.X, Player.Y))
                {
                    Player.RecibirDano(enemigo.Dano / 10);
                    _flashDano = 4;
                }
            }
        }

        private void VerificarEstado()
        {
            bool todosEliminados = true;
            foreach (var e in Enemigos)
            {
                if (e.EstaVivo) { todosEliminados = false; break; }
            }

            if (todosEliminados)
            {
                _nivelActual++;
                if (_nivelActual > 2)
                {
                    _ganado = true;
                    _juegoActivo = false;
                }
                else
                {
                    Player.Municion += 30;
                    Player.Curar(30);

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n\n");
                    Console.WriteLine("  ╔═══════════════════════════════════════╗");
                    Console.WriteLine("  ║        NIVEL {0} COMPLETADO!           ║", _nivelActual - 1);
                    Console.WriteLine("  ║     +30 MUNICION    +30 VIDA          ║");
                    Console.WriteLine("  ║     Preparate para el NIVEL {0}        ║", _nivelActual);
                    Console.WriteLine("  ╚═══════════════════════════════════════╝");
                    Console.ResetColor();
                    Thread.Sleep(3000);

                    InicializarNivel(_nivelActual);
                    MostrarPantallaInicio();
                }
            }
        }

        // ========================================
        //         ANIMACION DE MUERTE
        // ========================================

        private void AnimacionMuerte()
        {
            for (int i = 0; i < _altoVista; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(new string('X', _anchoVista));
                Console.ResetColor();
                Thread.Sleep(80);
            }

            Thread.Sleep(500);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
 
  ██    ██  ██████  ██    ██     ██████  ██ ███████ ██████  
   ██  ██  ██    ██ ██    ██     ██   ██ ██ ██      ██   ██ 
    ████   ██    ██ ██    ██     ██   ██ ██ █████   ██   ██ 
     ██    ██    ██ ██    ██     ██   ██ ██ ██      ██   ██ 
     ██     ██████   ██████      ██████  ██ ███████ ██████  
 
                                                            ");
            Console.ResetColor();
            Thread.Sleep(2000);
        }

        // ========================================
        //         PANTALLA FINAL
        // ========================================

        private void MostrarPantallaFinal()
        {
            Console.Clear();

            if (_ganado)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(@"
  ╔═══════════════════════════════════════════════════╗
  ║                                                   ║
  ║        ██    ██ ██  ██████ ████████  ██████        ║
  ║        ██    ██ ██ ██         ██    ██    ██       ║
  ║        ██    ██ ██ ██         ██    ██    ██       ║
  ║         ██  ██  ██ ██         ██    ██    ██       ║
  ║          ████   ██  ██████    ██     ██████        ║
  ║                                                   ║
  ║            COMPLETASTE TODOS LOS NIVELES          ║
  ║                                                   ║
  ╚═══════════════════════════════════════════════════╝");
            }
            else if (!Player.EstaVivo())
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(@"
  ╔═══════════════════════════════════════════════════╗
  ║                                                   ║
  ║                   GAME  OVER                      ║
  ║                                                   ║
  ╚═══════════════════════════════════════════════════╝");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n  Has salido del juego.");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n  ═══════════ ESTADISTICAS ═══════════");
            Console.WriteLine("  Puntos totales:       {0}", Player.Puntos);
            Console.WriteLine("  Enemigos eliminados:  {0}", _totalEnemigosEliminados);
            Console.WriteLine("  Nivel alcanzado:      {0}", _nivelActual);
            Console.WriteLine("  Vida restante:        {0}/100", Player.Vida);
            Console.WriteLine("  Municion restante:    {0}", Player.Municion);
            Console.WriteLine("  ═════════════════════════════════════");
            Console.ResetColor();
            Console.WriteLine("\n  Presiona una tecla para volver al menu...");
            Console.ReadKey(true);
        }

        // Implementación de iMotorJuego
        public void Avanzar() { }
        public void CambiarDireccion(ConsoleKey tecla) { }
        public bool Ganado() => _ganado;
        public bool Perdido() => !Player.EstaVivo();
    }
}