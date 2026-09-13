import { FormControl, FormGroup, Validators } from "@angular/forms";

export function createUpdateTaskNameForm(name: string) {
    return new FormGroup({
        name: new FormControl(name, [Validators.required, Validators.minLength(4), Validators.maxLength(24), Validators.pattern(/^[^*\\/'"^<>:|?]+$/)]),
    })
}


export function createUpdateTaskDescriptionForm(description: string) {
    return new FormGroup({
        description: new FormControl(description, [Validators.maxLength(1000),]),
    })
}
