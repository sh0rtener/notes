import { Component, inject, OnInit, signal } from "@angular/core";
import { Note, NoteApiService, NoteCardComponent } from "../../entities/note";
import { NotesListComponent } from "../../widgets/notes-list/ui/notes-list.component";


@Component({
    selector: 'app-landing',
    templateUrl: './landing.component.html',
    styleUrl: './landing.component.scss',
    imports: [NotesListComponent]
})

export class LandingComponent implements OnInit {

    private readonly noteApi = inject(NoteApiService);
    readonly notes = signal<Note[]>([]);

    ngOnInit(): void {
        this.noteApi.getNotes().subscribe(response => {
            this.notes.set(response.data ?? []);
        });
    }
}