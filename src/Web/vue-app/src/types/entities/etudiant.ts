import {IEntity} from "@/types/entities/entity";
import {Guid} from "@/types";

export class Etudiant implements IEntity {
    id?: Guid;
    estBeneficiaireRenforcementFr?: boolean;
    tourAdmission?: number;
    statutImmigration?: StatutImmigration;
    population?: Population;
    sanction?: Sanction;
    moyenneGeneraleAuSecondaire?: number;
    estAssujetiAuR18?: boolean;
}
