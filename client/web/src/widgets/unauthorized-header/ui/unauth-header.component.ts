import { Component } from "@angular/core";
import { PrimaryButtonComponent } from "../../../shared/ui/button/button/button.component";
import { RouterLink } from "@angular/router";

@Component({
    selector: 'app-unauth-header',
    templateUrl: './unauth-header.component.html',
    styleUrl: './unauth-header.component.scss',
    imports: [PrimaryButtonComponent, RouterLink]
})

export class UnauthHeaderComponent {

}