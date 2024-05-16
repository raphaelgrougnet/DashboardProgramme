import {IEtudiantService} from "@/injection/interfaces";
import {injectable} from "inversify";
import {ApiService} from "./apiService";
import {AxiosError, AxiosResponse} from "axios";
import {Etudiant} from "@/types/entities/etudiant";
import {Guid} from "@/types";
import {Note} from "@/types/enums/note";

@injectable()
export class EtudiantService extends ApiService implements IEtudiantService {
    public async getAllEtudiants() {
        const response = await this
            ._httpClient
            .get<AxiosResponse<Etudiant[]>>(`${process.env.VUE_APP_API_BASE_URL}/api/Etudiants`)
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<Etudiant[]>
            });
        return response.data as Etudiant[]
    }

    public async getEtudiant(EtudiantId: string) {
        const response = await this
            ._httpClient
            .get<AxiosResponse<Etudiant>>(`${process.env.VUE_APP_API_BASE_URL}/api/Etudiants/${EtudiantId}`)
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<Etudiant>;
            });
        return response.data as Etudiant
    }

    public async getStudentsForCours(idSession: Guid, idCours: Guid, idProgramme: Guid)
    {
        const response = await this
            ._httpClient
            .get<AxiosResponse<number>>(`${process.env.VUE_APP_API_BASE_URL}/api/programmes/${idProgramme}/sessions/${idSession}/cours/${idCours}/etudiants`)
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<number>;
            })
        return response.data as number
    }

    public async getAverageGradesForStudentsInClass(idSession: Guid, idCours: Guid, idProgramme: Guid) : Promise<Record<Note, number>>
    {
        const response = await this
            ._httpClient
            .get<AxiosResponse<Record<Note, number>>>(`${process.env.VUE_APP_API_BASE_URL}/api/programmes/${idProgramme}/sessions/${idSession}/cours/${idCours}/notes`)
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<Record<Note, number>>;
            })
        return response.data as Record<Note, number>
    }
}