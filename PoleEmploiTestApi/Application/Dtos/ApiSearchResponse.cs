using System.Text.Json.Serialization;

namespace Application.Dtos;

public record ApiSearchResponse(
    [property: JsonPropertyName("resultats")] List<Offre> Resultats,
    [property: JsonPropertyName("filtresPossibles")] List<FiltrePossible> FiltresPossibles
);

public record Offre(
    string Id,
    string Intitule,
    string Description,
    DateTime DateCreation,
    DateTime DateActualisation,
    LieuTravail LieuTravail,
    string RomeCode,
    string RomeLibelle,
    string Appellationlibelle,
    Entreprise Entreprise,
    string TypeContrat,
    string TypeContratLibelle,
    string NatureContrat,
    string ExperienceExige,
    string ExperienceLibelle,
    string ExperienceCommentaire,
    List<Formation> Formations,
    List<Langue> Langues,
    List<Permis> Permis,
    [property: JsonPropertyName("outilsBureautiques")] List<string> OutilsBureautiques,
    List<Competence> Competences,
    Salaire Salaire,
    string DureeTravailLibelle,
    string DureeTravailLibelleConverti,
    string ComplementExercice,
    string ConditionExercice,
    bool Alternance,
    Contact Contact,
    Agence Agence,
    int NombrePostes,
    bool AccessibleTH,
    string DeplacementCode,
    string DeplacementLibelle,
    string QualificationCode,
    string QualificationLibelle,
    string CodeNAF,
    string SecteurActivite,
    string SecteurActiviteLibelle,
    List<QualiteProfessionnelle> QualitesProfessionnelles,
    string TrancheEffectifEtab,
    OrigineOffre OrigineOffre,
    bool OffresManqueCandidats,
    ContexteTravail ContexteTravail
);

public record LieuTravail(
    string Libelle,
    double Latitude,
    double Longitude,
    string CodePostal,
    string Commune
);

public record Entreprise(
    string Nom,
    string Description,
    string Logo,
    string Url,
    bool EntrepriseAdaptee
);

public record Formation(
    string CodeFormation,
    string DomaineLibelle,
    string NiveauLibelle,
    string Commentaire,
    string Exigence
);

public record Langue(
    string Libelle,
    string Exigence
);

public record Permis(
    string Libelle,
    string Exigence
);

public record Competence(
    string Code,
    string Libelle,
    string Exigence
);

public record Salaire(
    string Libelle,
    string Commentaire,
    string Complement1,
    string Complement2
);

public record Contact(
    string Nom,
    string Coordonnees1,
    string Coordonnees2,
    string Coordonnees3,
    string Telephone,
    string Courriel,
    string Commentaire,
    string UrlRecruteur,
    string UrlPostulation
);

public record Agence(
    string Telephone,
    string Courriel
);

public record QualiteProfessionnelle(
    string Libelle,
    string Description
);

public record OrigineOffre(
    string Origine,
    string UrlOrigine,
    List<Partenaire> Partenaires
);

public record Partenaire(
    string Nom,
    string Url,
    string Logo
);

public record ContexteTravail(
    string[] Horaires,
    string[] ConditionsExercice
);

public record FiltrePossible(
    string Filtre,
    List<Agregation> Agregation
);

public record Agregation(
    string ValeurPossible,
    int NbResultats
);




