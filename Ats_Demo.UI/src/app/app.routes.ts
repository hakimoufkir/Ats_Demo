import { Routes } from '@angular/router';
import { EmployeeListComponent } from './components/employee-list/employee-list.component';
import { EmployeeFormComponent } from './components/employee-form/employee-form.component';

export const routes: Routes = [
    { path: '', redirectTo: '/employees', pathMatch: 'full' },
    { path: 'employees', component: EmployeeListComponent },
    { path: 'employees/new', component: EmployeeFormComponent },
    { path: 'employees/:id', component: EmployeeFormComponent },
    { path: '**', redirectTo: '/employees' }
];
