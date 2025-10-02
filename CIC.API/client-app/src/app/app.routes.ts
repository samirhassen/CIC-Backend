//src/app/app.routes.ts
import { Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout.component';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './layout/login/login.component';
import { AuthenticationComponent } from './authentication/authentication.component';
import { MsalGuard } from '@azure/msal-angular';

export const routes: Routes = [
     {
        path: '',
        component: AuthenticationComponent, // root path goes to login
      },
      {
        path: '',
        component: LayoutComponent, // layout contains header/footer
        children: [
          { path: 'home', component: HomeComponent },
          
        ],
      },
      { path: 'authentication', component: AuthenticationComponent }
];


// src/app/app.routes.ts
// import { Routes } from '@angular/router';
// import { LayoutComponent } from './layout/layout.component';
// import { HomeComponent } from './pages/home/home.component';
// import { LoginComponent } from './layout/login/login.component';
// import { AuthenticationComponent } from './authentication/authentication.component';
// import { MsalGuard } from '@azure/msal-angular';

// export const routes: Routes = [
//   { path: '', component: LoginComponent },
//   {
//     path: '',
//     component: LayoutComponent,
//     canActivate: [MsalGuard],          // protect anything under Layout
//     children: [
//       { path: 'home', component: HomeComponent },
//     ],
//   },
//   { path: 'authentication', component: AuthenticationComponent },
//   { path: '**', redirectTo: '' },
// ];

