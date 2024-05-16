namespace Application.Interfaces.ImportData;

public interface IImportDataRow
{
    OuiNon _436 { get; set; }
    OuiNon _514 { get; set; }
    OuiNon _526 { get; set; }
    OuiNon _536 { get; set; }
    string CodePermanentCrypte { get; set; }
    string Grille { get; set; }
    RecordsSpe Spe { get; set; }
    string NumeroDeProgramme { get; set; }
    string TitreProgramme { get; set; }
    ushort NombreDheuresDeCoursDansLaSession { get; set; }
    RecordsTourDadmission TourDadmission { get; set; }
    RecordsPopulation Population { get; set; }
    RecordsSanctionCollegiale SanctionCollegiale { get; set; }
    float Genmels { get; set; }
    OuiNon Sn4 { get; set; }
    OuiNon TsSn4 { get; set; }
    OuiNon Ts4Sn4Plus { get; set; }
    OuiNon Cst5 { get; set; }
    OuiNon TsSn5 { get; set; }
    OuiNon Ts5 { get; set; }
    OuiNon _514plus { get; set; }
    OuiNon _526plus { get; set; }
    OuiNon RenforcementEnFrançais { get; set; }
    RecordsEtudiantInternational EtudiantInternational { get; set; }
    OuiNon EtudiantAssujettiAuR18 { get; set; }
    OuiNon ServicesAdaptes { get; set; }
    string CoursInscritsActuellement { get; set; }
    string CoursInscritsSessionPasse { get; set; }
    IDictionary<string, object> AdditionalProperties { get; set; }
}