namespace Rdg.Dal;

public class ZakaznikRow
{
    public int Id { get; set; }
    public string Jmeno { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Ulice { get; set; } = string.Empty;
    public string Mesto { get; set; } = string.Empty;
    public string Psc { get; set; } = string.Empty;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Jmeno))
        {
            throw new ArgumentException("Jméno zákazníka je povinné.");
        }

        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@'))
        {
            throw new ArgumentException("E-mail zákazníka není platný.");
        }

        if (string.IsNullOrWhiteSpace(Ulice) || string.IsNullOrWhiteSpace(Mesto) || string.IsNullOrWhiteSpace(Psc))
        {
            throw new ArgumentException("Adresa (ulice, město, PSČ) je povinná.");
        }
    }

    public void Insert()
    {
        Validate();

        var polozky = Uloziste.Zakaznici.Nacti();
        Id = polozky.Count == 0 ? 1 : polozky.Max(x => x.Id) + 1;
        polozky.Add(this);
        Uloziste.Zakaznici.Uloz(polozky);
    }

    public void Update()
    {
        Validate();

        var polozky = Uloziste.Zakaznici.Nacti();
        var index = polozky.FindIndex(x => x.Id == Id);

        if (index < 0)
        {
            throw new InvalidOperationException($"Zákazník {Id} neexistuje.");
        }

        polozky[index] = this;
        Uloziste.Zakaznici.Uloz(polozky);
    }

    public void Delete()
    {
        var polozky = Uloziste.Zakaznici.Nacti();
        polozky.RemoveAll(x => x.Id == Id);
        Uloziste.Zakaznici.Uloz(polozky);
    }

    public static ZakaznikRow? Find(int id)
    {
        return Uloziste.Zakaznici.Nacti().FirstOrDefault(x => x.Id == id);
    }

    public static ZakaznikRow? FindByEmail(string email)
    {
        return Uloziste.Zakaznici.Nacti().FirstOrDefault(x =>
            string.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase));
    }

    public static List<ZakaznikRow> FindAll()
    {
        return Uloziste.Zakaznici.Nacti();
    }
}
