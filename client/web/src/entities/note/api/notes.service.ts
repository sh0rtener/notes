import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../shared/config/environment";
import { ApiResponse } from "../../../shared/api/api-response.model";
import { Note } from "../model/note.model";

@Injectable({
    providedIn: 'root'
})
export class NoteApiService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiUrl}/notes`;

    getNotes() {
        return this.http.get<ApiResponse<Note[]>>(`${this.apiUrl}/all`);
    }

    completeNote(id: number) {
        return this.http.patch(`${this.apiUrl}/${id}/complete`, {});
    }
}