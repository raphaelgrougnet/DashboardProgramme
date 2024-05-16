import {IEntity} from "@/types/entities/entity";

export interface IProduct extends IEntity {
    nameFr?: string
    nameEn?: string
    descriptionFr?: string
    descriptionEn?: string
    price?: number
    cardImage?: File
    savedCardImage?: string
    membersOnly?: boolean
}

export class Product implements IProduct {
    nameFr?: string;
    nameEn?: string;
    descriptionFr?: string;
    descriptionEn?: string;

    price?: number;
    cardImage?: File;
    savedCardImage?: string;
    membersOnly?: boolean
}