import {ISessionEtudeService} from "@/injection/interfaces";
import {injectable} from "inversify";
import {ApiService} from "./apiService";
import {AxiosError, AxiosResponse} from "axios";
import {SessionEtude} from "@/types";

@injectable()
export class SessionEtudeService extends ApiService implements ISessionEtudeService {
    public async getSessionEtudesForProgramme(ProgrammeId: string) {
        const response = await this
            ._httpClient
            .get<AxiosResponse<SessionEtude[]>>(`${process.env.VUE_APP_API_BASE_URL}/api/Programmes/${ProgrammeId}/SessionEtudes`)
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<SessionEtude[]>;
            });
        return response.data as SessionEtude[]
    }
}