import {Saison} from "@/types/enums/Saison";
import {IEntity} from "@/types/entities/entity";
import {Guid} from "@/types";

export class SessionEtude implements IEntity {
    id?: Guid;
    annee?: number;
    saison?: Saison;
    slug?: string;
    ordre?: number;
}