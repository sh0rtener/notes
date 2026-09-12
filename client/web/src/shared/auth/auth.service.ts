import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../config/environment";
import { AuthModel } from "./auth.model";
import { ApiResponse } from "../api/api-response.model";
import { TokenModel } from "./token.model";
import { catchError, tap, throwError } from "rxjs";
import { AuthApiService } from "./auth.api.service";

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private readonly authApiService = inject(AuthApiService);

    login(model: AuthModel) {
        return this.authApiService.signIn(model)
            .pipe(
                tap(r => {
                    console.log(r)
                    localStorage.setItem('access_token', r.data?.accessToken!);
                    localStorage.setItem('refresh_token', r.data?.refreshToken!);
                    localStorage.setItem('expiresAt', r.data?.expiresAt.toString()!);
                    localStorage.setItem('refreshExpiresAt', r.data?.refreshExpiresAt.toString()!);
                }),
                catchError((error: HttpErrorResponse) => {
                    const response = error.error as ApiResponse<object>;

                    switch (error.status) {
                        case 400:
                            return throwError(() => new Error(response.message ?? 'Ошибка на стороне клиента!'))
                        case 500:
                            return throwError(() => new Error('Ошибка сервера'))
                        default:
                            return throwError(() => new Error(
                                'Не удалось выполнить вход'
                            ));
                    }
                })
            )
    }

    refresh(refreshToken: string) {
        return this.authApiService.refresh(refreshToken)
            .pipe(
                tap(r => {
                    localStorage.setItem('access_token', r.data?.accessToken!);
                    localStorage.setItem('refresh_token', r.data?.refreshToken!);
                    localStorage.setItem('expiresAt', r.data?.expiresAt.toString()!);
                    localStorage.setItem('refreshExpiresAt', r.data?.refreshExpiresAt.toString()!);
                })
            )
    }

    getToken() {
        return localStorage.getItem('access_token')
    }

    getRefreshToken() {
        return localStorage.getItem('refresh_token')
    }

    logout() {
        localStorage.removeItem('access_token');
        localStorage.removeItem('refresh_token');
        localStorage.removeItem('expiresAt');
        localStorage.removeItem('refreshExpiresAt');
    }

    canRefresh() {
        const refreshExpires = localStorage.getItem('refreshExpiresAt')

        if (!refreshExpires) { return false; }

        return new Date(refreshExpires).getTime() > Date.now();
    }

    isAuth() {
        const expiresAt = localStorage.getItem('expiresAt');
        if (!expiresAt) { return false; }

        return this.getToken() !== null && (new Date(expiresAt).getTime() > Date.now());
    }
}