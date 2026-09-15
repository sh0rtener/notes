# ShNotes

**ShNotes** — full-stack приложение для работы с заметками.

Проект состоит из REST API на **ASP.NET Core** и клиентского приложения на **Angular**.

Основной упор сделан на разделение ответственности, поддерживаемую архитектуру и возможность дальнейшего расширения приложения.

## Возможности

* Регистрация пользователей
* Аутентификация
* Хранение credentials в виде хэша
* Привязка заметок к пользователям
* Создание заметок
* Просмотр списка заметок
* Поиск и фильтрация заметок
* Просмотр отдельной заметки
* Изменение названия и описания
* Изменение статуса
* Удаление заметок
* In-memory кеширование
* Инвалидация кеша при изменении данных
* REST API
* Swagger / OpenAPI
* SSR для клиентского приложения

## Стек технологий

### Backend

* **.NET 8**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **SQLite**
* **MediatR**
* **AutoMapper**
* **IMemoryCache**
* **Swagger / OpenAPI**
* **xUnit**

### Frontend

* **Angular 19**
* **TypeScript**
* **RxJS**
* **Angular Signals**
* **Reactive Forms**
* **SCSS**
* **Angular SSR**
* **FSD (Feature-Sliced Design)**

## Архитектура

Проект разделён на backend и frontend.

```text
.
├── client/
│   └── web/
│
├── src/
│   ├── Core/
│   │   ├── ShNotes.Core/
│   │   └── ShNotes.UseCases/
│   │
│   ├── Infra/
│   │   ├── ShNotes.Data/
│   │   └── ShNotes.Caching/
│   │
│   └── Presenters/
│       └── ShNotes.WebApi/
│
├── tests/
│   └── ShNotes.Tests/
│
├── infra/
└── docs/
```

---

## Backend

Backend построен с разделением доменной, прикладной, инфраструктурной и presentation-логики.

### Core

Содержит основные сущности и абстракции предметной области.

```text
ShNotes.Core
├── Entities
├── Value Objects
└── Domain exceptions
```

Слой не зависит от инфраструктуры или Web API.

### UseCases

Содержит прикладную логику и сценарии использования приложения.

```text
ShNotes.UseCases
├── Notes
│   ├── AddNote
│   ├── GetNotes
│   ├── GetNote
│   ├── ChangeNoteName
│   ├── ChangeNoteDescription
│   ├── ChangeNoteStatus
│   └── RemoveNote
│
└── Users
    ├── CreateUser
    ├── GetUser
    └── SignIn
```

Для организации use case'ов используется **MediatR**.

Контроллеры не содержат основную бизнес-логику и передают выполнение соответствующим use case'ам.

### Data

Слой отвечает за работу с базой данных и реализацию инфраструктурных зависимостей.

```text
ShNotes.Data
├── Daos
├── EntityFramework
├── SQL Scripts
└── Dependency Injection
```

Для доступа к данным используется **Entity Framework Core**.

Модели базы данных отделены от доменных сущностей с помощью DAO.

В качестве базы данных используется **SQLite**.

### Caching

Кеширование вынесено в отдельный слой.

```text
ShNotes.Caching
├── InMemory
├── CacheInvalidator
└── Dependency Injection
```

Для хранения кеша используется `IMemoryCache`.

Кеширование не смешивается с основной логикой use case'ов.

При изменении данных выполняется инвалидация связанных кешей.

### Web API

Presentation-слой приложения.

```text
ShNotes.WebApi
├── Controllers
├── Middlewares
├── Common
└── Program.cs
```

Контроллеры отвечают преимущественно за обработку HTTP-запросов и передачу управления use case'ам.

Для документации API используется Swagger / OpenAPI.

---

## Frontend

Клиентская часть находится в:

```text
client/web
```

Приложение построено на Angular и использует **Feature-Sliced Design** для организации frontend-кода.

Основные слои:

```text
src/app/
├── pages/
├── widgets/
├── features/
├── entities/
└── shared/
```

### Entities

Содержит основные сущности приложения и связанные с ними модели, API и UI.

Например:

```text
entities/
└── note/
    ├── api/
    ├── model/
    └── ui/
```

### Features

Содержит пользовательские действия и отдельные сценарии приложения.

Например:

```text
features/
├── register/
├── login/
└── add-note/
```

### Widgets

Содержит самостоятельные крупные блоки интерфейса.

Например:

```text
widgets/
└── notes-list/
```

`notes-list` отвечает за отображение списка заметок и взаимодействие с ним.

### Pages

Собирает widgets и features в полноценные страницы приложения.

---

## API

### Получение заметок

```http
GET /notes
```

Пример запроса с фильтрацией:

```http
GET /notes?name=example
```

### Получение заметки

```http
GET /notes/{id}
```

### Создание заметки

```http
POST /notes
```

### Изменение заметки

```http
PATCH /notes/{id}/name
PATCH /notes/{id}/description
```

### Изменение статуса

```http
PATCH /notes/{id}/status
```

### Удаление заметки

```http
DELETE /notes/{id}
```

Более подробное описание endpoints и моделей доступно через Swagger UI после запуска приложения.

---

## Запуск проекта

### Требования

Для запуска необходимы:
* Docker

Для доработки необходимы:

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* Node.js
* npm
* Git

### Клонирование репозитория

```bash
git clone https://github.com/sh0rtener/notes.git
cd notes
```

### Запуск приложения

```bash
sudo docker compose -f infra/docker-compose.yml --verbose up --build
```

### Backend

Восстановление зависимостей:

```bash
dotnet restore
```

Сборка:

```bash
dotnet build
```

Запуск:

```bash
dotnet run --project src/Presenters/ShNotes.WebApi
```

После запуска API будет доступен по адресу, указанному ASP.NET Core в консоли.

Swagger UI доступен в режиме разработки.

### Frontend

Перейти в клиентское приложение:

```bash
cd client/web
```

Установить зависимости:

```bash
npm install
```

Запустить development server:

```bash
npm start
```

После запуска приложение будет доступно по адресу:

```text
http://localhost:4200
```

### SSR

Для запуска собранного SSR-приложения:

```bash
npm run build
npm run serve:ssr:web
```

---

## Тестирование

### Backend

Для запуска backend-тестов:

```bash
dotnet test
```

Тесты находятся в:

```text
tests/ShNotes.Tests
```

### Frontend

Для запуска Angular unit-тестов:

```bash
npm test
```

---

## Цели проекта

Проект создаётся как практическая full-stack разработка с упором на:

* разделение ответственности;
* изоляцию доменной логики;
* тестируемость;
* абстрагирование работы с базой данных;
* отделение инфраструктуры от бизнес-логики;
* кеширование и инвалидацию данных;
* разделение frontend-кода по FSD;
* поддерживаемость кода;
* возможность дальнейшего расширения приложения.

## Планируемые улучшения

* [ ] Расширение покрытия тестами
* [ ] E2E-тестирование frontend
* [ ] Docker / Docker Compose
* [ ] Production-конфигурация
* [ ] Поддержка дополнительных СУБД
* [ ] Улучшение обработки ошибок на клиенте
* [ ] Расширение системы аккаунтов
* [ ] Дополнительные возможности работы с заметками
