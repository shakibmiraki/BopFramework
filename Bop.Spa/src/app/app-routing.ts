import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BlankComponent } from './shared/layouts/blank/blank.component';
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';



export const AppRoutes: Routes = [

  {
    path: 'home',
    component: BlankComponent
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
