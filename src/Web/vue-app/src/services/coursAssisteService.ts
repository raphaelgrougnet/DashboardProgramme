import {ICoursAssisteService} from "@/injection/interfaces";
import {injectable} from "inversify";
import {ApiService} from "./apiService";
import {AxiosError, AxiosResponse} from "axios";
import {Guid} from "@/types";
import {Note} from "@/types/enums/note";

@injectable()
export class CoursAssisteService extends ApiService implements ICoursAssisteService {
    public async getReussiteParSessionPourProgramme(programmeId: Guid): Promise<Record<string, Record<Note, number>>> {
        const reponse = await this
            ._httpClient
            .get<AxiosResponse<Record<string, Record<Note, number>>>>(`${process.env.VUE_APP_API_BASE_URL}/api/programmes/${programmeId}/reussite`)
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<Record<string, Record<Note, number>>>;
            });
        return reponse.data as Record<string, Record<Note, number>>;
    }

    public async getReussiteParCoursPourProgrammeEtSession(programmeId: Guid, sessionEtudeId: Guid): Promise<Record<string, Record<Note, number>>> {
        const reponse = await this
            ._httpClient
            .get<AxiosResponse<Record<string, Record<Note, number>>>>(`${process.env.VUE_APP_API_BASE_URL}/api/programmes/${programmeId}/sessions/${sessionEtudeId}/reussite`)
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<Record<string, Record<Note, number>>>;
            });
        return reponse.data as Record<string, Record<Note, number>>;
    }

    public async getReussitePourCoursDeSessionDeProgramme(programmeId: Guid, sessionEtudeId: Guid, coursId: Guid): Promise<Record<Note, number>> {
        const reponse = await this
            ._httpClient
            .get<AxiosResponse<Record<Note, number>>>(`${process.env.VUE_APP_API_BASE_URL}/api/programmes/${programmeId}/sessions/${sessionEtudeId}/cours/${coursId}/reussite`)
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<Record<Note, number>>;
            });
        return reponse.data as Record<Note, number>;
    }

    public async getReussitePourCoursEntre2Sessions(programmeId: Guid, coursId: Guid, sessionDebutId: Guid, sessionFinId: Guid): Promise<Record<Note, number>> {
        const reponse = await this
            ._httpClient
            .get<AxiosResponse<Record<Note, number>>>(`${process.env.VUE_APP_API_BASE_URL}/api/programmes/${programmeId}/comparerEntre/${sessionDebutId}/${sessionFinId}/cours/${coursId}/reussite`)
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<Record<Note, number>>;
            });
        return reponse.data as Record<Note, number>;
    }
}