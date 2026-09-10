import { Pipe, PipeTransform } from '@angular/core';
import { NoteStatus } from '../../model/notestatus.model';

@Pipe({
  name: 'noteStatusPipe',
  standalone: true,
})
export class NoteStatusPipe implements PipeTransform {
  transform(status: string): string {
    switch (status) {
      case NoteStatus.new:
        return 'Новая';

      case NoteStatus.onWork:
        return 'В процессе';

      case NoteStatus.completed:
        return 'Завершена';

      default:
        return 'Неизвестно';
    }
  }
}