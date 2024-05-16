import {IEntity} from "@/types/entities/entity";

export interface IPerson extends IEntity {
    crmId?: number
    firstName?: string
    lastName?: string
    jobTitle?: string
    email?: string
    phoneNumber?: string
    phoneExtension?: number
    apartment?: number
    street?: string
    city?: string
    zipCode?: string
}