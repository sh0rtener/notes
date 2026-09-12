import { Component, forwardRef, inject, input } from "@angular/core";
import { AbstractControl, ControlValueAccessor, NG_VALUE_ACCESSOR, NgControl } from '@angular/forms';

@Component({
    selector: 'app-input',
    templateUrl: './input.component.html',
    styleUrl: './input.component.scss',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => InputComponent),
            multi: true
        }
    ]
})

export class InputComponent implements ControlValueAccessor {
    label = input<string>();
    type = input<string>('text');
    placeholder = input<string>('');
    id = input.required<string>();
    disabled = input<boolean>();
    value = '';


    control = input<AbstractControl | null>(null);


    onInput(event: Event) {
        const input = event.target as HTMLInputElement;

        this.value = input.value;
        this.onChange(this.value);
    }


    private onChange = (value: string) => { };
    onTouched = () => { };
    onBlur() {
        this.onTouched();
    }

    writeValue(value: string): void {
        this.value = value ?? '';
    }

    registerOnChange(fn: (value: string) => void): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: () => void): void {
        this.onTouched = fn;
    }

    setDisabledState(isDisabled: boolean): void {
    }

    errorMessage(): string | null {
        const errors = this.control()?.errors;
        

        if (!errors) {
            return null;
        }

        if (errors['required']) {
            return 'Поле обязательно';
        }

        if (errors['minlength']) {
            return `Минимум ${errors['minlength'].requiredLength} символов`;
        }

        if (errors['maxlength']) {
            return `Максимум ${errors['maxlength'].requiredLength} символов`;
        }

        return 'Некорректное значение';
    }
}