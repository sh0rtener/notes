import { Component } from "@angular/core";
import { Note, NoteCardComponent } from "../../entities/note";
import { NotesListComponent } from "../../widgets/notes-list/ui/notes-list.component";


@Component({
    selector: 'app-landing',
    templateUrl: './landing.component.html',
    styleUrl: './landing.component.scss',
    imports: [NoteCardComponent, NotesListComponent]
})

export class LandingComponent {
    notes: Note[] =
        [
            { id: 1, name: "Задача #1", description: "", status: "onwork", createdAt: new Date(), updatedAt: new Date() },
            { id: 2, name: "Задача #2", description: "", status: "onwork", createdAt: new Date(), updatedAt: new Date() },
            { id: 3, name: "Задача #3", description: "", status: "onwork", createdAt: new Date(), updatedAt: new Date() },
        ]

}