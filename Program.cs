var categoriasTemp = new Ahorcado.PalabrasEnMemoria("POO");
var categorias = categoriasTemp.ObtenerCategorias();
string categoriaElegida = Ahorcado.ConsolaUI.PedirCategoria(categorias);

var repositorio = new Ahorcado.PalabrasEnMemoria(categoriaElegida);
var motor = new Ahorcado.MotorAhorcado(repositorio);
var ui = new Ahorcado.ConsolaUI(motor);

Console.WriteLine($"=== AHORCADO - {categoriaElegida} ===");

while (!motor.Ganado() && !motor.Perdido())
{
    ui.MostrarTablero();

    char letra = ui.PedirLetra();

    if (motor.LetraYaUsada(letra))
    {
        ui.MostrarMensaje("Ya usaste esa letra.");
        continue;
    }

    motor.RegistrarLetra(letra);
}

ui.MostrarTablero();

if (motor.Ganado())
{
    ui.MostrarMensaje($"\n¡Ganaste! La palabra era: {motor.PalabraSecreta}");
}
else
{
    ui.MostrarMensaje($"\nPerdiste. La palabra era: {motor.PalabraSecreta}");
}

if (ui.PreguntarOtraVez())
{
    var nuevoRepositorio = new Ahorcado.PalabrasEnMemoria(categoriaElegida);
    var nuevoMotor = new Ahorcado.MotorAhorcado(nuevoRepositorio);
    var nuevaUI = new Ahorcado.ConsolaUI(nuevoMotor);
}