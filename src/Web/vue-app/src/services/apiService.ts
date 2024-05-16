import {AxiosInstance} from "axios";
import {inject, injectable} from "inversify";

import {IApiService} from "@/injection/interfaces";
import {TYPES} from "@/injection/types";
import "@/extensions/date.extensions";

@injectable()
export class ApiService implements IApiService {
    _httpClient: AxiosInstance;

    constructor(@inject(TYPES.AxiosInstance) httpClient: AxiosInstance) {
        this._httpClient = httpClient;
    }

    public headersWithJsonContentType() {
        return {
            headers: {
                "Content-Type": 'application/json',
            },
        };
    }

    public headersWithTextPlainContentType() {
        return {
            headers: {
                "Content-Type": 'text/plain',
            },
        };
    }

    public headersWithFormDataContentType() {
        return {
            headers: {
                "Content-Type": '"multipart/form-data"',
            },
        };
    }

    public buildEmptyBody() {
        return '{}'
    }
}