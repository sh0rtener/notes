import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../shared/config/environment";
import { ApiResponse } from "../../../shared/api/api-response.model";
import { UserModel } from "../model/user.model";

@Injectable({
    providedIn: 'root'
})

export class UserService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiUrl}/users`;

    getUser() {
        return this.http.get<ApiResponse<UserModel>>(`${this.apiUrl}`);
    }
}