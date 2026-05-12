namespace Ahorcado
{
    public class PalabrasEnMemoria : IRepositorioPalabras
    {
        private readonly Dictionary<string, List<string>> _categorias = new()
        {
            ["Arquitectura"] = new() { "arquitectura", "componente", "descomposicion", "dependencia", "interfaz", "acoplamiento", "middleware" },
            ["POO"] = new() { "polimorfismo", "herencia", "encapsulamiento", "abstraccion", "clase" },
            [".NET"] = new() { "ensamblado", "namespace", "delegado" }
        };

        private readonly List<string> _palabras;

        public PalabrasEnMemoria(string categoria)
        {
            _palabras = _categorias[categoria];
        }

        public List<string> ObtenerCategorias() => _categorias.Keys.ToList();

        public string ObtenerPalabraAleatoria()
        {
            var random = new Random();
            return _palabras[random.Next(_palabras.Count)];
        }
    }
}