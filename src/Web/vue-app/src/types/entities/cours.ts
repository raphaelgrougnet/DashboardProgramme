import {IEntity} from "@/types/entities/entity";
import {Guid} from "@/types";

export class Cours implements IEntity {
    id?: Guid;
    code?: string;
    nom?: string;
}