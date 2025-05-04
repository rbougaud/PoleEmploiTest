using System.Text.Json.Serialization;

namespace Application.Dtos;

public record RootObject(
    [property: JsonPropertyName("resultats")] List<Resultat> Resultats,
    [property: JsonPropertyName("filtresPossibles")] List<FiltrePossible> FiltresPossibles
);

public record Resultat(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("intitule")] string Intitule,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("dateCreation")] DateTime DateCreation,
    [property: JsonPropertyName("dateActualisation")] DateTime DateActualisation,
    [property: JsonPropertyName("lieuTravail")] LieuTravail LieuTravail,
    [property: JsonPropertyName("romeCode")] string RomeCode,
    [property: JsonPropertyName("romeLibelle")] string RomeLibelle,
    [property: JsonPropertyName("appellationlibelle")] string AppellationLibelle,
    [property: JsonPropertyName("entreprise")] Entreprise Entreprise,
    [property: JsonPropertyName("typeContrat")] string TypeContrat,
    [property: JsonPropertyName("typeContratLibelle")] string TypeContratLibelle,
    [property: JsonPropertyName("natureContrat")] string NatureContrat,
    [property: JsonPropertyName("experienceExige")] string ExperienceExige,
    [property: JsonPropertyName("experienceLibelle")] string ExperienceLibelle,
    [property: JsonPropertyName("experienceCommentaire")] string ExperienceCommentaire,
    [property: JsonPropertyName("formations")] List<Formation> Formations,
    [property: JsonPropertyName("langues")] List<Langue> Langues,
    [property: JsonPropertyName("permis")] List<Permis> Permis,
    [property: JsonPropertyName("outilsBureautiques")] string OutilsBureautiques,
    [property: JsonPropertyName("competences")] List<Competence> Competences,
    [property: JsonPropertyName("salaire")] Salaire Salaire,
    [property: JsonPropertyName("dureeTravailLibelle")] string DureeTravailLibelle,
    [property: JsonPropertyName("dureeTravailLibelleConverti")] string DureeTravailLibelleConverti,
    [property: JsonPropertyName("complementExercice")] string ComplementExercice,
    [property: JsonPropertyName("conditionExercice")] string ConditionExercice,
    [property: JsonPropertyName("alternance")] bool Alternance,
    [property: JsonPropertyName("contact")] Contact Contact,
    [property: JsonPropertyName("agence")] Agence Agence,
    [property: JsonPropertyName("nombrePostes")] int NombrePostes,
    [property: JsonPropertyName("accessibleTH")] bool AccessibleTH,
    [property: JsonPropertyName("deplacementCode")] string DeplacementCode,
    [property: JsonPropertyName("deplacementLibelle")] string DeplacementLibelle,
    [property: JsonPropertyName("qualificationCode")] string QualificationCode,
    [property: JsonPropertyName("qualificationLibelle")] string QualificationLibelle,
    [property: JsonPropertyName("codeNAF")] string CodeNAF,
    [property: JsonPropertyName("secteurActivite")] string SecteurActivite,
    [property: JsonPropertyName("secteurActiviteLibelle")] string SecteurActiviteLibelle,
    [property: JsonPropertyName("qualitesProfessionnelles")] List<QualiteProfessionnelle> QualitesProfessionnelles,
    [property: JsonPropertyName("trancheEffectifEtab")] string TrancheEffectifEtab,
    [property: JsonPropertyName("origineOffre")] OrigineOffre OrigineOffre,
    [property: JsonPropertyName("offresManqueCandidats")] bool OffresManqueCandidats,
    [property: JsonPropertyName("contexteTravail")] ContexteTravail ContexteTravail
);

public record LieuTravail(
    [property: JsonPropertyName("libelle")] string Libelle,
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude,
    [property: JsonPropertyName("codePostal")] string CodePostal,
    [property: JsonPropertyName("commune")] string Commune
);

public record Entreprise(
    [property: JsonPropertyName("nom")] string Nom,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("logo")] string Logo,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("entrepriseAdaptee")] bool EntrepriseAdaptee
);

public record Formation(
    [property: JsonPropertyName("codeFormation")] string CodeFormation,
    [property: JsonPropertyName("domaineLibelle")] string DomaineLibelle,
    [property: JsonPropertyName("niveauLibelle")] string NiveauLibelle,
    [property: JsonPropertyName("commentaire")] string Commentaire,
    [property: JsonPropertyName("exigence")] string Exigence
);

public record Langue(
    [property: JsonPropertyName("libelle")] string Libelle,
    [property: JsonPropertyName("exigence")] string Exigence
);

public record Permis(
    [property: JsonPropertyName("libelle")] string Libelle,
    [property: JsonPropertyName("exigence")] string Exigence
);

public record Competence(
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("libelle")] string Libelle,
    [property: JsonPropertyName("exigence")] string Exigence
);

public record Salaire(
    [property: JsonPropertyName("libelle")] string Libelle,
    [property: JsonPropertyName("commentaire")] string Commentaire,
    [property: JsonPropertyName("complement1")] string Complement1,
    [property: JsonPropertyName("complement2")] string Complement2
);

public record Contact(
    [property: JsonPropertyName("nom")] string Nom,
    [property: JsonPropertyName("coordonnees1")] string Coordonnees1,
    [property: JsonPropertyName("coordonnees2")] string Coordonnees2,
    [property: JsonPropertyName("coordonnees3")] string Coordonnees3,
    [property: JsonPropertyName("telephone")] string Telephone,
    [property: JsonPropertyName("courriel")] string Courriel,
    [property: JsonPropertyName("commentaire")] string Commentaire,
    [property: JsonPropertyName("urlRecruteur")] string UrlRecruteur,
    [property: JsonPropertyName("urlPostulation")] string UrlPostulation
);

public record Agence(
    [property: JsonPropertyName("telephone")] string Telephone,
    [property: JsonPropertyName("courriel")] string Courriel
);

public record QualiteProfessionnelle(
    [property: JsonPropertyName("libelle")] string Libelle,
    [property: JsonPropertyName("description")] string Description
);

public record OrigineOffre(
    [property: JsonPropertyName("origine")] string Origine,
    [property: JsonPropertyName("urlOrigine")] string UrlOrigine,
    [property: JsonPropertyName("partenaires")] List<Partenaire> Partenaires
);

public record Partenaire(
    [property: JsonPropertyName("nom")] string Nom,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("logo")] string Logo
);

public record ContexteTravail(
    [property: JsonPropertyName("horaires")] string Horaires,
    [property: JsonPropertyName("conditionsExercice")] string ConditionsExercice
);

public record FiltrePossible(
    [property: JsonPropertyName("filtre")] string Filtre,
    [property: JsonPropertyName("agregation")] List<Agregation> Agregation
);

public record Agregation(
    [property: JsonPropertyName("valeurPossible")] string ValeurPossible,
    [property: JsonPropertyName("nbResultats")] int NbResultats
);



