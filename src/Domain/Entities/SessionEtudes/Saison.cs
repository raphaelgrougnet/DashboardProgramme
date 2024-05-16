namespace Domain.Entities.SessionEtudes;

public enum Saison
{
    Automne,
    Hiver,
    Ete
}

public static class SaisonExt
{
    public static int ComparerAvec(this Saison saisonA, Saison saisonB)
    {
        return saisonA.GetOrdre().CompareTo(saisonB.GetOrdre());
    }

    public static byte GetOrdre(this Saison saison)
    {
        return saison switch
        {
            Saison.Hiver => 0,
            Saison.Ete => 1,
            _ => 2
        };
    }
}