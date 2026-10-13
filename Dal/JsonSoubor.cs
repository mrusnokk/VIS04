using System.Text.Encodings.Web;
using System.Text.Json;

namespace Rdg.Dal;

public class JsonSoubor<T>
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private readonly string cesta;

    public JsonSoubor(string cesta)
    {
        this.cesta = cesta;
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(cesta))!);
    }

    public List<T> Nacti()
    {
        if (!File.Exists(cesta))
        {
            return new List<T>();
        }

        return JsonSerializer.Deserialize<List<T>>(File.ReadAllText(cesta), Options) ?? new List<T>();
    }

    public void Uloz(List<T> polozky)
    {
        File.WriteAllText(cesta, JsonSerializer.Serialize(polozky, Options));
    }
}
