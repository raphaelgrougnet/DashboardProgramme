import {ISpeQueryService} from "@/injection/interfaces";
import {injectable} from "inversify";
import {ApiService} from "./apiService";
import {AxiosError, AxiosResponse} from "axios";
import {Guid, SpeAggregate} from "@/types";

@injectable()
export class SpeQueryService extends ApiService implements ISpeQueryService {
    public async getSpeAggregatePourProgramme(programmeId: Guid) {
        const response = await this
            ._httpClient
            .get<AxiosResponse<SpeAggregate>>(`${process.env.VUE_APP_API_BASE_URL}/api/Programmes/${programmeId}/spe`)
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<SpeAggregate>;
            });
        return response.data as SpeAggregate
    }
}