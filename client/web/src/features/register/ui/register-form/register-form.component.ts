import { Component } from "@angular/core";
import { InputComponent } from "../../../../shared/ui/input/input/input.component";
import { PrimaryButtonComponent } from "../../../../shared/ui/button/button/button.component";
import { ReactiveFormsModule } from "@angular/forms";
import { createRegisterForm } from "../../model/register.form";

@Component({
    selector: 'app-register-form',
    templateUrl: './register-form.component.html',
    styleUrl: './register-form.component.scss',
    imports: [InputComponent, PrimaryButtonComponent, ReactiveFormsModule]
})

export class RegisterFormComponent {
    registerForm = createRegisterForm();

    onSubmit() {
        if (this.registerForm.invalid) {
            this.registerForm.markAllAsTouched();
            return;
        }
    }
}