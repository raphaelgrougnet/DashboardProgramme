import {ICoursService} from "@/injection/interfaces";
import {injectable} from "inversify";
import {ApiService} from "./apiService";
import {AxiosError, AxiosResponse} from "axios";
import {Cours, Etudiant, Guid} from "@/types";

@injectable()
export class CoursService extends ApiService implements ICoursService {
    public async getCoursForSessionOfProgramme(idProgramme: Guid, idSession: Guid) {
        const response = await this
            ._httpClient
            .get<AxiosResponse<Cours[]>>(`${process.env.VUE_APP_API_BASE_URL}/api/programmes/${idProgramme}/sessions/${idSession}/cours`)
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<Cours[]>;
            });
        return response.data as Cours[]
    }
    

}