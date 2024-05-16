using Application.Interfaces.Services.Correlations;

using Domain.Repositories;

using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;

namespace Application.Services.Correlations;

public class DataCorrelationService : IDataCorrelationService
{
    private readonly IDataCorrelationRepository _dataCorrelationRepository;

    public DataCorrelationService(IDataCorrelationRepository dataCorrelationRepository)
    {
        _dataCorrelationRepository = dataCorrelationRepository;
    }

    public Task<double> CalculatePValue(Guid idCours, string critere)
    {
        IEnumerable<(float noteMoyenne, IEnumerable<string> coursSecondaire)> etudiants =
            _dataCorrelationRepository.GetNoteWithCoursSecondaireReussisForGivenCoursId(idCours);

        (List<float> groupeReussi, List<float> groupeRate) groupes = GetGroupes(etudiants, critere);

        return CalculatePValue(groupes.groupeReussi, groupes.groupeRate);
    }

    public Task<double> CalculatePValueGENMELS(Guid idCours)
    {
        IEnumerable<(float noteMoyenne, float GENMELS)> etudiantsGENMELS =
            _dataCorrelationRepository.GetNoteWithGENMELSForGivenCoursId(idCours);
        float mediane = etudiantsGENMELS.Count() % 2 != 0
            ? etudiantsGENMELS.ElementAt(etudiantsGENMELS.Count() / 2).GENMELS
            : (etudiantsGENMELS.ElementAt(etudiantsGENMELS.Count() / 2).GENMELS +
               etudiantsGENMELS.ElementAt((etudiantsGENMELS.Count() / 2) - 1).GENMELS) / 2;
        (List<float> groupeReussi, List<float> groupeRate) groupesGenmels =
            GetGroupesGENMELS(_dataCorrelationRepository.GetNoteWithGENMELSForGivenCoursId(idCours), mediane);
        return CalculatePValue(groupesGenmels.groupeReussi, groupesGenmels.groupeRate);
    }

    public Task<double> CalculatePValueTourAdmission(Guid idCours, byte tourAdmission)
    {
        IEnumerable<(float noteMoyenne, byte tourAdmission)> etudiantsTourAdmission =
            _dataCorrelationRepository.GetNoteWithWithTourAdmissionForGivenCoursId(idCours);
        (List<float> groupeReussi, List<float> groupeRate) groupesTourAdmission =
            GetGroupesTourAdmission(etudiantsTourAdmission, tourAdmission);
        return CalculatePValue(groupesTourAdmission.groupeReussi, groupesTourAdmission.groupeRate);
    }

    public Task<double> CalculatePValueInternational(Guid idCours, string etudiantInternational)
    {
        IEnumerable<(float noteMoyenne, string etudiantInternational)> etudiantsInternational =
            _dataCorrelationRepository.GetNoteWithWithInternationalForGivenCoursId(idCours);
        (List<float> groupeReussi, List<float> groupeRate) groupesInternational =
            GetGroupesInternational(etudiantsInternational, etudiantInternational);
        return CalculatePValue(groupesInternational.groupeReussi, groupesInternational.groupeRate);
    }

    private (List<float> groupeReussi, List<float> groupeRate) GetGroupes(
        IEnumerable<(float noteMoyenne, IEnumerable<string> coursSecondaire)> etudiants, string critere)
    {
        List<float> groupeReussi = new();
        List<float> groupeRate = new();

        foreach ((float noteMoyenne, IEnumerable<string> coursSecondaire) in etudiants)
        {
            if (coursSecondaire.Any(cours => cours == critere))
            {
                groupeReussi.Add(noteMoyenne);
            }
            else
            {
                groupeRate.Add(noteMoyenne);
            }
        }

        return (groupeReussi, groupeRate);
    }

    private (List<float> groupeReussi, List<float> groupeRate) GetGroupesGENMELS(
        IEnumerable<(float noteMoyenne, float GENMELS)> etudiantsGENMELS, float mediane)
    {
        List<float> groupeReussi = new();
        List<float> groupeRate = new();

        foreach ((float noteMoyenne, float genmels) in etudiantsGENMELS)
        {
            if (genmels >= mediane)
            {
                groupeReussi.Add(noteMoyenne);
            }
            else
            {
                groupeRate.Add(noteMoyenne);
            }
        }

        return (groupeReussi, groupeRate);
    }

    private (List<float> groupeReussi, List<float> groupeRate) GetGroupesTourAdmission(
        IEnumerable<(float noteMoyenne, byte tourAdmission)> etudiantsTourAdmission, byte tourAdmission)
    {
        List<float> groupeReussi = new();
        List<float> groupeRate = new();

        foreach ((float noteMoyenne, byte tourAdmissionEtudiant) in etudiantsTourAdmission)
        {
            if (tourAdmissionEtudiant == tourAdmission)
            {
                groupeReussi.Add(noteMoyenne);
            }
            else
            {
                groupeRate.Add(noteMoyenne);
            }
        }

        return (groupeReussi, groupeRate);
    }

    private (List<float> groupeReussi, List<float> groupeRate) GetGroupesInternational(
        IEnumerable<(float noteMoyenne, string etudiantInternational)> etudiantsEtudiantInternational,
        string etudiantInternationalChoix)
    {
        List<float> groupeReussi = new();
        List<float> groupeRate = new();

        foreach ((float noteMoyenne, string etudiantInternational) in etudiantsEtudiantInternational)
        {
            if (etudiantInternational == etudiantInternationalChoix)
            {
                groupeReussi.Add(noteMoyenne);
            }
            else
            {
                groupeRate.Add(noteMoyenne);
            }
        }

        return (groupeReussi, groupeRate);
    }

    private Task<double> CalculatePValue(List<float> groupeReussi, List<float> groupeRate)
    {
        double mean1 = groupeReussi.Mean();
        double stdDev1 = groupeReussi.StandardDeviation();
        double mean2 = groupeRate.Mean();
        double stdDev2 = groupeRate.StandardDeviation();

        double variance1 = Math.Pow(stdDev1, 2);
        double variance2 = Math.Pow(stdDev2, 2);


        double dividande = (variance1 * (groupeReussi.Count - 1)) + (variance2 * (groupeRate.Count - 1));
        double diviseur = groupeReussi.Count + groupeRate.Count - 2;


        double Sp = Math.Sqrt(dividande / diviseur);

        dividande = mean1 - mean2;
        diviseur = Math.Sqrt((1f / groupeReussi.Count) + (1f / groupeRate.Count));

        double Zobs = dividande / (Sp * diviseur);


        double degreeOfFreedom = groupeReussi.Count + groupeRate.Count - 2;

        StudentT studentT = new(0, 1, degreeOfFreedom);
        double pValue = 2 * (1 - studentT.CumulativeDistribution(Math.Abs(Zobs)));
        return Task.FromResult(pValue);
    }
}