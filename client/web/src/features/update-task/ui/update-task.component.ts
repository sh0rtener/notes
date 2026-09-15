import { Component, effect, inject, model, output } from "@angular/core";
import { createUpdateTaskDescriptionForm, createUpdateTaskNameForm } from "../model/update-task.form";
import { Note, NoteApiService } from "../../../entities/note";
import { ToastrService } from "ngx-toastr";
import { PrimaryButtonComponent } from "../../../shared/ui/button/button/button.component";
import { InputComponent } from "../../../shared/ui/input/input/input.component";
import { ReactiveFormsModule, } from "@angular/forms";
import { Router } from "@angular/router";

@Component({
    selector: 'app-update-task-form',
    templateUrl: './update-task.component.html',
    styleUrl: './update-task.component.scss',
    imports: [PrimaryButtonComponent, ReactiveFormsModule]
})

export class UpdateTaskComponent {
    model = model.required<Note>();
    private readonly toastr = inject(ToastrService);
    private noteService = inject(NoteApiService);
    private router = inject(Router)

    completeNote() {
        this.noteService.completeNote(this.model().id).subscribe({
            next: (x) => {
                this.toastr.success('Заметка успешно завершена!')
                window.location.reload();
            },
            error: (e: Error) => {
                this.toastr.error(e.message, 'Возникла ошибка во время авторизации')

            }
        })
    }

    onWorkNote() {
        this.noteService.getToWork(this.model().id).subscribe({
            next: (x) => {
                this.toastr.success('Заметка успешно удалена!')
                window.location.reload();
            },
            error: (e: Error) => {
                this.toastr.error(e.message, 'Возникла ошибка во время авторизации')

            }
        })
    }

    
    removeNote() {
        this.noteService.remove(this.model().id).subscribe({
            next: (x) => {
                this.toastr.success('Заметка успешно удалена!')
                this.router.navigate(['/'])
            },
            error: (e: Error) => {
                this.toastr.error(e.message, 'Возникла ошибка во время авторизации')

            }
        })
    }

}