import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { SharedModule } from "../shared/shared.module";
import { UserService } from "../core/services/user.service";
import { AuthenticationRoutes } from "./authentication.routing";
import { RegistrationFormComponent } from "./registration-form/registration-form.component";
import { LoginFormComponent } from "./login-form/login-form.component";
import { ActivationFormComponent } from "./activation-form/activation-form.component";
import { TextMaskModule } from "angular2-text-mask";
import { AccessDeniedComponent } from "./access-denied/access-denied.component";
import { RouterModule } from "@angular/router";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    RouterModule.forChild(AuthenticationRoutes),
    TextMaskModule,
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
