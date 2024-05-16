export type SPE = number;
export type NbEtudiants = number;

export type SpePourSession = Record<SPE, NbEtudiants>;

export class SpeAggregate {
    sessionActuelle = {} as SpePourSession;
    moyenneSessionsPrecedentes = {} as SpePourSession;
    troisDernieresSessions = [] as SpePourSession[];
}