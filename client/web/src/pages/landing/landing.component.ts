import { Component } from "@angular/core";
import { Note, NoteCardComponent } from "../../entities/note";


@Component({
    selector: 'app-landing',
    templateUrl: './landing.component.html',
    styleUrl: './landing.component.scss',
    imports: [NoteCardComponent]
})

export class LandingComponent {
    note: Note = { name: "Задача #1", description: "", status: "onwork", createdAt: new Date(), updatedAt: new Date() }
}