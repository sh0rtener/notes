import { Component } from "@angular/core";
import { UnauthHeaderComponent } from "../../widgets/unauthorized-header/ui/unauth-header.component";
import { PrimaryButtonComponent } from "../../shared/ui/button/button/button.component";

@Component({
    selector: 'app-landing-page',
    templateUrl: './landing.component.html',
    styleUrl: './landing.component.scss',
    imports: [UnauthHeaderComponent, PrimaryButtonComponent]
})

export class LandingComponent {
    rotationLogoX = 0;
    rotationLogoY = 0;

    onMouseMove(event: MouseEvent) {
        const x = event.clientX / window.innerWidth;
        const y = event.clientY / window.innerHeight;

        this.rotationLogoY = (x - 0.5) * 20;
        this.rotationLogoX = -(y - 0.5) * 20;
    }
}