import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { SharedModule } from "../shared/shared.module";
import { UserService } from "../core/services/user.service";
import { AuthenticationRoutes } from "./authentication.routing";
import { RegistrationFormComponent } from "./registration-form/registration-form.component";
import { LoginFormComponent } from "./login-form/login-form.component";
import { ActivationFormComponent } from "./activation-form/activation-form.component";
import { AccessDeniedComponent } from "./access-denied/access-denied.component";
import { RouterModule } from "@angular/router";

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(AuthenticationRoutes),
  ],
  declarations: [
    RegistrationFormComponent,
    LoginFormComponent,
    ActivationFormComponent,
    AccessDeniedComponent,
  ],
  providers: [UserService],
  exports: [],
})
export class AuthenticationModule {}
