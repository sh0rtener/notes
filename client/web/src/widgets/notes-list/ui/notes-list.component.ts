import { Component, effect, input, model, signal } from "@angular/core";
import { Note, NoteCardComponent } from "../../../entities/note";
import { ModalComponent } from "../../../shared/ui/modal/modal.component";
import { CreateTaskComponent } from "../../../features/create-task/create-task.component";
import { PrimaryButtonComponent } from "../../../shared/ui/button/button/button.component";
import { CreateTaskModel } from "../../../entities/note/model/create-task.model";

@Component({
    selector: 'app-notes-list',
    templateUrl: './notes-list.component.html',
    styleUrl: './notes-list.component.scss',
    imports: [NoteCardComponent, ModalComponent, CreateTaskComponent, PrimaryButtonComponent]
})

export class NotesListComponent {
    notes = model.required<Note[]>();
    isCreateTaskModal = false;

    onNoteCreated(note: Note) {
        this.notes.update(notes => [...notes, note])
    }

    createTask() {
        this.isCreateTaskModal = true;
    }
}