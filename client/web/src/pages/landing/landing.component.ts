import { Component } from "@angular/core";
import { UnauthHeaderComponent } from "../../widgets/unauthorized-header/ui/unauth-header.component";
import { PrimaryButtonComponent } from "../../shared/ui/button/button/button.component";
import { RouterLink } from "@angular/router";

@Component({
    selector: 'app-landing-page',
    templateUrl: './landing.component.html',
    styleUrl: './landing.component.scss',
    imports: [UnauthHeaderComponent, PrimaryButtonComponent, RouterLink]
})

export class LandingComponent {
    
}