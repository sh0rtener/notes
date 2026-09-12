import { inject, Injectable } from "@angular/core";
import { environment } from "../../../shared/config/environment";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { RegisterModel } from "../model/register.model";
import { catchError, throwError } from "rxjs";
import { ApiResponse } from "../../../shared/api/api-response.model";

@Injectable({
    providedIn: 'root'
})
export class RegisterService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiUrl}/users`;

    registerUser(model: RegisterModel) {
        return this.http.post<ApiResponse<object>>(`${this.apiUrl}`, model).pipe(
            catchError((error: HttpErrorResponse) => {
                const response = error.error as ApiResponse<object>;

                switch (error.status) {
                    case 400:
                        return throwError(() => new Error(response.Message ?? 'Ошибка на стороне клиента!'))
                    case 500:
                        return throwError(() => new Error('Ошибка сервера'))
                    default:
                        return throwError(() => new Error(
                            'Не удалось выполнить регистрацию'
                        ));
                }
            })
        );
    }
}