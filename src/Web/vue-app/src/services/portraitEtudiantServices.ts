import {injectable} from "inversify"

import "@/extensions/date.extensions"
import {ApiService} from "@/services/apiService"
import {IPortraitEtudiantService} from "@/injection/interfaces"


@injectable()
export class PortraitEtudiantServices extends ApiService implements IPortraitEtudiantService {
    public async getSeriesData(idProgramme: string): Promise<{ rt: number[], r18: number[], sa: number[] }> {
        try {
            const response = await this
                ._httpClient
                .get<{
                    rt: number[],
                    r18: number[],
                    sa: number[]
                }>(`${process.env.VUE_APP_API_BASE_URL}/api/programmes/${idProgramme}/portrait-etudiant`);
            const dataSeries = response.data;
            return dataSeries;
        } catch (error) {
            return Promise.reject(error)
        }
    }


}