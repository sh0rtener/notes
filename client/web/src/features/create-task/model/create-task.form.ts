import { FormControl, FormGroup, Validators } from "@angular/forms";

export function createCreateTaskForm() {
    return new FormGroup({
        name: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(24), Validators.pattern(/^[^*\\/'"^<>:|?]+$/)]),
        description: new FormControl('', [Validators.maxLength(1000),]),
    })
}