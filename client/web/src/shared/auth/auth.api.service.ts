import { inject, Injectable } from "@angular/core";
import { environment } from "../config/environment";
import { AuthModel } from "./auth.model";
import { HttpClient } from "@angular/common/http";
import { ApiResponse } from "../api/api-response.model";
import { TokenModel } from "./token.model";

@Injectable({
    providedIn: 'root'
})
export class AuthApiService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiUrl}/users`;

    signIn(model: AuthModel) {
        return this.http.post<ApiResponse<TokenModel>>(`${this.apiUrl}/sign-in`, model);
    }

    refresh(refreshToken: string) {
        const response = { refreshToken: refreshToken }
        return this.http.post<ApiResponse<TokenModel>>(`${this.apiUrl}/refresh-token`, response);
    }
}