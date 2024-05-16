using System.Text.Json;
using System.Text.Json.Nodes;

using Domain.Common;
using Domain.Constants.User;
using Domain.Entities;
using Domain.Entities.CoursAssistes;
using Domain.Entities.CoursNs;
using Domain.Entities.CoursSecondaireReussis;
using Domain.Entities.Etudiants;
using Domain.Entities.GrilleProgrammes;
using Domain.Entities.Identity;
using Domain.Entities.Programmes;
using Domain.Entities.SessionAssistees;
using Domain.Entities.SessionEtudes;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// ReSharper disable HeuristicUnreachableCode
#pragma warning disable CS0162 // Unreachable code detected

namespace Persistence;

public class DashboardProgrammeDbContextInitializer(
    ILogger<DashboardProgrammeDbContextInitializer> logger,
    DashboardProgrammeDbContext context,
    RoleManager<Role> roleManager,
    UserManager<User> userManager)
{
    private const string DEFAULT_EMAIL = "admin@gmail.com";

    /// <summary>
    ///     Si vrai, arrête de seeder dès qu'un premier duplicat est trouvé
    /// </summary>
    private const bool COURTCIRCUITER_INSERTION = true;

    private static readonly string? CHEMIN_SEEDS =
        Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName is
            { } racineSrc
            ? Path.Combine(Path.Combine(racineSrc, "Persistence"), "Seeds")
            : null;

    private User BuildUser()
    {
        return new User
        {
            Email = DEFAULT_EMAIL,
            UserName = DEFAULT_EMAIL,
            NormalizedEmail = DEFAULT_EMAIL.Normalize(),
            NormalizedUserName = DEFAULT_EMAIL,
            PhoneNumber = "555-555-5555",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false
        };
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    private static JsonArray LireFichierJsonSeed<T>() where T : Entity
    {
        string chemin = Path.Combine(CHEMIN_SEEDS!, $"{typeof(T).Name}.json");

        if (!File.Exists(chemin))
        {
            throw new InvalidOperationException($"Le fichier de seed {chemin} est manquant.");
        }

        string contenu = File.ReadAllText(chemin);
        return JsonSerializer.Deserialize<JsonArray>(contenu) ??
               throw new FormatException("Le fichier de seed est invalide");
    }

    public async Task SeedAsync()
    {
        try
        {
            await SeedRoles();
            await SeedUsersAndMembersForRole(Roles.ADMINISTRATOR);

            if (CHEMIN_SEEDS is not null)
            {
                // await SeedCours();
                await SeedProgramme();
                // await SeedSessionEtude();
                // await SeedGrilleProgramme();
                // await SeedEtudiant();
                // await SeedCoursSecondaireReussi();
                // await SeedSessionAssistee();
                // await SeedCoursAssiste();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task SeedCours()
    {
        JsonArray lstCours = LireFichierJsonSeed<Cours>();

        foreach (JsonNode? jsonNode in lstCours)
        {
            if (jsonNode is null)
            {
                continue;
            }

            string codeCours = jsonNode["Code"]!.GetValue<String>().Trim().ToUpper();
            string nomCours = jsonNode["Nom"]!.GetValue<String>().Trim();

            Cours? existingCours = context.Cours.IgnoreQueryFilters()
                .FirstOrDefault(x => x.Code.Trim().ToUpper() == codeCours);
            if (existingCours is not null)
            {
                if (COURTCIRCUITER_INSERTION)
                {
                    return;
                }

                continue;
            }

            Cours newCours = new(codeCours, nomCours);
            context.Cours.Add(newCours);
        }

        await context.SaveChangesAsync();
    }

    private async Task SeedCoursAssiste()
    {
        JsonArray lstCoursAssiste = LireFichierJsonSeed<CoursAssiste>();

        foreach (JsonNode? jsonNode in lstCoursAssiste)
        {
            if (jsonNode is null)
            {
                continue;
            }

            string hashEtudiant = jsonNode["SessionAssistee_Etudiant"]!.GetValue<String>().Trim().ToUpper();
            byte nieme = jsonNode["SessionAssistee_NiemeSession"]!.GetValue<byte>();
            string codeCours = jsonNode["Cours"]!.GetValue<String>().Trim().ToUpper();
            byte groupe = jsonNode["NumeroGroupe"]!.GetValue<byte>();
            Enum.TryParse(jsonNode["NoteRecue"]!.GetValue<String>(), out Note note);

            Etudiant etudiant =
                context.Etudiants.IgnoreQueryFilters().FirstOrDefault(x => x.HashCodePermanent == hashEtudiant) ??
                throw new InvalidOperationException("Etudiant introuvable pour le cours assisté");

            SessionAssistee sessionAssistee =
                context.SessionAssistees.IgnoreQueryFilters().FirstOrDefault(x =>
                    x.Etudiant == etudiant && x.NiemeSession == nieme) ??
                throw new InvalidOperationException("SessionAssistee introuvable pour le cours assisté");

            Cours cours =
                context.Cours.IgnoreQueryFilters().FirstOrDefault(x =>
                    x.Code == codeCours) ??
                throw new InvalidOperationException("Cours introuvable pour le cours assisté");

            CoursAssiste? existingCa = context.CoursAssistes.IgnoreQueryFilters()
                .FirstOrDefault(x => x.Cours == cours && x.SessionAssistee == sessionAssistee);
            if (existingCa is not null)
            {
                if (COURTCIRCUITER_INSERTION)
                {
                    return;
                }

                continue;
            }

            CoursAssiste newCa = new(cours, note, groupe, sessionAssistee);
            sessionAssistee.AddCoursAssiste(newCa);
            context.CoursAssistes.Add(newCa);
        }

        await context.SaveChangesAsync();
    }

    private async Task SeedCoursSecondaireReussi()
    {
        JsonArray lstCoursSec = LireFichierJsonSeed<CoursSecondaireReussi>();

        foreach (JsonNode? jsonNode in lstCoursSec)
        {
            if (jsonNode is null)
            {
                continue;
            }

            string hashEtudiant = jsonNode["Etudiant"]!.GetValue<String>().Trim().ToUpper();
            string codeCours = jsonNode["CodeCours"]!.GetValue<String>().Trim().ToUpper();

            Etudiant etudiant =
                context.Etudiants.IgnoreQueryFilters().FirstOrDefault(x => x.HashCodePermanent == hashEtudiant) ??
                throw new InvalidOperationException("Etudiant introuvable pour le cours du secondaire");

            CoursSecondaireReussi? existingCoursSec = context.CoursSecondaireReussis.IgnoreQueryFilters()
                .FirstOrDefault(x => x.Etudiant == etudiant && x.CodeCours == codeCours);
            if (existingCoursSec is not null)
            {
                if (COURTCIRCUITER_INSERTION)
                {
                    return;
                }

                continue;
            }

            CoursSecondaireReussi newCoursSec = new(etudiant, codeCours);
            etudiant.AddCoursSecondaireReussi(newCoursSec);
            context.CoursSecondaireReussis.Add(newCoursSec);
        }

        await context.SaveChangesAsync();
    }

    private async Task SeedEtudiant()
    {
        JsonArray lstEtudiants = LireFichierJsonSeed<Etudiant>();

        foreach (JsonNode? jsonNode in lstEtudiants)
        {
            if (jsonNode is null)
            {
                continue;
            }

            string hash = jsonNode["HashCodePermanent"]!.GetValue<String>().Trim().ToUpper();
            bool renforcement = jsonNode["EstBeneficiaireRenforcementFr"]!.GetValue<bool>();
            byte tour = jsonNode["TourAdmission"]!.GetValue<byte>();
            Enum.TryParse(jsonNode["StatutImmigration"]!.GetValue<String>(), out StatutImmigration statut);
            Enum.TryParse(jsonNode["Population"]!.GetValue<String>(), out Population population);
            Enum.TryParse(jsonNode["Sanction"]!.GetValue<String>(), out Sanction sanction);
            float genmels = jsonNode["MoyenneGeneraleAuSecondaire"]!.GetValue<float>();
            bool r18 = jsonNode["EstAssujetiAuR18"]!.GetValue<bool>();

            Etudiant? existingEtudiant = context.Etudiants.IgnoreQueryFilters()
                .FirstOrDefault(x => x.HashCodePermanent == hash);
            if (existingEtudiant is not null)
            {
                if (COURTCIRCUITER_INSERTION)
                {
                    return;
                }

                continue;
            }

            Etudiant newEtudiant = new(hash, renforcement, tour, statut, population, sanction, genmels, r18);
            context.Etudiants.Add(newEtudiant);
        }

        await context.SaveChangesAsync();
    }

    private async Task SeedGrilleProgramme()
    {
        JsonArray lstGrilleProgramme = LireFichierJsonSeed<GrilleProgramme>();

        foreach (JsonNode? jsonNode in lstGrilleProgramme)
        {
            if (jsonNode is null)
            {
                continue;
            }

            string codeProgramme = jsonNode["Programme"]!.GetValue<String>().Trim().ToUpper();
            byte nbSessions = jsonNode["EtaleeSurNbSessions"]!.GetValue<byte>();
            ushort annee = jsonNode["AnneeMiseAJour"]!.GetValue<ushort>();

            Programme programme =
                context.Programmes.IgnoreQueryFilters().FirstOrDefault(x => x.Numero == codeProgramme) ??
                throw new InvalidOperationException("Programme introuvable pour la grille");

            GrilleProgramme? existingGrilleProgramme = context.GrilleProgrammes.IgnoreQueryFilters()
                .FirstOrDefault(x =>
                    x.Programme == programme && x.EtaleeSurNbSessions == nbSessions &&
                    x.AnneeMiseAJour == annee);
            if (existingGrilleProgramme is not null)
            {
                if (COURTCIRCUITER_INSERTION)
                {
                    return;
                }

                continue;
            }

            GrilleProgramme newGrilleProgramme = new(programme, nbSessions, annee);
            context.GrilleProgrammes.Add(newGrilleProgramme);
        }

        await context.SaveChangesAsync();
    }

    private async Task SeedProgramme()
    {
        JsonArray lstProgrammes = LireFichierJsonSeed<Programme>();

        foreach (JsonNode? jsonNode in lstProgrammes)
        {
            if (jsonNode is null)
            {
                continue;
            }

            string numeroProgramme = jsonNode["Numero"]!.GetValue<String>().Trim().ToUpper();
            string nomProgramme = jsonNode["Nom"]!.GetValue<String>().Trim();

            Programme? existingProgramme = context.Programmes.IgnoreQueryFilters()
                .FirstOrDefault(x => x.Numero.Trim().ToUpper() == numeroProgramme);
            if (existingProgramme is not null)
            {
                if (COURTCIRCUITER_INSERTION)
                {
                    return;
                }

                continue;
            }

            Programme newProgramme = new(numeroProgramme, nomProgramme);
            context.Programmes.Add(newProgramme);
        }

        await context.SaveChangesAsync();
    }

    private async Task SeedRoles()
    {
        if (!roleManager.RoleExistsAsync(Roles.ADMINISTRATOR).Result)
        {
            await roleManager.CreateAsync(new Role
            {
                Name = Roles.ADMINISTRATOR, NormalizedName = Roles.ADMINISTRATOR.Normalize()
            });
        }
    }

    private async Task SeedSessionAssistee()
    {
        JsonArray lstSessionAssistee = LireFichierJsonSeed<SessionAssistee>();

        foreach (JsonNode? jsonNode in lstSessionAssistee)
        {
            if (jsonNode is null)
            {
                continue;
            }

            string hashEtudiant = jsonNode["Etudiant"]!.GetValue<String>().Trim().ToUpper();
            string gpProgramme = jsonNode["GrilleProgramme_Programme"]!.GetValue<String>().Trim().ToUpper();
            ushort gpAnnee = jsonNode["GrilleProgramme_Annee"]!.GetValue<ushort>();
            byte gpEtalee = jsonNode["GrilleProgramme_Nb"]!.GetValue<byte>();
            ushort seAnnee = jsonNode["SessionEtude_Annee"]!.GetValue<ushort>();
            Enum.TryParse(jsonNode["SessionEtude_Saison"]!.GetValue<String>(), out Saison seSaison);
            bool servicesAdaptes = jsonNode["EstBeneficiaireServicesAdaptes"]!.GetValue<bool>();
            ushort heures = jsonNode["NbTotalHeures"]!.GetValue<ushort>();
            byte nieme = jsonNode["NiemeSession"]!.GetValue<byte>();

            Etudiant etudiant =
                context.Etudiants.IgnoreQueryFilters().FirstOrDefault(x => x.HashCodePermanent == hashEtudiant) ??
                throw new InvalidOperationException("Etudiant introuvable pour la session assistée");

            GrilleProgramme grilleProgramme =
                context.GrilleProgrammes.IgnoreQueryFilters().FirstOrDefault(x =>
                    x.Programme.Numero == gpProgramme && x.EtaleeSurNbSessions == gpEtalee &&
                    x.AnneeMiseAJour == gpAnnee) ??
                throw new InvalidOperationException("GrilleProgramme introuvable pour la session assistée");

            SessionEtude sessionEtude =
                context.SessionEtudes.IgnoreQueryFilters().FirstOrDefault(x =>
                    x.Annee == seAnnee && x.Saison == seSaison) ??
                throw new InvalidOperationException("SessionEtude introuvable pour la session assistée");

            SessionAssistee? existingSess = context.SessionAssistees.IgnoreQueryFilters()
                .FirstOrDefault(x => x.Etudiant == etudiant && x.SessionEtude == sessionEtude);
            if (existingSess is not null)
            {
                if (COURTCIRCUITER_INSERTION)
                {
                    return;
                }

                continue;
            }

            SessionAssistee newSess = new(etudiant, grilleProgramme, sessionEtude, heures, nieme, servicesAdaptes);
            etudiant.AddSessionAssistee(newSess);
            context.SessionAssistees.Add(newSess);
        }

        await context.SaveChangesAsync();
    }

    private async Task SeedSessionEtude()
    {
        JsonArray lstSessionEtudes = LireFichierJsonSeed<SessionEtude>();

        foreach (JsonNode? jsonNode in lstSessionEtudes)
        {
            if (jsonNode is null)
            {
                continue;
            }

            ushort annee = jsonNode["Annee"]!.GetValue<ushort>();
            Enum.TryParse(jsonNode["Saison"]!.GetValue<String>(), out Saison saison);
            string slug = jsonNode["Slug"]!.GetValue<String>().Trim();

            SessionEtude? existingSessionEtude = context.SessionEtudes.IgnoreQueryFilters()
                .FirstOrDefault(x => x.Saison == saison && x.Annee == annee);
            if (existingSessionEtude is not null)
            {
                if (COURTCIRCUITER_INSERTION)
                {
                    return;
                }

                continue;
            }

            SessionEtude newSessionEtude = new(annee, saison);
            newSessionEtude.SetSlug(slug);
            context.SessionEtudes.Add(newSessionEtude);
        }

        await context.SaveChangesAsync();
    }

    private async Task SeedUsersAndMembersForRole(string role)
    {
        User? user = await userManager.FindByEmailAsync(DEFAULT_EMAIL);
        if (user == null)
        {
            user = BuildUser();
            IdentityResult? result = await userManager.CreateAsync(user, "Qwerty123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
            else
            {
                throw new Exception($"Could not seed/create {role} user.");
            }
        }

        Member? existingMember = context.Members.IgnoreQueryFilters().FirstOrDefault(x => x.User.Id == user.Id);
        switch (existingMember)
        {
            case { Active: true }:
                return;
            case null:
                {
                    Member member = new("ADMIN", "MEMBER", 1, "123, my street", "my city", "A1A 1A1");
                    member.SetUser(user);
                    member.Activate();
                    context.Members.Add(member);
                    await context.SaveChangesAsync();
                    break;
                }
            default:
                {
                    if (!existingMember.Active)
                    {
                        existingMember.Activate();
                        context.Members.Update(existingMember);
                        await context.SaveChangesAsync();
                    }

                    break;
                }
        }
    }
}