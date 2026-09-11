import { Component } from "@angular/core";
import { RegisterFormComponent } from "../../features/register/ui/register-form/register-form.component";
import { EmptyHeaderComponent } from "../../widgets/empty-header/ui/empty-header.component";

@Component({
    selector: 'app-register-page',
    templateUrl: './register.component.html',
    styleUrl: './register.component.scss',
    imports: [RegisterFormComponent, EmptyHeaderComponent]
})

export class RegisterComponent {

}