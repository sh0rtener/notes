import { Routes } from '@angular/router';
import { LandingComponent } from '../pages/landing/landing.component';
import { RegisterComponent } from '../pages/register/register.component';
import { LoginComponent } from '../pages/login/login.component';
import { NotesComponent } from '../pages/notes/notes.component';

export const routes: Routes = [
    {
        path: '',
        component: LandingComponent
    },
    {
        path: 'notes',
        component: NotesComponent
    },
    {
        path: 'register',
        component: RegisterComponent
    },
    {
        path: 'login',
        component: LoginComponent
    }
];
