import { Component, inject } from "@angular/core";
import { InputComponent } from "../../../../shared/ui/input/input/input.component";
import { PrimaryButtonComponent } from "../../../../shared/ui/button/button/button.component";
import { ReactiveFormsModule } from "@angular/forms";
import { createRegisterForm } from "../../model/register.form";
import { ToastrService } from "ngx-toastr";
import { RegisterService } from "../../api/register.service";
import { RegisterModel } from "../../model/register.model";

@Component({
    selector: 'app-register-form',
    templateUrl: './register-form.component.html',
    styleUrl: './register-form.component.scss',
    imports: [InputComponent, PrimaryButtonComponent, ReactiveFormsModule]
})

export class RegisterFormComponent {
    registerForm = createRegisterForm();
    private readonly toastr = inject(ToastrService);
    private readonly service = inject(RegisterService);

    onSubmit() {
        if (this.registerForm.invalid) {
            this.registerForm.markAllAsTouched();
            return;
        }

        const model: RegisterModel = { username: this.registerForm.controls.login.value!, password: this.registerForm.controls.password.value! }

        this.service.registerUser(model).subscribe({
            next: () => {
                this.toastr.success('Регистрация успешно пройдена!', 'Успешно!')
            },
            error: (e: Error) => {
                this.toastr.error(e.message, 'Возникла ошибка во время регистрации')
            }
        })
    }
}