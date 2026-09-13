import { Component, effect, input, model, signal } from "@angular/core";
import { Note, NoteCardComponent } from "../../../entities/note";
import { ModalComponent } from "../../../shared/ui/modal/modal.component";
import { CreateTaskComponent } from "../../../features/create-task/create-task.component";
import { PrimaryButtonComponent } from "../../../shared/ui/button/button/button.component";
import { UpdateTaskComponent } from "../../../features/update-task/ui/update-task.component";

@Component({
    selector: 'app-notes-list',
    templateUrl: './notes-list.component.html',
    styleUrl: './notes-list.component.scss',
    imports: [NoteCardComponent, ModalComponent, CreateTaskComponent, PrimaryButtonComponent, UpdateTaskComponent]
})

export class NotesListComponent {
    notes = model.required<Note[]>();
    isCreateTaskModal = false;
    isUpdateTaskModal = false;
    selectedNote: Note = { id: -1, name: 'untitled', description: '', status: 'new', createdAt: new Date(), updatedAt: new Date() }

    onNoteCreated(note: Note) {
        this.notes.update(notes => [...notes, note])
    }

    onNoteUpdated(note: Note) {
        console.log(note)
        console.log(this.notes().find(x => x.id === note.id))
        this.notes.update(notes =>
            notes.map(x => x.id === note.id ? note : x))
    }

    createTask() {
        this.isCreateTaskModal = true;
    }

    updateTask(note: Note) {
        this.selectedNote = note;
        this.isUpdateTaskModal = true;
    }
}