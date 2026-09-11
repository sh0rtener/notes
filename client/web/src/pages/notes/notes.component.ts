import { Component, inject, OnInit, signal } from "@angular/core";
import { Note, NoteApiService, NoteCardComponent } from "../../entities/note";
import { NotesListComponent } from "../../widgets/notes-list/ui/notes-list.component";


@Component({
    selector: 'app-landing',
    templateUrl: './notes.component.html',
    styleUrl: './notes.component.scss',
    imports: [NotesListComponent]
})

export class NotesComponent implements OnInit {

    private readonly noteApi = inject(NoteApiService);
    readonly notes = signal<Note[]>([]);

    ngOnInit(): void {
        this.noteApi.getNotes().subscribe(response => {
            this.notes.set(response.data ?? []);
        });
    }
}