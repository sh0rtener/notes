import { afterNextRender, Component, inject, signal } from "@angular/core";
import { UserModel } from "../../model/user.model";
import { UserService } from "../../api/user.service";

@Component({
    selector: 'app-user-badge',
    templateUrl: './user-badge.component.html',
    styleUrl: './user-badge.component.scss'
})

export class UserBadgeComponent {
    private readonly userService = inject(UserService);

    readonly user = signal<UserModel>({ id: -1, name: 'undefined', createdAt: new Date() });

    constructor() {
        afterNextRender(() => {
            this.loadNotes();
        });
    }

    private loadNotes() {
        this.userService.getUser().subscribe({
            next: response => {
                this.user.set(response.data ?? { id: -1, name: 'undefined', createdAt: new Date() });
            },
            error: error => {
            }
        });
    }
}