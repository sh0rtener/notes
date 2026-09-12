import { afterNextRender, Component, inject } from "@angular/core";
import { UnauthHeaderComponent } from "../../widgets/unauthorized-header/ui/unauth-header.component";
import { PrimaryButtonComponent } from "../../shared/ui/button/button/button.component";
import { Router, RouterLink } from "@angular/router";
import { AuthService } from "../../shared/auth/auth.service";

@Component({
    selector: 'app-landing-page',
    templateUrl: './landing.component.html',
    styleUrl: './landing.component.scss',
    imports: [UnauthHeaderComponent, PrimaryButtonComponent, RouterLink]
})

export class LandingComponent {
    private readonly authService = inject(AuthService);
    private readonly router = inject(Router)

    constructor() {
        afterNextRender(() => {
            if (this.authService.isAuth()) {
                this.router.navigate(['/notes']);
            }
        });
    }
}