import { AccessDeniedComponent } from "./access-denied/access-denied.component";
import { ActivationFormComponent } from "./activation-form/activation-form.component";
import { Routes } from "@angular/router";

import { RegistrationFormComponent } from "./registration-form/registration-form.component";
import { LoginFormComponent } from "./login-form/login-form.component";

export const AuthenticationRoutes: Routes = [
  {
    path: "",
    children: [
      {
        path: "login",
        component: LoginFormComponent,
      },
      {
        path: "register",
        component: RegistrationFormComponent,
      },
      {
        path: "activate",
        component: ActivationFormComponent,
      },
      {
        path: "access-denied",
        component: AccessDeniedComponent,
      },
    ],
  },
];
