import { Component, input, model } from "@angular/core";
import { Note } from "../../model/note.model";
import { NoteStatusBadgeComponent } from "../note-status-badge";
import { DatePipe } from "@angular/common";
import { NoteStatus } from "../../model/notestatus.model";

@Component({
    selector: 'app-note-card',
    templateUrl: './note-card.component.html',
    styleUrl: './note-card.component.scss',
    imports: [NoteStatusBadgeComponent, DatePipe]
})

export class NoteCardComponent {
    note = model.required<Note>();

    completeNote() {
        this.note.update(note => ({
            ...note,
            status: NoteStatus.completed
        }))
    }

    isComplete() {
        return this.note().status === NoteStatus.completed;
    }
}