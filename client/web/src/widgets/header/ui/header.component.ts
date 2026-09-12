import { Component, inject } from "@angular/core";
import { UserBadgeComponent } from "../../../entities/user";
import { PrimaryButtonComponent } from "../../../shared/ui/button/button/button.component";
import { AuthService } from "../../../shared/auth/auth.service";
import { Router } from "@angular/router";

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrl: './header.component.scss',
    imports: [UserBadgeComponent, PrimaryButtonComponent]
})

export class HeaderComponent {
    private readonly authService = inject(AuthService);
    private readonly router = inject(Router);

    logout() {
        this.authService.logout();
        this.router.navigate(['/'])
    }
}