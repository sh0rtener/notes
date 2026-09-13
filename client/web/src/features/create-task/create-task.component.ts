import { Component, inject, model, output } from "@angular/core";
import { InputComponent } from "../../shared/ui/input/input/input.component";
import { PrimaryButtonComponent } from "../../shared/ui/button/button/button.component";
import { createCreateTaskForm } from "./model/create-task.form";
import { ToastrService } from "ngx-toastr";
import { ReactiveFormsModule } from "@angular/forms";
import { CreateTaskModel } from "../../entities/note/model/create-task.model";
import { Note, NoteApiService } from "../../entities/note";

@Component({
    selector: 'app-create-task-form',
    templateUrl: './create-task.component.html',
    styleUrl: './create-task.component.scss',
    imports: [InputComponent, PrimaryButtonComponent, ReactiveFormsModule]
})

export class CreateTaskComponent {
    createTaskForm = createCreateTaskForm();
    private readonly toastr = inject(ToastrService);
    private noteService = inject(NoteApiService);
    created = output<Note>();
    model = model<Note>({ id: -1, name: 'untitled', description: '', status: 'new', createdAt: new Date(), updatedAt: new Date() });

    onSubmit() {
        if (this.createTaskForm.invalid) {
            this.createTaskForm.markAllAsTouched();
            return;
        }

        const model: CreateTaskModel = { name: this.createTaskForm.controls.name.value!, description: this.createTaskForm.controls.description.value! }

        this.noteService.createNote(model).subscribe({
            next: (x) => {
                this.toastr.success('Заметка успешно создана!')
                
                this.model.update(m => ({
                    id: x.data as number,
                    name: this.createTaskForm.controls.name.value!,
                    description: this.createTaskForm.controls.description.value!,
                    status: 'new',
                    createdAt: new Date(),
                    updatedAt: new Date()
                }));
                this.created.emit(this.model())
            },
            error: (e: Error) => {
                this.toastr.error(e.message, 'Возникла ошибка во время авторизации')
            }
        })
    }
}