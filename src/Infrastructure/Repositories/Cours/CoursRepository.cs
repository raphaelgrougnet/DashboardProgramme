using Application.Exceptions.Cours;
using Application.Interfaces;

using Domain.Entities.CoursAssistes;
using Domain.Entities.CoursNs;
using Domain.Entities.Etudiants;
using Domain.Entities.SessionAssistees;
using Domain.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Repositories.CoursNs;

public class CoursRepository(IDashboardProgrammeDbContext context) : ICoursRepository
{
    public async Task<Cours> CreateCours(Cours cours)
    {
        await AddCoursWithoutSaveChanges(cours);
        await SaveChangesAsync();
        return cours;
    }

    public async Task<Cours> AddCoursWithoutSaveChanges(Cours cours)
    {
        if (context.Cours.Any(x => x.Code.Trim() == cours.Code.Trim()))
        {
            throw new CoursWithCodeAlreadyExistsException($"A course with code {cours.Code} already exists.");
        }


        context.Cours.Add(cours);
        return cours;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public Cours? FindByCode(string code)
    {
        EntityEntry<Cours>? coursEnAttenteDeSauvegarde =
            context.EfChangeTracker.Entries<Cours>().FirstOrDefault(c => c.Entity.Code == code);

        if (coursEnAttenteDeSauvegarde is not null)
        {
            return coursEnAttenteDeSauvegarde.Entity;
        }

        return context.Cours
            .FirstOrDefault(x =>
                x.Code == code);
    }

    public Cours FindById(Guid id)
    {
        Cours? cours = context.Cours
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);
        if (cours is null)
        {
            throw new CoursNotFoundException($"Could not find cours with id {id}.");
        }

        return cours;
    }

    public List<Cours> GetAll()
    {
        return context.Cours.AsNoTracking().ToList();
    }

    public List<Cours> GetCoursForSessionEtudeOfProgramme(Guid idProgramme, Guid idSessionEtude)
    {
        return context.CoursAssistes.AsNoTracking()
            .Where(ca =>
                ca.SessionAssistee.SessionEtude.Id == idSessionEtude &&
                ca.SessionAssistee.GrilleProgramme.Programme.Id == idProgramme)
            .Select(ca => ca.Cours)
            .Distinct()
            .ToList();
    }

    public int GetEtudiantsForCours(Guid coursId, Guid idSessionEtude, Guid idProgramme)
    {
        var studentCount = context.CoursAssistes
            .AsNoTracking()
            .Where(ca => ca.Cours.Id == coursId && ca.SessionAssistee.SessionEtude.Id == idSessionEtude)
            .Select(ca => ca.SessionAssistee.Etudiant.Id)
            .Distinct()
            .Count();

        return studentCount;
    }
    
    public Dictionary<Note,int> GetAverageGradesForStudentsInClass(Guid coursId, Guid idSessionEtude, Guid idProgramme)
    {
        
        Dictionary<Note, int> countMoyenneEtudiants = new Dictionary<Note, int>
        {
            { Note.A, 0 },
            { Note.B, 0 },
            { Note.C, 0 },
            { Note.D, 0 },
            { Note.Echec, 0 }
        };        
        
        var studentIds = context.CoursAssistes
            .AsNoTracking()
            .Where(ca => ca.Cours.Id == coursId && ca.SessionAssistee.SessionEtude.Id == idSessionEtude)
            .Select(ca => ca.SessionAssistee.Etudiant.Id)
            .Distinct()
            .ToList();

        foreach (Guid studentId in studentIds)
        {
            
            var grades = context.CoursAssistes
                .AsNoTracking()
                .Include(ca => ca.SessionAssistee)
                .Where(ca => ca.SessionAssistee.Etudiant.Id == studentId && ca.NoteRecue != Note.Incomplet)
                .Select(ca => ca.NoteRecue) 
                .ToList();
            
            if (grades.Count == 0) continue;

            int studentGradesCount = grades.Sum(grade => grade.ToByte());  
            var studentMoyenne = (double)studentGradesCount / grades.Count;
            
            _ = studentMoyenne switch
            {
                >= 90 => countMoyenneEtudiants[Note.A] += 1,
                >= 80 => countMoyenneEtudiants[Note.B] += 1,
                >= 70 => countMoyenneEtudiants[Note.C] += 1,
                >= 60 => countMoyenneEtudiants[Note.D] += 1,
                _ => countMoyenneEtudiants[Note.Echec] += 1
            };

        }
        
        return countMoyenneEtudiants;
    }
}