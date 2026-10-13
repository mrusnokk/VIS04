namespace Rdg.Dal;

public class ObjednavkaRow
{
    public int Id { get; set; }
    public int ZakaznikId { get; set; }
    public int MaterialId { get; set; }
    public int Sirka { get; set; }
    public int Vyska { get; set; }
    public int Hloubka { get; set; }
    public string Poznamka { get; set; } = string.Empty;
    public string Stav { get; set; } = string.Empty;
    public DateTime VytvorenoAt { get; set; }

    public void Validate()
    {
        if (Sirka <= 0 || Vyska <= 0 || Hloubka <= 0)
        {
            throw new ArgumentException("Rozměry musí být větší než 0.");
        }

        if (MaterialRow.Find(MaterialId) is null)
        {
            throw new ArgumentException($"Materiál s ID {MaterialId} neexistuje.");
        }
    }

    public void Insert()
    {
        Validate();

        var polozky = Uloziste.Objednavky.Nacti();
        Id = polozky.Count == 0 ? 1 : polozky.Max(x => x.Id) + 1;
        polozky.Add(this);
        Uloziste.Objednavky.Uloz(polozky);
    }

    public void Update()
    {
        Validate();

        var polozky = Uloziste.Objednavky.Nacti();
        var index = polozky.FindIndex(x => x.Id == Id);

        if (index < 0)
        {
            throw new InvalidOperationException($"Objednávka {Id} neexistuje.");
        }

        polozky[index] = this;
        Uloziste.Objednavky.Uloz(polozky);
    }

    public void Delete()
    {
        var polozky = Uloziste.Objednavky.Nacti();
        polozky.RemoveAll(x => x.Id == Id);
        Uloziste.Objednavky.Uloz(polozky);
    }

    public static ObjednavkaRow? Find(int id)
    {
        return Uloziste.Objednavky.Nacti().FirstOrDefault(x => x.Id == id);
    }

    public static List<ObjednavkaRow> FindAll()
    {
        return Uloziste.Objednavky.Nacti();
    }
}
