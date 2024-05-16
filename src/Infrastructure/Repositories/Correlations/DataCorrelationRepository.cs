using Application.Exceptions.Etudiants;
using Application.Interfaces;

using Domain.Entities.CoursAssistes;
using Domain.Repositories;

using Slugify;

namespace Infrastructure.Repositories.Correlations;

public class DataCorrelationRepository : IDataCorrelationRepository
{
    private readonly IDashboardProgrammeDbContext _context;
    private readonly ISlugHelper _slugHelper;

    public DataCorrelationRepository(IDashboardProgrammeDbContext context, ISlugHelper slugHelper)
    {
        _context = context;
        _slugHelper = slugHelper;
    }

    public IEnumerable<(float noteMoyenne, IEnumerable<string> coursSecondaire)>
        GetNoteWithCoursSecondaireReussisForGivenCoursId(Guid id)
    {
        _context.SetRequestTimeout(180);
        var result = _context.CoursAssistes
            .Where(ca => ca.Cours.Id == id && ca.NoteRecue != Note.Incomplet)
            .Select(ca => new
            {
                IdEtudiant = ca.SessionAssistee.Etudiant.Id,
                ca.NoteRecue,
                ca.SessionAssistee.Etudiant.CoursSecondaireReussis
            })
            .ToLookup(obj => obj.IdEtudiant)
            .Select(ig => new
            {
                NoteMoyenne = Convert.ToSingle(ig.Average(obj => obj.NoteRecue.ToByte())),
                CoursSecondaireReussis = ig.SelectMany(obj => obj.CoursSecondaireReussis).ToHashSet()
                    .Select(csr => csr.CodeCours)
            })
            .ToList();
        _context.SetRequestTimeout();

        IEnumerable<(float noteMoyenne, IEnumerable<string> coursSecondaire)> lstTuple =
            result.Select(l => (l.NoteMoyenne, l.CoursSecondaireReussis)).ToArray();

        if (!lstTuple.Any())
        {
            throw new EtudiantNotFoundException($"Aucun étudiants n'ont été trouvé pour le cours avec l'id {id}.");
        }

        return lstTuple;
    }

    public IEnumerable<(float noteMoyenne, float GENMELS)> GetNoteWithGENMELSForGivenCoursId(Guid id)
    {
        _context.SetRequestTimeout(180);
        var result = _context.CoursAssistes
            .Where(ca => ca.Cours.Id == id && ca.NoteRecue != Note.Incomplet)
            .Select(ca => new
            {
                IdEtudiant = ca.SessionAssistee.Etudiant.Id,
                ca.NoteRecue,
                GENMELS = ca.SessionAssistee.Etudiant.MoyenneGeneraleAuSecondaire
            })
            .ToLookup(obj => obj.IdEtudiant)
            .Select(ig => new
            {
                NoteMoyenne = Convert.ToSingle(ig.Average(obj => obj.NoteRecue.ToByte())), ig.First().GENMELS
            })
            .ToList();
        _context.SetRequestTimeout();

        IEnumerable<(float noteMoyenne, float GENMELS)> lstTuple =
            result.Select(l => (l.NoteMoyenne, l.GENMELS)).ToArray();

        if (!lstTuple.Any())
        {
            throw new EtudiantNotFoundException($"Aucun étudiants n'ont été trouvé pour le cours avec l'id {id}.");
        }

        return lstTuple.OrderBy(obj => obj.GENMELS);
    }


    public IEnumerable<(float noteMoyenne, byte tourAdmission)> GetNoteWithWithTourAdmissionForGivenCoursId(Guid id)
    {
        _context.SetRequestTimeout(180);

        var result = _context.CoursAssistes
            .Where(ca => ca.Cours.Id == id && ca.NoteRecue != Note.Incomplet)
            .Select(ca => new
            {
                IdEtudiant = ca.SessionAssistee.Etudiant.Id, ca.NoteRecue, ca.SessionAssistee.Etudiant.TourAdmission
            })
            .AsEnumerable() // Load data into memory
            .GroupBy(obj => new { obj.IdEtudiant, obj.TourAdmission })
            .Select(ig => new
            {
                NoteMoyenne = Convert.ToSingle(ig.Average(obj => obj.NoteRecue.ToByte())), ig.Key.TourAdmission
            })
            .ToList();

        _context.SetRequestTimeout();

        IEnumerable<(float noteMoyenne, byte tourAdmission)> lstTuple =
            result.Select(l => (l.NoteMoyenne, l.TourAdmission)).ToArray();

        if (!lstTuple.Any())
        {
            throw new EtudiantNotFoundException($"Aucun étudiants n'ont été trouvé pour le cours avec l'id {id}.");
        }

        return lstTuple;
    }

    public IEnumerable<(float noteMoyenne, string etudiantInternational)>
        GetNoteWithWithInternationalForGivenCoursId(Guid id)
    {
        _context.SetRequestTimeout(180);

        var result = _context.CoursAssistes
            .Where(ca => ca.Cours.Id == id && ca.NoteRecue != Note.Incomplet)
            .Select(ca => new
            {
                IdEtudiant = ca.SessionAssistee.Etudiant.Id,
                ca.NoteRecue,
                EtudiantInternational = ca.SessionAssistee.Etudiant.StatutImmigration
            })
            .AsEnumerable() // Load data into memory
            .GroupBy(obj => new { obj.IdEtudiant, obj.EtudiantInternational })
            .Select(ig => new
            {
                NoteMoyenne = Convert.ToSingle(ig.Average(obj => obj.NoteRecue.ToByte())),
                EtudiantInternational = ig.Key.EtudiantInternational.ToString()
            })
            .ToList();

        _context.SetRequestTimeout();

        IEnumerable<(float noteMoyenne, string etudiantInternational)> lstTuple =
            result.Select(l => (l.NoteMoyenne, l.EtudiantInternational)).ToArray();

        if (!lstTuple.Any())
        {
            throw new EtudiantNotFoundException($"Aucun étudiants n'ont été trouvé pour le cours avec l'id {id}.");
        }

        return lstTuple;
    }
}