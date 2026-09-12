import { Component, inject } from "@angular/core";
import { InputComponent } from "../../../../shared/ui/input/input/input.component";
import { PrimaryButtonComponent } from "../../../../shared/ui/button/button/button.component";
import { createLoginForm } from "../../model/login.form";
import { ToastrService } from "ngx-toastr";
import { AuthService } from "../../../../shared/auth/auth.service";
import { AuthModel } from "../../../../shared/auth/auth.model";
import { ReactiveFormsModule } from "@angular/forms";

@Component({
    selector: 'app-login-form',
    templateUrl: './login-form.component.html',
    styleUrl: './login-form.component.scss',
    imports: [InputComponent, PrimaryButtonComponent, ReactiveFormsModule]
})

export class LoginFormComponent {
    loginForm = createLoginForm();
    private readonly toastr = inject(ToastrService);
    private readonly authService = inject(AuthService);

    onSubmit() {
        if (this.loginForm.invalid) {
            this.loginForm.markAllAsTouched();
            return;
        }

        const model: AuthModel = { username: this.loginForm.controls.login.value!, password: this.loginForm.controls.password.value! };

        this.authService.login(model).subscribe({
            next: () => {
                this.toastr.success('Вход успешно выполнен!', 'Успешно!')
            },
            error: (e: Error) => {
                this.toastr.error(e.message, 'Возникла ошибка во время авторизации')
            }
        })
    }
}