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