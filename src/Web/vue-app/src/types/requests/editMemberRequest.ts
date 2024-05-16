export interface IEditMemberRequest {
    id?: string;
    firstName?: string;
    lastName?: string;
    email?: string;
    password?: string;
    role?: string;
    programmes?: string[];
}