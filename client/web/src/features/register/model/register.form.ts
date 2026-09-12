import { FormControl, FormGroup, Validators } from "@angular/forms";

export function createRegisterForm() {
    return new FormGroup({
        login: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(15), Validators.pattern(/^[^*\\/'"^<>:|?]+$/)]),
        password: new FormControl('', [Validators.required,]),
        passwordRepeat: new FormControl('', [Validators.required,])
    })
}