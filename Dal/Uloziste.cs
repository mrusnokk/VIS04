namespace Rdg.Dal;

public static class Uloziste
{
    public static JsonSoubor<ZakaznikRow> Zakaznici { get; private set; } = null!;
    public static JsonSoubor<MaterialRow> Materialy { get; private set; } = null!;
    public static JsonSoubor<ObjednavkaRow> Objednavky { get; private set; } = null!;

    public static void Inicializuj(string adresar)
    {
        Zakaznici = new JsonSoubor<ZakaznikRow>(Path.Combine(adresar, "zakaznici.json"));
        Materialy = new JsonSoubor<MaterialRow>(Path.Combine(adresar, "materialy.json"));
        Objednavky = new JsonSoubor<ObjednavkaRow>(Path.Combine(adresar, "objednavky.json"));

        if (Materialy.Nacti().Count == 0)
        {
            Materialy.Uloz(new List<MaterialRow>
            {
                new() { Id = 1, Nazev = "Dub" },
                new() { Id = 2, Nazev = "Smrk" },
                new() { Id = 3, Nazev = "Kov" },
                new() { Id = 4, Nazev = "Sklo" }
            });
        }
    }
}
