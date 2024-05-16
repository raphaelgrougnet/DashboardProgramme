import {IProgrammeService} from "@/injection/interfaces";
import {injectable} from "inversify";
import {ApiService} from "./apiService";
import {AxiosError, AxiosResponse} from "axios";
import {Programme} from "@/types/entities/programme";
import {ICreateProgrammeRequest, IEditProgrammeRequest} from "@/types/requests";
import {SucceededOrNotResponse} from "@/types/responses";

@injectable()
export class ProgrammeService extends ApiService implements IProgrammeService {
    public async getAllProgrammes() {
        const response = await this
            ._httpClient
            .get<AxiosResponse<Programme[]>>(`${process.env.VUE_APP_API_BASE_URL}/api/Programmes`)
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<Programme[]>
            });
        return response.data as Programme[]
    }

    public async getProgramme(ProgrammeId: string) {
        const response = await this
            ._httpClient
            .get<AxiosResponse<Programme>>(`${process.env.VUE_APP_API_BASE_URL}/api/Programmes/${ProgrammeId}`)
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<Programme>;
            });
        return response.data as Programme
    }

    public async deleteProgramme(ProgrammeId: string) {
        const response = await this
            ._httpClient
            .delete<AxiosResponse<any>>(`${process.env.VUE_APP_API_BASE_URL}/api/programmes/${ProgrammeId}`)
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<SucceededOrNotResponse>
            });
        return new SucceededOrNotResponse(response.status === 204)

    }

    public async createProgramme(request: ICreateProgrammeRequest) {
        const response = await this
            ._httpClient
            .post<ICreateProgrammeRequest, AxiosResponse<any>>(
                `${process.env.VUE_APP_API_BASE_URL}/api/programmes`,
                request,
                this.headersWithJsonContentType())
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<SucceededOrNotResponse>
            });
        const succeededOrNotResponse = response.data as SucceededOrNotResponse;
        return new SucceededOrNotResponse(succeededOrNotResponse.succeeded, succeededOrNotResponse.errors)
    }

    public async editProgramme(request: IEditProgrammeRequest) {
        const response = await this
            ._httpClient
            .put<ICreateProgrammeRequest, AxiosResponse<any>>(
                `${process.env.VUE_APP_API_BASE_URL}/api/programmes/${request.id}`,
                request,
                this.headersWithJsonContentType())
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<SucceededOrNotResponse>
            });
        const succeededOrNotResponse = response.data as SucceededOrNotResponse;
        return new SucceededOrNotResponse(succeededOrNotResponse.succeeded, succeededOrNotResponse.errors)
    }
}