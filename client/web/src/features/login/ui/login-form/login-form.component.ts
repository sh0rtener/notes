import { Component } from "@angular/core";
import { InputComponent } from "../../../../shared/ui/input/input/input.component";
import { PrimaryButtonComponent } from "../../../../shared/ui/button/button/button.component";

@Component({
    selector: 'app-login-form',
    templateUrl: './login-form.component.html',
    styleUrl: './login-form.component.scss',
    imports: [InputComponent, PrimaryButtonComponent]
})

export class LoginFormComponent {

}