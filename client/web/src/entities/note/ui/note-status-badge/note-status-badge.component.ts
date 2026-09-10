import { Component, input } from "@angular/core";
import { NgClass } from '@angular/common';
import { NoteStatusPipe } from "./note-status-pipe";

@Component({
    selector: 'app-note-status-badge',
    templateUrl: './note-status-badge.component.html',
    styleUrl: './note-status-badge.component.scss',
    imports: [NgClass, NoteStatusPipe]
})

export class NoteStatusBadgeComponent {
    status = input.required<string>();
}