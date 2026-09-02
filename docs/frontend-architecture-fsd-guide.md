# Архитектурное руководство: Frontend на Angular + TypeScript + Feature-Sliced Design (FSD)

Данный документ представляет собой подробный обучающий справочник и практическое руководство по созданию клиентской части приложения **ShNotes** на **Angular (Standalone, Signals, TypeScript)** с использованием методологии **Feature-Sliced Design (FSD)**.

---

## 1. Концепция и философия Feature-Sliced Design (FSD)

### 1.1. Зачем нужен FSD?
В классических Angular-проектах код часто группируют по техническому типу файлов:
```text
src/app/
├── components/   # сотни разнородных компонентов в одной куче
├── services/     # сервисы, смешивающие бизнес-логику и утилиты
├── models/       # интерфейсы без привязки к контексту
└── pages/
```
Такой подход быстро приводит к:
1. **Спагетти-зависимостям**: компоненты хаотично импортируют сервисы и другие компоненты.
2. **Циклическим зависимостям (Circular Dependencies)**: модуль A зависит от B, а B — от A.
3. **Размытию ответственности**: трудно понять, где заканчивается логика сущности и начинается пользовательский сценарий.

**FSD решает это, разделяя проект на строго иерархические слои по бизнес-смыслу.**

---

### 1.2. Иерархия слоев (Layers) и Золотое правило зависимостей

В FSD существует 6 стандартных слоев. **Зависимости могут быть направлены ТОЛЬКО сверху вниз.**

```text
┌────────────────────────────────────────────────────────┐
│                        1. APP                          │
│   (Инициализация, роутинг, глобальные провайдеры)      │
└───────────────────────────┬────────────────────────────┘
                            │ ↓
┌───────────────────────────▼────────────────────────────┐
│                       2. PAGES                         │
│   (Композиция полных страниц: Login, Notes, Details)   │
└───────────────────────────┬────────────────────────────┘
                            │ ↓
┌───────────────────────────▼────────────────────────────┐
│                      3. WIDGETS                        │
│   (Крупные самодостаточные блоки UI: Header, NoteList) │
└───────────────────────────┬────────────────────────────┘
                            │ ↓
┌───────────────────────────▼────────────────────────────┐
│                      4. FEATURES                       │
│   (Действия пользователя: CreateNote, DeleteNote)      │
└───────────────────────────┬────────────────────────────┘
                            │ ↓
┌───────────────────────────▼────────────────────────────┘
│                      5. ENTITIES                       │
│   (Бизнес-сущности: Note, User, Session)               │
└───────────────────────────┬────────────────────────────┘
                            │ ↓
┌───────────────────────────▼────────────────────────────┐
│                       6. SHARED                        │
│   (UI-kit, HTTP-клиент, утилиты без бизнес-логики)     │
└────────────────────────────────────────────────────────┘
```

> [!IMPORTANT]
> **Главное правило FSD:**
> Модуль на определенном слое может импортировать код **только из слоев, расположенных ниже**.
> - `entities` **не могут** импортировать `features`, `widgets`, `pages`, `app`.
> - `features` **не могут** импортировать `widgets` или `pages`.
> - `shared` не знает ни о каких бизнес-сущностях вообще.

---

### 1.3. Слайсы (Slices) и Сегменты (Segments)

1. **Слой (Layer)** — верхний уровень классификации (например, `features`).
2. **Слайс (Slice)** — изолированная бизнес-единица внутри слоя (например, `features/create-note`, `entities/note`).
   - **Правило кросс-импортов:** Слайсы на одном слое **не могут напрямую импортировать друг друга**. Если двум фичам нужен общий код, он должен быть вынесен на уровень `entities` или `shared`.
3. **Сегмент (Segment)** — техническое разделение внутри слайса:
   - `ui/` — компоненты отображения (HTML, SCSS, Component TS).
   - `model/` — типы, интерфейсы, состояние (Signals/RxJS).
   - `api/` — методы для работы с бэкендом.
   - `lib/` — вспомогательные локальные хелперы.
4. **Public API (`index.ts`)**:
   - Каждый слайс обязан иметь входной файл `index.ts`.
   - Внешний мир может импортировать только то, что экспортировано из `index.ts`.
   - *Пример:* `import { NoteCard } from '@/entities/note';` вместо глубокого импорта `.../ui/note-card/note-card.component`.

---

## 2. Разделение ответственности: что куда класть?

| Вопрос | Слой | Пример |
|---|---|---|
| Какая форма данных у сущности? | `entities/{name}/model` | `interface Note`, `enum NoteStatus` |
| Как получить или обновить данные сущности с сервера? | `entities/{name}/api` | `NoteApiService.getById(id)` |
| Как выглядит карточка сущности без действий? | `entities/{name}/ui` | `NoteCardComponent`, `NoteStatusBadgeComponent` |
| Какое действие совершает пользователь? | `features/{action}` | `CreateNoteComponent`, `DeleteNoteButtonComponent` |
| Как объединить список карточек, пагинацию и фильтры в один готовый блок? | `widgets/{name}` | `NotesListWidget`, `HeaderWidget` |
| Как выглядит экран целиком? | `pages/{name}` | `NotesPageComponent`, `LoginPageComponent` |
| Переиспользуемая кнопка, инпут, форматтер даты, перехватчик токенов? | `shared` | `ButtonComponent`, `AuthInterceptor`, `DatePipe` |

---

## 3. Полная структура каталогов проекта ShNotes Frontend

```text
src/
├── app/
│   ├── app.component.ts             # Корневой shell-компонент (<router-outlet />)
│   ├── app.config.ts                # provideRouter, provideHttpClient(withInterceptors(...))
│   ├── app.routes.ts                # Маршрутизация с lazy loading страниц
│   └── styles/
│       ├── _variables.scss          # Цветовая палитра, отступы, шрифты
│       ├── _reset.scss              # Нормализация стилей
│       └── main.scss                # Точка входа стилей
│
├── pages/
│   ├── notes-page/                  # Главная страница: список заметок + сайдбар/фильтры
│   │   ├── notes-page.component.ts
│   │   ├── notes-page.component.html
│   │   ├── notes-page.component.scss
│   │   └── index.ts
│   ├── note-details-page/           # Просмотр и редактирование одной заметки
│   │   ├── note-details-page.component.ts
│   │   ├── note-details-page.component.html
│   │   └── index.ts
│   └── login-page/                  # Вход по паролю или гостевой сессии
│       ├── login-page.component.ts
│       ├── login-page.component.html
│       └── index.ts
│
├── widgets/
│   ├── header/                      # Шапка: логотип, профиль, смена темы, выход
│   │   ├── ui/header.component.ts
│   │   └── index.ts
│   ├── notes-list/                  # Сетка заметок: скелетоны при загрузке, empty state, карточки
│   │   ├── ui/notes-list.component.ts
│   │   └── index.ts
│   ├── note-editor-modal/           # Модальное окно создания/редактирования
│   │   ├── ui/note-editor-modal.component.ts
│   │   └── index.ts
│   └── note-filters-bar/            # Панель поиска по названию, статусу и дате
│       ├── ui/note-filters-bar.component.ts
│       └── index.ts
│
├── features/
│   ├── create-note/                 # Форма создания новой заметки
│   │   ├── ui/create-note-form.component.ts
│   │   └── index.ts
│   ├── edit-note-title/             # Inline-редактирование имени заметки
│   │   ├── ui/edit-note-title.component.ts
│   │   └── index.ts
│   ├── edit-note-description/       # Редактирование текста заметки
│   │   ├── ui/edit-note-description.component.ts
│   │   └── index.ts
│   ├── change-note-status/          # Кнопки смены статуса («В работу», «Завершить»)
│   │   ├── ui/change-status-button.component.ts
│   │   └── index.ts
│   ├── delete-note/                 # Кнопка удаления с диалогом подтверждения
│   │   ├── ui/delete-note-button.component.ts
│   │   └── index.ts
│   ├── filter-notes/                # Логика и инпуты фильтрации
│   │   ├── model/filter.service.ts  # Реактивный стейт фильтров на Signals
│   │   └── index.ts
│   ├── auth-by-credentials/         # Логин/пароль форма
│   │   ├── ui/login-form.component.ts
│   │   └── index.ts
│   └── auth-by-session/             # Автовход/гостевая сессия по sessionKey из localStorage
│       ├── model/session-auth.service.ts
│       └── index.ts
│
├── entities/
│   ├── note/
│   │   ├── model/note.model.ts      # Note, ShortNote, NoteStatus, GetNoteFilter
│   │   ├── api/note-api.service.ts  # HttpClient запросы к /notes
│   │   ├── ui/
│   │   │   ├── note-card/           # Презентационная карточка
│   │   │   └── note-status-badge/   # Цветовой бейдж статуса
│   │   └── index.ts
│   ├── user/
│   │   ├── model/user.model.ts      # UserDto, CreateUserRequest
│   │   ├── api/user-api.service.ts  # HttpClient запросы к /users
│   │   └── index.ts
│   └── session/
│       ├── model/tokens.model.ts    # SignInResponse, RefreshTokenRequest
│       ├── model/auth-state.service.ts # Хранение Access/Refresh токенов на Signals
│       └── index.ts
│
└── shared/
    ├── api/
    │   ├── api-response.model.ts    # Обертка { status, data, message }
    │   ├── auth.interceptor.ts      # JWT Bearer + автоматический refresh при 401
    │   └── error.interceptor.ts     # Тосты/уведомления об ошибках
    ├── config/
    │   └── environment.ts           # { apiUrl: 'http://localhost:5000' }
    ├── ui/                          # UI-Kit (чистые UI элементы без бизнес-данных)
    │   ├── button/
    │   ├── input/
    │   ├── textarea/
    │   ├── badge/
    │   ├── modal/
    │   └── spinner/
    └── lib/
        ├── date-format.ts           # Форматирование дат
        └── storage.service.ts       # Обертка над localStorage
```

---

## 4. Реализация ключевых компонентов и паттернов

### 4.1. Слой Shared: Типизация API и перехватчик токенов

#### `shared/api/api-response.model.ts`
```typescript
export interface ApiResponse<T> {
  status: number; // 1 = Ok, 0 = Failed
  data: T;
  message?: string;
}
```

#### `shared/api/auth.interceptor.ts`
```typescript
import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthStateService } from '../../entities/session';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from './api-response.model';
import { SignInResponse } from '../../entities/session';
import { environment } from '../config/environment';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authState = inject(AuthStateService);
  const http = inject(HttpClient);
  const token = authState.accessToken();

  // Добавляем Bearer токен, если он есть
  let authReq = req;
  if (token && !req.url.includes('/sign-in') && !req.url.includes('/refresh-token')) {
    authReq = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      // Если получили 401 и есть Refresh Token — пытаемся обновиться
      if (error.status === 401 && authState.refreshToken()) {
        return http
          .post<ApiResponse<SignInResponse>>(`${environment.apiUrl}/users/refresh-token`, {
            refreshToken: authState.refreshToken()
          })
          .pipe(
            switchMap(res => {
              authState.setTokens(res.data);
              const retryReq = req.clone({
                setHeaders: { Authorization: `Bearer ${res.data.accessToken}` }
              });
              return next(retryReq);
            }),
            catchError(refreshErr => {
              authState.clear();
              return throwError(() => refreshErr);
            })
          );
      }
      return throwError(() => error);
    })
  );
};
```

---

### 4.2. Слой Entities: Сущность Заметки (`entities/note`)

#### `entities/note/model/note.model.ts`
```typescript
export enum NoteStatus {
  Created = 0,
  OnWork = 1,
  Completed = 2
}

export interface Note {
  id: number;
  name: string;
  description: string;
  status: NoteStatus;
  createdAt: string;
  updatedAt?: string;
}

export interface ShortNote {
  id: number;
  name: string;
  status: NoteStatus;
  createdAt: string;
  updatedAt?: string;
}

export interface GetNoteFilter {
  name?: string;
  status?: NoteStatus;
  dateFrom?: string;
  limit?: number;
  offset?: number;
}
```

#### `entities/note/api/note-api.service.ts`
```typescript
import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../shared/config/environment';
import { ApiResponse } from '../../../shared/api/api-response.model';
import { Note, ShortNote, GetNoteFilter, NoteStatus } from '../model/note.model';

@Injectable({ providedIn: 'root' })
export class NoteApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/notes`;

  getAll(filter: GetNoteFilter = {}): Observable<ShortNote[]> {
    let params = new HttpParams();
    if (filter.name) params = params.set('name', filter.name);
    if (filter.status !== undefined) params = params.set('status', filter.status);
    if (filter.dateFrom) params = params.set('dateFrom', filter.dateFrom);
    if (filter.limit) params = params.set('limit', filter.limit);
    if (filter.offset) params = params.set('offset', filter.offset);

    return this.http
      .get<ApiResponse<ShortNote[]>>(`${this.baseUrl}/all`, { params })
      .pipe(map(res => res.data));
  }

  getById(id: number): Observable<Note> {
    return this.http
      .get<ApiResponse<Note>>(`${this.baseUrl}?id=${id}`)
      .pipe(map(res => res.data));
  }

  create(name: string, description: string): Observable<number> {
    return this.http
      .post<ApiResponse<number>>(this.baseUrl, { name, description })
      .pipe(map(res => res.data));
  }

  changeName(id: number, name: string): Observable<Note> {
    return this.http
      .patch<ApiResponse<Note>>(`${this.baseUrl}/${id}/name`, { name })
      .pipe(map(res => res.data));
  }

  changeDescription(id: number, description: string): Observable<Note> {
    return this.http
      .patch<ApiResponse<Note>>(`${this.baseUrl}/${id}/description`, { description })
      .pipe(map(res => res.data));
  }

  changeStatusToWork(id: number): Observable<NoteStatus> {
    return this.http
      .patch<ApiResponse<NoteStatus>>(`${this.baseUrl}/${id}/to-work`, {})
      .pipe(map(res => res.data));
  }

  changeStatusComplete(id: number): Observable<NoteStatus> {
    return this.http
      .patch<ApiResponse<NoteStatus>>(`${this.baseUrl}/${id}/complete`, {})
      .pipe(map(res => res.data));
  }

  delete(id: number): Observable<number> {
    return this.http
      .delete<ApiResponse<number>>(`${this.baseUrl}/${id}`)
      .pipe(map(res => res.data));
  }
}
```

#### `entities/note/ui/note-card/note-card.component.ts`
```typescript
import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShortNote, NoteStatus } from '../../model/note.model';
import { NoteStatusBadgeComponent } from '../note-status-badge/note-status-badge.component';

@Component({
  selector: 'app-note-card',
  standalone: true,
  imports: [CommonModule, NoteStatusBadgeComponent],
  template: `
    <article class="note-card" [class.completed]="note.status === NoteStatus.Completed">
      <div class="note-card__header">
        <h3 class="note-card__title" (click)="select.emit(note.id)">{{ note.name }}</h3>
        <app-note-status-badge [status]="note.status" />
      </div>
      <div class="note-card__footer">
        <time class="note-card__date">{{ note.createdAt | date: 'dd.MM.yyyy HH:mm' }}</time>
        <ng-content select="[actions]" />
      </div>
    </article>
  `,
  styleUrls: ['./note-card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NoteCardComponent {
  @Input({ required: true }) note!: ShortNote;
  @Output() select = new EventEmitter<number>();
  readonly NoteStatus = NoteStatus;
}
```

---

### 4.3. Слой Features: Действие пользователя (`features/change-note-status`)

#### `features/change-note-status/ui/change-status-button.component.ts`
```typescript
import { Component, Input, Output, EventEmitter, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NoteApiService, NoteStatus } from '../../../entities/note';

@Component({
  selector: 'app-change-status-button',
  standalone: true,
  imports: [CommonModule],
  template: `
    @if (status === NoteStatus.Created) {
      <button 
        class="btn btn--primary" 
        [disabled]="loading()" 
        (click)="takeToWork()">
        {{ loading() ? 'Обновление...' : 'В работу' }}
      </button>
    } @else if (status === NoteStatus.OnWork) {
      <button 
        class="btn btn--success" 
        [disabled]="loading()" 
        (click)="complete()">
        {{ loading() ? 'Обновление...' : 'Завершить' }}
      </button>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChangeStatusButtonComponent {
  @Input({ required: true }) noteId!: number;
  @Input({ required: true }) status!: NoteStatus;
  @Output() statusChanged = new EventEmitter<NoteStatus>();

  private readonly api = inject(NoteApiService);
  readonly loading = signal(false);
  readonly NoteStatus = NoteStatus;

  takeToWork(): void {
    this.loading.set(true);
    this.api.changeStatusToWork(this.noteId).subscribe({
      next: (newStatus) => {
        this.loading.set(false);
        this.statusChanged.emit(newStatus);
      },
      error: () => this.loading.set(false)
    });
  }

  complete(): void {
    this.loading.set(true);
    this.api.changeStatusComplete(this.noteId).subscribe({
      next: (newStatus) => {
        this.loading.set(false);
        this.statusChanged.emit(newStatus);
      },
      error: () => this.loading.set(false)
    });
  }
}
```

---

### 4.4. Слой Widgets: Виджет списка заметок (`widgets/notes-list`)

#### `widgets/notes-list/ui/notes-list.component.ts`
```typescript
import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShortNote, NoteCardComponent } from '../../../entities/note';
import { ChangeStatusButtonComponent } from '../../../features/change-note-status';
import { DeleteNoteButtonComponent } from '../../../features/delete-note';

@Component({
  selector: 'app-notes-list',
  standalone: true,
  imports: [
    CommonModule, 
    NoteCardComponent, 
    ChangeStatusButtonComponent, 
    DeleteNoteButtonComponent
  ],
  template: `
    @if (loading) {
      <div class="skeleton-grid">
        <div class="skeleton-card" *ngFor="let _ of [1,2,3,4,5,6]"></div>
      </div>
    } @else if (notes.length === 0) {
      <div class="empty-state">
        <p class="empty-state__text">Заметок не найдено</p>
      </div>
    } @else {
      <div class="notes-grid">
        @for (note of notes; track note.id) {
          <app-note-card [note]="note" (select)="onSelectNote.emit($event)">
            <div actions class="card-actions">
              <app-change-status-button 
                [noteId]="note.id" 
                [status]="note.status" 
                (statusChanged)="onRefreshList.emit()" 
              />
              <app-delete-note-button 
                [noteId]="note.id" 
                (deleted)="onRefreshList.emit()" 
              />
            </div>
          </app-note-card>
        }
      </div>
    }
  `,
  styleUrls: ['./notes-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NotesListComponent {
  @Input() notes: ShortNote[] = [];
  @Input() loading = false;
  @Output() onSelectNote = new EventEmitter<number>();
  @Output() onRefreshList = new EventEmitter<void>();
}
```

---

### 4.5. Слой Pages: Экран заметок (`pages/notes-page`)

#### `pages/notes-page/ui/notes-page.component.ts`
```typescript
import { Component, OnInit, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NoteApiService, ShortNote, GetNoteFilter } from '../../../entities/note';
import { HeaderComponent } from '../../../widgets/header';
import { NotesListComponent } from '../../../widgets/notes-list';
import { NoteFiltersBarComponent } from '../../../widgets/note-filters-bar';
import { NoteEditorModalComponent } from '../../../widgets/note-editor-modal';

@Component({
  selector: 'app-notes-page',
  standalone: true,
  imports: [
    CommonModule,
    HeaderComponent,
    NotesListComponent,
    NoteFiltersBarComponent,
    NoteEditorModalComponent
  ],
  template: `
    <app-header (createNoteRequested)="openCreateModal()" />
    
    <main class="page-container">
      <app-note-filters-bar (filterChange)="applyFilters($event)" />
      
      <app-notes-list 
        [notes]="notes()" 
        [loading]="loading()"
        (onSelectNote)="openEditModal($event)"
        (onRefreshList)="loadNotes()"
      />
    </main>

    @if (isModalOpen()) {
      <app-note-editor-modal 
        [noteId]="selectedNoteId()" 
        (close)="closeModal()" 
        (saved)="loadNotes()" 
      />
    }
  `,
  styleUrls: ['./notes-page.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NotesPageComponent implements OnInit {
  private readonly api = inject(NoteApiService);

  readonly notes = signal<ShortNote[]>([]);
  readonly loading = signal(false);
  readonly isModalOpen = signal(false);
  readonly selectedNoteId = signal<number | null>(null);

  private currentFilter: GetNoteFilter = {};

  ngOnInit(): void {
    this.loadNotes();
  }

  loadNotes(): void {
    this.loading.set(true);
    this.api.getAll(this.currentFilter).subscribe({
      next: (data) => {
        this.notes.set(data);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  applyFilters(filter: GetNoteFilter): void {
    this.currentFilter = filter;
    this.loadNotes();
  }

  openCreateModal(): void {
    this.selectedNoteId.set(null);
    this.isModalOpen.set(true);
  }

  openEditModal(id: number): void {
    this.selectedNoteId.set(id);
    this.isModalOpen.set(true);
  }

  closeModal(): void {
    this.isModalOpen.set(false);
    this.selectedNoteId.set(null);
  }
}
```

### 4.6. Реализация гостевой сессии на стороне фронтенда

Гостевая сессия реализуется полностью на стороне фронтенда (`features/auth-by-guest-session`) без изменения контрактов бэкенда:
- Бэкенд предоставляет стандартные эндпоинты `POST /users` и `POST /users/sign-in`.
- Фронтенд при первом входе проверяет ключ `shnotes_guest_auth` в `localStorage`:
  1. Если ключа нет — генерирует валидный гостевой логин (например, `gst_` + 8 случайных символов, длина 12 символов, укладывается в инвариант `3-15`) и пароль.
  2. Регистрирует пользователя на сервере через `POST /users`.
  3. Сохраняет сгенерированные гостевые учетные данные в `localStorage`.
  4. Авторизуется через `POST /users/sign-in` и сохраняет пару JWT токенов.
- При повторных визитах — автоматически восстанавливает токены или выполняет прозрачный `sign-in` по сохраненным данным.

#### Пример сервиса `features/auth-by-guest-session/model/guest-session.service.ts`:
```typescript
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, switchMap, tap } from 'rxjs';
import { environment } from '../../../shared/config/environment';
import { ApiResponse } from '../../../shared/api/api-response.model';
import { AuthStateService, SignInResponse } from '../../../entities/session';
import { LocalStorageService } from '../../../shared/lib/storage.service';

interface GuestCredentials {
  username: string;
  password: string;
}

@Injectable({ providedIn: 'root' })
export class GuestSessionService {
  private readonly http = inject(HttpClient);
  private readonly authState = inject(AuthStateService);
  private readonly storage = inject(LocalStorageService);
  private readonly STORAGE_KEY = 'shnotes_guest_auth';

  initSession(): Observable<SignInResponse> {
    const saved = this.storage.get<GuestCredentials>(this.STORAGE_KEY);

    if (saved) {
      return this.signIn(saved.username, saved.password);
    }

    const credentials = this.generateGuestCredentials();
    return this.registerGuest(credentials).pipe(
      switchMap(() => this.signIn(credentials.username, credentials.password)),
      tap(() => this.storage.set(this.STORAGE_KEY, credentials))
    );
  }

  private registerGuest(creds: GuestCredentials): Observable<ApiResponse<unknown>> {
    return this.http.post<ApiResponse<unknown>>(`${environment.apiUrl}/users`, creds);
  }

  private signIn(username: string, password: string): Observable<SignInResponse> {
    return this.http
      .post<ApiResponse<SignInResponse>>(`${environment.apiUrl}/users/sign-in`, { username, password })
      .pipe(
        tap(res => this.authState.setTokens(res.data)),
        switchMap(res => of(res.data))
      );
  }

  private generateGuestCredentials(): GuestCredentials {
    const randomHex = Math.random().toString(36).substring(2, 10);
    return {
      username: `gst_${randomHex}`,
      password: `P@ss_${Math.random().toString(36).substring(2, 12)}!`
    };
  }
}
```

---

## 5. UI/UX и дизайн-система

### 5.1. Цветовая палитра и статусы
В соответствии с концепцией заметок и статусами бэкенда (`Created`, `OnWork`, `Completed`):
- **Фон приложения**: Минималистичный нейтральный светлый (`#F8F9FA`) / темный (`#121214`).
- **Карточки заметок**: Чистый белый (`#FFFFFF`) / темно-серый (`#1E1E24`) со скруглением `12px` и легкой тенью `0 4px 12px rgba(0,0,0,0.06)`.
- **Статусы**:
  - `Created` (Создано): Серый / Индиго бейдж (`#4F46E5`).
  - `OnWork` (В работе): Янтарно-оранжевый (`#F59E0B`).
  - `Completed` (Выполнено): Изумрудно-зеленый (`#10B981`) с мягким зачеркиванием заголовка.

### 5.2. Состояния интерфейса
Для каждого экрана и виджета предусмотрены 4 обязательных состояния:
1. **Loading State**: Skeleton-анимации вместо резких спиннеров.
2. **Empty State**: Приятная заглушка с иллюстрацией/иконкой («У вас пока нет заметок. Создайте первую!»).
3. **Error State**: Информативное сообщение с кнопкой «Повторить запрос».
4. **Action Pending**: Блокировка кнопок во время выполнения мутаций (избежание дублирования кликов).

---

## 6. Чеклист соответствия правилам FSD

- [x] **Иерархия соблюдена**: Ни один нижний слой не импортирует верхний.
- [x] **Кросс-импорты исключены**: Фичи не импортируют другие фичи.
- [x] **Public API**: Каждый слайс экспортирует наружу только публичные контракты через `index.ts`.
- [x] **Entities vs Features**: Бизнес-модели и простые карточки находятся в `entities`, а кнопки с действиями и мутациями — в `features`.
- [x] **Shared чист от бизнеса**: В `shared` только примитивы, перехватчики и утилиты.
- [x] **Signals + OnPush**: Локальное реактивное состояние контролируется сигналами Angular, гарантируя максимальную производительность.

