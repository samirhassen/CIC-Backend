import { Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout.component';
import { HomeComponent } from './pages/home/home.component';
import { AuthenticationComponent } from './authentication/authentication.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,  
  },
  {
    path: 'home',
    component: HomeComponent,  
  },
];
