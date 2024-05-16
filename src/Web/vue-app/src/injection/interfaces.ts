// eslint-disable-next-line @typescript-eslint/no-empty-interface
import {SpeAggregate, SucceededOrNotResponse} from "@/types/responses"
import {ICreateProgrammeRequest, IEditMemberRequest, IEditProgrammeRequest} from "@/types/requests"
import {Cours, IAuthenticatedMember, Member, SessionEtude} from "@/types/entities"
import {Programme} from "@/types/entities/programme";
import {Etudiant} from "@/types/entities/etudiant";
import {ICreateMemberRequest} from "@/types/requests/createMemberRequest";
import {Guid} from "@/types";
import {Note} from "@/types/enums/note";

export interface IApiService {
    headersWithJsonContentType(): any

    headersWithFormDataContentType(): any

    buildEmptyBody(): string
}

export interface IMemberService {
    getCurrentMember(): Promise<IAuthenticatedMember | undefined>

    getMember(memberId: string): Promise<Member>

    getAllMembers(): Promise<Member[]>

    createMember(request: ICreateMemberRequest): Promise<SucceededOrNotResponse>

    editMember(request: IEditMemberRequest): Promise<SucceededOrNotResponse>

    deleteMember(memberId: string): Promise<SucceededOrNotResponse>

}

export interface IProgrammeService {
    getAllProgrammes(): Promise<Programme[]>

    getProgramme(programmeId: string): Promise<Programme>

    createProgramme(request: ICreateProgrammeRequest): Promise<SucceededOrNotResponse>

    editProgramme(request: IEditProgrammeRequest): Promise<SucceededOrNotResponse>

    deleteProgramme(programmeId: string): Promise<SucceededOrNotResponse>
}

export interface IEtudiantService {
    getAllEtudiants(): Promise<Etudiant[]>

    getEtudiant(etudiantId: string): Promise<Etudiant>

    getStudentsForCours(idSession : Guid, idCours : Guid, idProgramme: Guid): Promise<number>

    getAverageGradesForStudentsInClass(idSession : Guid, idCours : Guid, idProgramme: Guid): Promise<Record<Note, number>>
}

export interface ISessionEtudeService {
    getSessionEtudesForProgramme(programmeId: string): Promise<SessionEtude[]>
}

export interface ICoursAssisteService {
    getReussiteParSessionPourProgramme(programmeId: Guid): Promise<Record<string, Record<Note, number>>>

    getReussiteParCoursPourProgrammeEtSession(programmeId: Guid, sessionEtudeId: Guid): Promise<Record<string, Record<Note, number>>>

    getReussitePourCoursDeSessionDeProgramme
    (programmeId: Guid, sessionEtudeId: Guid, coursId: Guid): Promise<Record<Note, number>>

    getReussitePourCoursEntre2Sessions
    (programmeId: Guid, coursId: Guid, sessionDebutId: Guid, sessionFinId: Guid): Promise<Record<Note, number>>

}

export interface ICoursService {
    getCoursForSessionOfProgramme(idProgramme: Guid, idSession: Guid): Promise<Cours[]>
}

export interface ICorrelationsServices {
    getValeurP(critere: string, cours: string): Promise<number>

    getValeurPGENMELS(cours: string): Promise<number>

    getValeurPTourAdmission(cours: string, tourAdmission: number): Promise<number>

    getValeurPInternational(cours: string, etudiantInternational: string): Promise<number>
}

export interface ISpeQueryService {
    getSpeAggregatePourProgramme(programmeId: Guid): Promise<SpeAggregate>
}

export interface IImportDataService {
    importData(request: string): Promise<SucceededOrNotResponse>
}

export interface IPortraitEtudiantService {
    getSeriesData(idProgramme: string): Promise<{ rt: number[], r18: number[], sa: number[] }>
}
