import {IEntity} from "@/types/entities/entity";

export interface IAuthenticatedMember extends IEntity {
    firstName: string
    lastName: string
    jobTitle?: string
    email?: string
    phoneNumber?: string
    phoneExtension?: number
    apartment?: number
    street?: string
    city?: string
    zipCode?: string
    roles: string[]
    organizationId: string
    enterpriseNumber?: string
    memberNumber?: string
}