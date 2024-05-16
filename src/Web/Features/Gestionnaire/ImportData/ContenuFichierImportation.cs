using System.ComponentModel.DataAnnotations;

using Application.Interfaces.ImportData;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Web.Features.Gestionnaire.ImportData;
#pragma warning disable // Disable all warnings

public class ConcreteListTypeConverter<TInterface, TConcrete> : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return new List<TInterface>((IEnumerable<TInterface>)serializer.Deserialize<List<TConcrete>>(reader));
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}

public class ContenuFichierImportation : IContenuFichierImportation
{
    private IDictionary<string, object>? _additionalProperties;

    [JsonConverter(typeof(ConcreteListTypeConverter<IImportDataRow, ImportDataRow>))]
    [JsonProperty("records", Required = Required.Always)]
    [Required]
    public List<IImportDataRow> Records { get; set; } = new();

    [JsonProperty("sheetName", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    public string SheetName { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties
    {
        get => _additionalProperties ??= new Dictionary<string, object>();
        set => _additionalProperties = value;
    }
}

public class ImportDataRow : IImportDataRow
{
    private IDictionary<string, object>? _additionalProperties;

    [JsonProperty("436", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public OuiNon _436 { get; set; }

    [JsonProperty("514", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public OuiNon _514 { get; set; }

    [JsonProperty("526", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public OuiNon _526 { get; set; }

    [JsonProperty("536", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public OuiNon _536 { get; set; }

    [JsonProperty("Code permanent crypté", Required = Required.Always)]
    [Required]
    [StringLength(64, MinimumLength = 64)]
    public string CodePermanentCrypte { get; set; }

    [JsonProperty("Grille", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [RegularExpression(@"^[0-9]{3}[A-Z][A-Z0-9]-2[0-9]{3}-[0-9]$")]
    public string Grille { get; set; }

    [JsonProperty("SPE", Required = Required.Always)]
    public RecordsSpe Spe { get; set; }

    [JsonProperty("Numéro de programme", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [RegularExpression(@"^[0-9]{3}[.][A-Z][A-Z0-9]$")]
    public string NumeroDeProgramme { get; set; }

    [JsonProperty("Titre programme", Required = Required.Always)]
    [Required]
    public string TitreProgramme { get; set; }

    [JsonProperty("Nombre d'heures de cours dans la session", Required = Required.Always)]
    public ushort NombreDheuresDeCoursDansLaSession { get; set; }

    [JsonProperty("Tour d'admission", Required = Required.Always)]
    public RecordsTourDadmission TourDadmission { get; set; }

    [JsonProperty("Population", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public RecordsPopulation Population { get; set; }

    [JsonProperty("Sanction collégiale", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public RecordsSanctionCollegiale SanctionCollegiale { get; set; }

    [JsonProperty("GENMELS", Required = Required.Always)]
    [Range(0D, 100D)]
    public float Genmels { get; set; }

    [JsonProperty("SN4", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public OuiNon Sn4 { get; set; }

    [JsonProperty("TS_SN4", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public OuiNon TsSn4 { get; set; }

    [JsonProperty("TS4_SN4+", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public OuiNon Ts4Sn4Plus { get; set; }

    [JsonProperty("CST5", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public OuiNon Cst5 { get; set; }

    [JsonProperty("TS_SN5", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public OuiNon TsSn5 { get; set; }

    [JsonProperty("TS5", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public OuiNon Ts5 { get; set; }

    [JsonProperty("514+", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public OuiNon _514plus { get; set; }

    [JsonProperty("526+", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public OuiNon _526plus { get; set; }

    [JsonProperty("Renforcement en français", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public OuiNon RenforcementEnFrançais { get; set; }

    [JsonProperty("Étudiant international", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public RecordsEtudiantInternational EtudiantInternational { get; set; }

    [JsonProperty("Étudiant assujetti au R18", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public OuiNon EtudiantAssujettiAuR18 { get; set; }

    [JsonProperty("Services adaptés", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [JsonConverter(typeof(StringEnumConverter))]
    public OuiNon ServicesAdaptes { get; set; }

    [JsonProperty("Cours inscrits actuellement", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [RegularExpression(@"(^\s*$)|(^\s*([0-9]{3}[A-Z0-9]{3}[A-Z]{2})(; ?[0-9]{3}[A-Z0-9]{3}[A-Z]{2})*\s*$)")]
    public string CoursInscritsActuellement { get; set; }

    [JsonProperty("Cours inscrits session passé", Required = Required.Always)]
    [Required(AllowEmptyStrings = true)]
    [RegularExpression(
        @"(^\s*$)|(^\s*aucune donnée\s*$)|(^\s*([0-9]{3}[A-Z0-9]{3}[A-Z]{2};([ABCDE]|Aucun résultat))(; ?[0-9]{3}[A-Z0-9]{3}[A-Z]{2};([ABCDE]|Aucun résultat))*\s*$)")]
    public string CoursInscritsSessionPasse { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties
    {
        get => _additionalProperties ??= new Dictionary<string, object>();
        set => _additionalProperties = value;
    }
}