import { AccessDeniedComponent } from './access-denied/access-denied.component';
import { ActivationFormComponent } from './activation-form/activation-form.component';
import { ModuleWithProviders } from '@angular/core';
import { RouterModule } from '@angular/router';

import { RegistrationFormComponent } from './registration-form/registration-form.component';
import { LoginFormComponent } from './login-form/login-form.component';

export const routing: ModuleWithProviders = RouterModule.forChild([
  {
    path: 'register',
    component: RegistrationFormComponent
  },
  {
    path: 'login',
    component: LoginFormComponent
  },
  {
    path: 'activate',
    component: ActivationFormComponent
  },
  {
    path: 'access-denied',
    component: AccessDeniedComponent
  }
]);
