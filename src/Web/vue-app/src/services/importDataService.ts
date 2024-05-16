import {injectable} from "inversify";
import {ApiService} from "@/services/apiService";
import {IImportDataService} from "@/injection/interfaces";
import {SucceededOrNotResponse} from "@/types/responses";
import {AxiosError, AxiosResponse} from "axios";

@injectable()
export class ImportDataService extends ApiService implements IImportDataService {
    public async importData(request: string) {
        console.log("importData service: ", request);
        const response = await this
            ._httpClient
            .post<string, AxiosResponse<any>>(
                `${process.env.VUE_APP_API_BASE_URL}/api/importData`,
                request,
                this.headersWithTextPlainContentType())
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<SucceededOrNotResponse>
            });
        const succeededOrNotResponse = response.data as SucceededOrNotResponse;
        return new SucceededOrNotResponse(succeededOrNotResponse.succeeded, succeededOrNotResponse.errors)

    }

} 