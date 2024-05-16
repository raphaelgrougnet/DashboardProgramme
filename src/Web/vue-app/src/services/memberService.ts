import {AxiosError, AxiosResponse} from "axios"
import {injectable} from "inversify"

import "@/extensions/date.extensions"
import {ApiService} from "@/services/apiService"
import {IMemberService} from "@/injection/interfaces"
import {IAuthenticatedMember} from "@/types/entities/authenticatedMember"
import {Member} from "@/types";
import {ICreateMemberRequest} from "@/types/requests/createMemberRequest";
import {SucceededOrNotResponse} from "@/types/responses";
import {IEditMemberRequest} from "@/types/requests";

@injectable()
export class MemberService extends ApiService implements IMemberService {
    public async getCurrentMember(): Promise<IAuthenticatedMember | undefined> {
        try {
            const response = await this
                ._httpClient
                .get<IAuthenticatedMember, AxiosResponse<IAuthenticatedMember>>(`${process.env.VUE_APP_API_BASE_URL}/api/members/me`);
            return response.data
        } catch (error) {
            return Promise.reject(error)
        }
    }

    public async getMember(memberId: string) {
        const response = await this
            ._httpClient
            .get<AxiosResponse<Member>>(`${process.env.VUE_APP_API_BASE_URL}/api/admin/members/${memberId}`)
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<Member>;
            });
        return response.data as Member
    }

    public async getAllMembers() {
        const response = await this
            ._httpClient
            .get<AxiosResponse<Member[]>>(`${process.env.VUE_APP_API_BASE_URL}/api/admin/members`)
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<Member[]>
            });
        return response.data as Member[]
    }

    public async createMember(request: ICreateMemberRequest) {
        const response = await this
            ._httpClient
            .post<ICreateMemberRequest, AxiosResponse<SucceededOrNotResponse>>(
                `${process.env.VUE_APP_API_BASE_URL}/api/admin/members`,
                request,
                this.headersWithJsonContentType())
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<SucceededOrNotResponse>
            });
        const succeededOrNotResponse = response.data as SucceededOrNotResponse;
        return new SucceededOrNotResponse(succeededOrNotResponse.succeeded, succeededOrNotResponse.errors)
    }

    public async editMember(request: IEditMemberRequest) {
        const response = await this
            ._httpClient
            .patch<IEditMemberRequest, AxiosResponse<SucceededOrNotResponse>>(
                `${process.env.VUE_APP_API_BASE_URL}/api/admin/members`,
                request,
                this.headersWithJsonContentType())
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<SucceededOrNotResponse>
            });
        const succeededOrNotResponse = response.data as SucceededOrNotResponse;
        return new SucceededOrNotResponse(succeededOrNotResponse.succeeded, succeededOrNotResponse.errors)
    }

    public async deleteMember(memberId:string){
        const response = await this
            ._httpClient
            .delete<AxiosResponse<any>>(`${process.env.VUE_APP_API_BASE_URL}/api/admin/members/${memberId}`)
            .catch(function (error: AxiosError) {
                return error.response as AxiosResponse<SucceededOrNotResponse>
            });
        return new SucceededOrNotResponse(response.status === 204)
    }

} 