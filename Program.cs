using System.Text;
using Rdg.Bl;
using Rdg.Dal;

Console.OutputEncoding = Encoding.UTF8;

Uloziste.Inicializuj(Path.Combine(AppContext.BaseDirectory, "data"));

var sluzba = new ObjednavkaSluzba();

Console.WriteLine("=== Row Data Gateway (RDG) ===");
Console.WriteLine();
Console.WriteLine("Dostupné materiály:");

foreach (var material in sluzba.Materialy())
{
    Console.WriteLine($"  {material.Id}: {material.Nazev}");
}

ObjednavkaRow? ulozena = null;

while (ulozena is null)
{
    Console.WriteLine();

    var zakaznik = new ZakaznikRow
    {
        Jmeno = Cti("Jméno zákazníka: "),
        Email = Cti("E-mail: "),
        Ulice = Cti("Ulice: "),
        Mesto = Cti("Město: "),
        Psc = Cti("PSČ: ")
    };

    var objednavka = new ObjednavkaRow
    {
        MaterialId = CtiCislo("ID materiálu: "),
        Sirka = CtiCislo("Šířka (mm): "),
        Vyska = CtiCislo("Výška (mm): "),
        Hloubka = CtiCislo("Hloubka (mm): "),
        Poznamka = Cti("Poznámka: ")
    };

    try
    {
        ulozena = sluzba.VytvorObjednavku(zakaznik, objednavka);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Chyba: {ex.Message} Zadejte údaje znovu.");
    }
}

Console.WriteLine();
Console.WriteLine($"Objednávka uložena. ID: {ulozena.Id}, stav: {ulozena.Stav}");

if (Cti("Potvrdit objednávku? (a/n): ").ToLowerInvariant() is "a" or "ano")
{
    ulozena = sluzba.Potvrd(ulozena.Id);
    Console.WriteLine($"Objednávka {ulozena.Id} je ve stavu {ulozena.Stav}.");
}

Console.WriteLine();
Console.WriteLine("=== Ověření ===");
Console.WriteLine("Find:");
Vypis(ObjednavkaRow.Find(ulozena.Id)!);
Console.WriteLine("FindAll:");

foreach (var objednavka in ObjednavkaRow.FindAll())
{
    Vypis(objednavka);
}

static string Cti(string vyzva)
{
    Console.Write(vyzva);
    return (Console.ReadLine() ?? throw new EndOfStreamException()).Trim();
}

static int CtiCislo(string vyzva)
{
    while (true)
    {
        if (int.TryParse(Cti(vyzva), out var cislo))
        {
            return cislo;
        }

        Console.WriteLine("Chyba: zadejte celé číslo.");
    }
}

static void Vypis(ObjednavkaRow o)
{
    Console.WriteLine($"  [{o.Id}] zákazník {o.ZakaznikId} | materiál {o.MaterialId} | {o.Sirka} x {o.Vyska} x {o.Hloubka} mm | {o.Stav} | {o.Poznamka}");
}
