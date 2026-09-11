import { Component } from "@angular/core";
import { UnauthHeaderComponent } from "../../widgets/unauthorized-header/ui/unauth-header.component";

@Component({
    selector: 'app-landing-page',
    templateUrl: './landing.component.html',
    styleUrl: './landing.component.scss',
    imports: [UnauthHeaderComponent]
})

export class LandingComponent {

}