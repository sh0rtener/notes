import { afterNextRender, Component, inject, OnInit, signal } from "@angular/core";
import { Note, NoteApiService, NoteCardComponent } from "../../entities/note";
import { NotesListComponent } from "../../widgets/notes-list/ui/notes-list.component";
import { HeaderComponent } from "../../widgets/header";


@Component({
    selector: 'app-landing',
    templateUrl: './notes.component.html',
    styleUrl: './notes.component.scss',
    imports: [NotesListComponent, HeaderComponent]
})

export class NotesComponent {

    private readonly noteApi = inject(NoteApiService);
    readonly notes = signal<Note[]>([]);

    constructor() {
        afterNextRender(() => {
            this.loadNotes();
        });
    }

    private loadNotes() {
        this.noteApi.getNotes().subscribe({
            next: response => {
                this.notes.set(response.data ?? []);
            },
            error: error => {
            }
        });
    }
}