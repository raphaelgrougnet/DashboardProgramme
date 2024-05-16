import {IEntity} from "@/types/entities/entity";
import {Guid} from "@/types";

export class Programme implements IEntity {
    id?: Guid;
    nom?: string;
    numero?: string;
}