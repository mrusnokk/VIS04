using Rdg.Dal;

namespace Rdg.Bl;

public class ObjednavkaSluzba
{
    public List<MaterialRow> Materialy()
    {
        return MaterialRow.FindAll();
    }

    public ObjednavkaRow VytvorObjednavku(ZakaznikRow zakaznik, ObjednavkaRow objednavka)
    {
        zakaznik.Validate();
        objednavka.Validate();

        var existujici = ZakaznikRow.FindByEmail(zakaznik.Email);

        if (existujici is null)
        {
            zakaznik.Insert();
            existujici = zakaznik;
        }

        objednavka.ZakaznikId = existujici.Id;
        objednavka.Stav = Stav.CekaNaPotvrzeni;
        objednavka.VytvorenoAt = DateTime.UtcNow;
        objednavka.Insert();

        return objednavka;
    }

    public ObjednavkaRow Potvrd(int id)
    {
        var objednavka = ObjednavkaRow.Find(id)
            ?? throw new ArgumentException($"Objednávka {id} neexistuje.");

        if (objednavka.Stav != Stav.CekaNaPotvrzeni)
        {
            throw new InvalidOperationException($"Potvrdit lze jen objednávku ve stavu {Stav.CekaNaPotvrzeni}.");
        }

        objednavka.Stav = Stav.Potvrzena;
        objednavka.Update();

        return objednavka;
    }
}
