using System.Text.RegularExpressions;

using Application.Interfaces.ImportData;
using Application.Interfaces.Services.Gestionnaire;

using Domain.Entities.CoursAssistes;
using Domain.Entities.CoursNs;
using Domain.Entities.CoursSecondaireReussis;
using Domain.Entities.Etudiants;
using Domain.Entities.GrilleProgrammes;
using Domain.Entities.Programmes;
using Domain.Entities.SessionAssistees;
using Domain.Entities.SessionEtudes;
using Domain.Repositories;

namespace Application.Services.Gestionnaire;

public partial class ImportDataService : IImportDataService
{
    private readonly ICoursAssisteRepository _coursAssisteRepository;

    private readonly ICoursRepository _coursRepository;

    private readonly ICoursSecondaireReussiRepository _coursSecondaireReussiRepository;
    private readonly IEtudiantRepository _etudiantREpository;
    private readonly IGrilleProgrammeRepository _grilleProgrammeCreationService;
    private readonly IProgrammeRepository _programmeRepository;
    private readonly ISessionAssisteeRepository _sessionAssisteeRepository;
    private readonly ISessionEtudeRepository _sessionEtudeRepository;


    public ImportDataService(
        IEtudiantRepository etudiantREpository,
        ISessionEtudeRepository sessionEtudeRepository,
        ISessionAssisteeRepository sessionAssisteeRepository,
        ICoursRepository coursRepository,
        ICoursAssisteRepository coursAssisteRepository,
        ICoursSecondaireReussiRepository coursSecondaireReussiRepository,
        IGrilleProgrammeRepository grilleProgrammeRepository,
        IProgrammeRepository programmeRepository)
    {
        _etudiantREpository = etudiantREpository;
        _sessionEtudeRepository = sessionEtudeRepository;
        _sessionAssisteeRepository = sessionAssisteeRepository;
        _coursRepository = coursRepository;
        _coursAssisteRepository = coursAssisteRepository;
        _coursSecondaireReussiRepository = coursSecondaireReussiRepository;
        _grilleProgrammeCreationService = grilleProgrammeRepository;
        _programmeRepository = programmeRepository;
    }

    public async Task ImportData(IContenuFichierImportation contenuFichier)
    {
        SessionEtude sessionEtude = await ObtenirOuCreerSessionEtude(contenuFichier.SheetName);

        foreach (IImportDataRow row in contenuFichier.Records)
        {
            string grille = row.Grille;
            ushort nbHeures = row.NombreDheuresDeCoursDansLaSession;
            byte nIemeSession = row.Spe.ToByte();
            bool estBeneficiaireServicesAdaptes = row.ServicesAdaptes.ToBool();

            GrilleProgramme grilleProgramme = await ObtenirOuCreerGrilleProgramme(grille);

            Etudiant etudiant = await ObtenirOuCreerEtudiant(row);


            Dictionary<string, bool> DictCoursSecondaire = new()
            {
                { "436", row._436.ToBool() },
                { "536", row._536.ToBool() },
                { "SN4", row.Sn4.ToBool() },
                { "TS_SN4", row.TsSn4.ToBool() },
                { "TS4_SN4+", row.Ts4Sn4Plus.ToBool() },
                { "CST5", row.Cst5.ToBool() },
                { "TS_SN5", row.TsSn5.ToBool() },
                { "TS5", row.Ts5.ToBool() },
                { "514+", row._514plus.ToBool() },
                { "526+", row._526plus.ToBool() },
                { "514", row._514.ToBool() },
                { "526", row._526.ToBool() }
            };

            foreach (KeyValuePair<string, bool> kv in from kv in DictCoursSecondaire
                     where kv.Value
                     let exiistingCoursSecondaireReussi =
                         _coursSecondaireReussiRepository.FindByEtudiantAndCodeCourse(etudiant.Id, kv.Key)
                     where exiistingCoursSecondaireReussi is null
                     select kv)
            {
                await _coursSecondaireReussiRepository.CreateCoursSecondaireReussi(
                    new CoursSecondaireReussi(etudiant, kv.Key));
            }

            SessionAssistee sessionAssistee = _sessionAssisteeRepository.FindByParams(
                etudiant, grilleProgramme, sessionEtude, nbHeures, nIemeSession,
                estBeneficiaireServicesAdaptes) ?? await _sessionAssisteeRepository.AddSessionAssisteeWithoutSaving(
                new SessionAssistee(etudiant, grilleProgramme, sessionEtude, nbHeures, nIemeSession,
                    estBeneficiaireServicesAdaptes)
            );

            List<string> coursActuels = row.CoursInscritsActuellement.Trim().Split(";").ToList();
            await InsertCours(coursActuels);

            if (row.CoursInscritsSessionPasse.Trim().ToLower() != "aucune donnée")
            {
                List<string> coursPasses = row.CoursInscritsSessionPasse.Trim().Split(";").ToList();
                await InsertCours(coursPasses, sessionAssistee);
            }
        }

        await SaveDataInBD();
    }

    public async Task SaveDataInBD()
    {
        await _etudiantREpository.SaveChangesAsync();
        await _coursAssisteRepository.SaveChangesAsync();
        await _coursSecondaireReussiRepository.SaveChangesAsync();
        await _sessionAssisteeRepository.SaveChangesAsync();
        await _coursAssisteRepository.SaveChangesAsync();
        await _grilleProgrammeCreationService.SaveChangesAsync();
        await _sessionEtudeRepository.SaveChangesAsync();
    }

    /// <summary>
    ///     Permet de creer un cours et de l'ajouter a la base de donnees, crèe un cours assisté si la session assistée est non
    ///     null
    /// </summary>
    /// <param name="lstCode"></param>
    /// <param name="sessionAssistee"></param>
    /// <param name="estCoursAssiste"></param>
    private async Task InsertCours(IEnumerable<string> lstCode)
    {
        foreach (string codeCours in lstCode)
        {
            string codeCoursFormate = formatCode(codeCours.Trim());
            Cours? existingCours = _coursRepository.FindByCode(codeCoursFormate);
            if (existingCours is null)
            {
                await _coursRepository.AddCoursWithoutSaveChanges(new Cours(codeCoursFormate, ""));
            }
        }
    }

    private async Task InsertCours(IReadOnlyList<string> lstCode, SessionAssistee sessionAssistee)
    {
        foreach ((string code, string str_note) in Enumerable.Range(0, lstCode.Count)
                     .GroupBy(i => i / 2, i => lstCode[i]).Select(ig => (ig.First(), ig.Last())))
        {
            string codeFormate = formatCode(code.Trim());

            if (!Enum.TryParse(str_note.Trim().ToUpperInvariant(), out Note note))
            {
                note = str_note.Trim().ToUpper() switch
                {
                    "E" => Note.Echec,
                    _ => Note.Incomplet
                };
            }

            Cours newCours = _coursRepository.FindByCode(codeFormate) ??
                             await _coursRepository.AddCoursWithoutSaveChanges(new Cours(codeFormate, ""));

            if (_coursAssisteRepository.FindBySessionAssisteeIdAndCoursId(sessionAssistee.Id, newCours.Id) is null)
            {
                await _coursAssisteRepository.CreateCoursAssisteWithoutSaveChanges(new CoursAssiste(newCours, note,
                    sessionAssistee));
            }
        }
    }

    private string formatCode(string code)
    {
        string programme = code[..3];
        string cours = code[3..6];
        string suffixe = code[6..8];
        return $"{programme}-{cours}-{suffixe}";
    }

    private async Task<Etudiant> ObtenirOuCreerEtudiant(IImportDataRow row)
    {
        string HashCodePermanent = row.CodePermanentCrypte;
        bool EstBeneficiaireRenforcementFr = row.RenforcementEnFrançais.ToBool();
        byte TourAdmission = row.TourDadmission.ToByte();
        bool EstAssujetiAuR18 = row.EtudiantAssujettiAuR18.ToBool();
        Population population = row.Population.ToPopulation();
        StatutImmigration statutImmigration = row.EtudiantInternational.ToStatutImmigration();
        Sanction sanction = row.SanctionCollegiale.ToSanction();
        float moyenneGeneraleAuSecondaire = row.Genmels;

        return _etudiantREpository.FindByHashCodePermanent(HashCodePermanent) ??
               await _etudiantREpository.AddEtudiantWIthoutSaveChanges(new Etudiant(HashCodePermanent,
                   EstBeneficiaireRenforcementFr, TourAdmission,
                   statutImmigration, population, sanction, moyenneGeneraleAuSecondaire, EstAssujetiAuR18));
    }

    [GeneratedRegex("^(?<CodeProgramme>[0-9]{3}[A-Z][A-Z0-9])-(?<Annee>2[0-9]{3})-(?<Etalee>[0-9])$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex RegexGrilleProgramme();

    private async Task<GrilleProgramme> ObtenirOuCreerGrilleProgramme(string grille)
    {
        Match resultatRegexGrilleProgramme = RegexGrilleProgramme().Match(grille);
        ushort anneeMiseAJour = Convert.ToUInt16(resultatRegexGrilleProgramme.Groups["Annee"].Value);
        string codeprogramme = resultatRegexGrilleProgramme.Groups["CodeProgramme"].Value;
        string format_code = $"{codeprogramme[..3]}.{codeprogramme[3..]}";
        byte etaleeSurNbSessions = byte.Parse(resultatRegexGrilleProgramme.Groups["Etalee"].Value);

        Programme programme = await _programmeRepository.FindByNumeroProgramme(format_code) ??
                              throw new InvalidOperationException($"Le programme {format_code} n'existe pas");

        GrilleProgramme grilleProgramme =
            await _grilleProgrammeCreationService.FindByParams(programme, etaleeSurNbSessions, anneeMiseAJour) ??
            await _grilleProgrammeCreationService.AddGrilleProgrammeWithoutSaving(
                new GrilleProgramme(programme, etaleeSurNbSessions, anneeMiseAJour));

        return grilleProgramme;
    }

    private async Task<SessionEtude> ObtenirOuCreerSessionEtude(string sheetName)
    {
        string[] date = sheetName.Split("-");
        ushort annee = ushort.Parse(date[0]);
        ushort mois = ushort.Parse(date[1]);

        Saison saison = mois switch
        {
            <= 6 => Saison.Hiver,
            >= 8 => Saison.Automne,
            _ => Saison.Ete
        };

        return _sessionEtudeRepository.FindByAnneeAndSaison(annee, saison) ??
               await _sessionEtudeRepository.AddSessionEtudeWithoutSaving(new SessionEtude(annee, saison));
    }
}