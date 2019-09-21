import { LocalizationComponent } from './localization/localization.component';
import { RouterModule } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from '../core/services/auth.guard';
import { RootComponent } from './root/root.component';
import { AuthGuardPermission } from '../core/models/auth-guard-permission';

export const routing: ModuleWithProviders = RouterModule.forChild([
  {
    path: 'dashboard',
    component: RootComponent,
    data: {
      permission: {
        permittedRoles: ['Registered']
      } as AuthGuardPermission
    },
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        component: HomeComponent,
        data: {
          permission: {
            permittedRoles: ['Registered']
          } as AuthGuardPermission
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'home',
        component: HomeComponent,
        data: {
          permission: {
            permittedRoles: ['Registered']
          } as AuthGuardPermission
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'localization',
        component: LocalizationComponent,
        data: {
          permission: {
            permittedRoles: ['Administrators']
          } as AuthGuardPermission
        },
        canActivate: [AuthGuard]
      }
    ]
  }
]);
