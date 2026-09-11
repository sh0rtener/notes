import { Component } from "@angular/core";
import { EmptyHeaderComponent } from "../../widgets/empty-header/ui/empty-header.component";
import { LoginFormComponent } from "../../features/login/ui/login-form/login-form.component";

@Component({
    selector: 'app-login-page',
    templateUrl: './login.component.html',
    styleUrl: './login.component.scss',
    imports: [EmptyHeaderComponent, LoginFormComponent]
})

export class LoginComponent {

}