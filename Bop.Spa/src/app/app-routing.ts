import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';


export const AppRoutes: Routes = [

  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },

  // {
  //   path: '',
  //   component: FullComponent,
  //   children: [
  //     {
  //       path: '',
  //       redirectTo: '/dashboard',
  //       pathMatch: 'full'
  //     },
  //     {
  //       path: 'dashboard',
  //       loadChildren: () =>
  //         import('./dashboard/dashboard.module').then(m => m.DashboardModule)
  //     }
  //   ]
  // },
  // {
  //   path: '',
  //   component: BlankComponent,
  //   children: [
  //     {
  //       path: '',
  //       loadChildren: () =>
  //         import('./authentication/authentication.module').then(
  //           m => m.AuthenticationModule
  //         )
  //     }
  //   ]
  // },
  {
    path: '**',
    component: PageNotFoundComponent
  }
];
