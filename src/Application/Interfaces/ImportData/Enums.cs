using System.Runtime.Serialization;

using Domain.Entities.Etudiants;

namespace Application.Interfaces.ImportData;

public enum RecordsSpe
{
    _1 = 1,
    _2 = 2,
    _3 = 3,
    _4 = 4,
    _5 = 5,
    _6 = 6,
    _7 = 7,
    _8 = 8
}

public static class SpeExt
{
    public static byte ToByte(this RecordsSpe value)
    {
        return (byte)value;
    }
}

public enum RecordsTourDadmission
{
    _1 = 1,
    _2 = 2,
    _3 = 3,
    _4 = 4,
    _5 = 5
}

public static class ToursDadmissionExt
{
    public static byte ToByte(this RecordsTourDadmission value)
    {
        return (byte)value;
    }
}

public enum RecordsPopulation
{
    [EnumMember(Value = @"A")] A = 0,
    [EnumMember(Value = @"B")] B = 1,
    [EnumMember(Value = @"I")] I = 2
}

public static class PopulationExt
{
    public static Population ToPopulation(this RecordsPopulation value)
    {
        return value switch
        {
            RecordsPopulation.A => Population.NouvelEtudiant,
            RecordsPopulation.B => Population.ParcoursCollegial,
            RecordsPopulation.I => Population.International,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}

public enum RecordsSanctionCollegiale
{
    [EnumMember(Value = @"Non")] Non = 0,
    [EnumMember(Value = @"non")] NonMin = Non,

    [EnumMember(Value = @"DEC préuniversitaire")]
    DecPreuniversitaire = 2,
    [EnumMember(Value = @"DEC technique")] DecTechnique = 3
}

public static class SanctionExt
{
    public static Sanction ToSanction(this RecordsSanctionCollegiale value)
    {
        return value switch
        {
            RecordsSanctionCollegiale.Non => Sanction.Aucune,
            RecordsSanctionCollegiale.DecPreuniversitaire => Sanction.DecPreUniversitaire,
            RecordsSanctionCollegiale.DecTechnique => Sanction.DecTechnique,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}

public enum RecordsEtudiantInternational
{
    [EnumMember(Value = @"CC")] Cc = 0,
    [EnumMember(Value = @"RP")] Rp = 1,
    [EnumMember(Value = @"RT")] Rt = 2
}

public static class EtudiantInternationalExt
{
    public static StatutImmigration ToStatutImmigration(this RecordsEtudiantInternational value)
    {
        return value switch
        {
            RecordsEtudiantInternational.Cc => StatutImmigration.CitoyenCanadien,
            RecordsEtudiantInternational.Rp => StatutImmigration.ResidentPermanent,
            RecordsEtudiantInternational.Rt => StatutImmigration.ResidentTemporaire,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}

public enum OuiNon
{
    [EnumMember(Value = @"Oui")] Oui = 0,
    [EnumMember(Value = @"Non")] Non = 1,
    [EnumMember(Value = @"oui")] OuiMin = Oui,
    [EnumMember(Value = @"non")] NonMin = Non,
    [EnumMember(Value = @"En Cours")] EnCours = Non,
    [EnumMember(Value = @"en Cours")] enCours = Non
}

public static class OuiNonExt
{
    public static bool ToBool(this OuiNon value)
    {
        return value == OuiNon.Oui;
    }
}