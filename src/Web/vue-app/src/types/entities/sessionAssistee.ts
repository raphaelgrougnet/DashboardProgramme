import {Etudiant} from "@/types/entities/etudiant";
import {GrilleProgramme} from "@/types/entities/grilleProgramme";
import {SessionEtude} from "@/types/entities/sessionEtude";

export class SessionAssistee {
    etudiant?: Etudiant;
    grilleProgramme?: GrilleProgramme;
    sessionEtude?: SessionEtude;
    nbTotalHeures?: number;
    niemeSession?: number;
    estBeneficiaireServicesAdaptes?: boolean;
}
