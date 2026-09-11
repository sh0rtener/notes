import { Component } from "@angular/core";
import { PrimaryButtonComponent } from "../../../shared/ui/button/button/button.component";

@Component({
    selector: 'app-unauth-header',
    templateUrl: './unauth-header.component.html',
    styleUrl: './unauth-header.component.scss',
    imports: [PrimaryButtonComponent]
})

export class UnauthHeaderComponent {

}