import { Component, input } from "@angular/core"; 
import { ControlValueAccessor } from '@angular/forms';

@Component({
    selector: 'app-input',
    templateUrl: './input.component.html',
    styleUrl: './input.component.scss'
})

export class InputComponent implements ControlValueAccessor {
    label = input<string>();
    type = input<string>('text');
    placeholder = input<string>('');
    id = input.required<string>();
    disabled = input<boolean>();
    value = '';

    onInput(event: Event) {
        const input = event.target as HTMLInputElement;

        this.value = input.value;
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
}