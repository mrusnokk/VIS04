namespace Rdg.Dal;

public class MaterialRow
{
    public int Id { get; set; }
    public string Nazev { get; set; } = string.Empty;

    public void Insert()
    {
        var polozky = Uloziste.Materialy.Nacti();
        Id = polozky.Count == 0 ? 1 : polozky.Max(x => x.Id) + 1;
        polozky.Add(this);
        Uloziste.Materialy.Uloz(polozky);
    }

    public void Update()
    {
        var polozky = Uloziste.Materialy.Nacti();
        var index = polozky.FindIndex(x => x.Id == Id);

        if (index < 0)
        {
            throw new InvalidOperationException($"Materiál {Id} neexistuje.");
        }

        polozky[index] = this;
        Uloziste.Materialy.Uloz(polozky);
    }

    public void Delete()
    {
        var polozky = Uloziste.Materialy.Nacti();
        polozky.RemoveAll(x => x.Id == Id);
        Uloziste.Materialy.Uloz(polozky);
    }

    public static MaterialRow? Find(int id)
    {
        return Uloziste.Materialy.Nacti().FirstOrDefault(x => x.Id == id);
    }

    public static List<MaterialRow> FindAll()
    {
        return Uloziste.Materialy.Nacti();
    }
}
