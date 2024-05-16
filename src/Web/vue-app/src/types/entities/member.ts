import {IPerson} from "@/types/entities/person";
import {Guid} from "@/types";

export class Member implements IPerson {
    id?: Guid;
    crmId?: number;
    firstName?: string;
    lastName?: string;
    jobTitle?: string;
    email?: string;
    password?: string;
    phoneNumber?: string;
    phoneExtension?: number;
    apartment?: number;
    street?: string;
    city?: string;
    zipCode?: string;
    userId?: string;
    organizationId?: string;
    roles?: string[];
    active?: string;
    programmes?: Guid[];
}