import {injectable} from "inversify"

import "@/extensions/date.extensions"
import {ApiService} from "@/services/apiService"
import {ICorrelationsServices} from "@/injection/interfaces"

@injectable()
export class CorrelationsServices extends ApiService implements ICorrelationsServices {
    public async getValeurP(critere: string, cours: string): Promise<number> {
        try {
            const response = await this
                ._httpClient
                .get<number>(`${process.env.VUE_APP_API_BASE_URL}/api/correlations/calculate-p-value/${cours}/${critere}`);
            // Extract the required data from response.data and return it
            const pValue = response.data;
            return pValue;
        } catch (error) {
            return Promise.reject(error)
        }
    }

    public async getValeurPGENMELS(cours: string): Promise<number> {
        try {
            const response = await this
                ._httpClient
                .get<number>(`${process.env.VUE_APP_API_BASE_URL}/api/correlations/calculate-p-value-genmels/${cours}`);
            // Extract the required data from response.data and return it
            const pValue = response.data;
            return pValue;
        } catch (error) {
            return Promise.reject(error)
        }
    }

    public async getValeurPTourAdmission(cours: string, tourAdmission: number): Promise<number> {
        try {
            const response = await this
                ._httpClient
                .get<number>(`${process.env.VUE_APP_API_BASE_URL}/api/correlations/calculate-p-value-tour-admission/${cours}/${tourAdmission}`);
            // Extract the required data from response.data and return it
            const pValue = response.data;
            return pValue;
        } catch (error) {
            return Promise.reject(error)
        }
    }

    public async getValeurPInternational(cours: string, etudiantInternational: string): Promise<number> {
        try {
            const response = await this
                ._httpClient
                .get<number>(`${process.env.VUE_APP_API_BASE_URL}/api/correlations/calculate-p-value-international/${cours}/${etudiantInternational}`);
            // Extract the required data from response.data and return it
            const pValue = response.data;
            return pValue;
        } catch (error) {
            return Promise.reject(error)
        }
    }

}