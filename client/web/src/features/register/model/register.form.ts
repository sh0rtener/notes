import { FormControl, FormGroup, Validators } from "@angular/forms";

export function createRegisterForm() {
    return new FormGroup({
        login: new FormControl('', [Validators.required,]),
        password: new FormControl('', [Validators.required,]),
        passwordRepeat: new FormControl('', [Validators.required,])
    })
}