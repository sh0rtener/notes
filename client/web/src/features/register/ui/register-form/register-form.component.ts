import { Component, inject } from "@angular/core";
import { InputComponent } from "../../../../shared/ui/input/input/input.component";
import { PrimaryButtonComponent } from "../../../../shared/ui/button/button/button.component";
import { ReactiveFormsModule } from "@angular/forms";
import { createRegisterForm } from "../../model/register.form";
import { ToastrService } from "ngx-toastr";
import { RegisterService } from "../../api/register.service";
import { RegisterModel } from "../../model/register.model";
import { AuthService } from "../../../../shared/auth/auth.service";
import { Router } from "@angular/router";

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
    private readonly authService = inject(AuthService);
    private readonly router = inject(Router);

    onSubmit() {
        if (this.registerForm.invalid) {
            this.registerForm.markAllAsTouched();
            return;
        }

        const model: RegisterModel = { username: this.registerForm.controls.login.value!, password: this.registerForm.controls.password.value! }

        this.service.registerUser(model).subscribe({
            next: async () => {
                this.toastr.success('Регистрация успешно пройдена!', 'Успешно!')
                
                this.authService.login(model).subscribe()

                await new Promise<void>(resolve => {
                    setTimeout(resolve, 1000);
                });
                
                this.router.navigate(['/'])
            },
            error: (e: Error) => {
                this.toastr.error(e.message, 'Возникла ошибка во время регистрации')
            }
        })
    }
}