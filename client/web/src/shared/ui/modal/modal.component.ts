import { Component, input, model } from "@angular/core";

@Component({
    selector: 'app-modal',
    templateUrl: './modal.component.html',
    styleUrl: './modal.component.scss'
})

export class ModalComponent {
    title = input.required<string>();
    isActive = model(false);

    close() {
        this.isActive.set(false);
    }
}