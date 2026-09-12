import { Component, inject, input, model } from "@angular/core";
import { Note } from "../../model/note.model";
import { NoteStatusBadgeComponent } from "../note-status-badge";
import { DatePipe } from "@angular/common";
import { NoteStatus } from "../../model/notestatus.model";
import { NoteApiService } from "../../api/notes.service";

@Component({
    selector: 'app-note-card',
    templateUrl: './note-card.component.html',
    styleUrl: './note-card.component.scss',
    imports: [NoteStatusBadgeComponent, DatePipe]
})

export class NoteCardComponent {
    private readonly service = inject(NoteApiService);

    note = model.required<Note>();

    completeNote() {
        this.service.completeNote(this.note().id).subscribe();
        this.note.update(note => ({
            ...note,
            status: NoteStatus.completed
        }))
    }

    isComplete() {
        return this.note().status === NoteStatus.completed;
    }
}