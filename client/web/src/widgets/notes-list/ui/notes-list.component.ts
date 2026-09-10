import { Component, input } from "@angular/core";
import { Note, NoteCardComponent } from "../../../entities/note";

@Component({
    selector: 'app-notes-list',
    templateUrl: './notes-list.component.html',
    styleUrl: './notes-list.component.scss',
    imports: [NoteCardComponent]
})

export class NotesListComponent {
    notes = input.required<Note[]>();
}