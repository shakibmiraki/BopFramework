import { AuthGuard } from "../core/services/auth.guard";
import { RootComponent } from "./root/root.component";

import { SharedModule } from "./../shared/shared.module";
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { HomeComponent } from "./home/home.component";
import { DashboardRoutes } from "./dashboard.routing";
import { TextMaskModule } from "angular2-text-mask";
import { LocalizationComponent } from "./localization/localization.component";
import { RouterModule } from "@angular/router";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    TextMaskModule,
    RouterModule.forChild(DashboardRoutes),
  ],
  declarations: [RootComponent, HomeComponent, LocalizationComponent],
  exports: [],
  providers: [AuthGuard],
})
export class DashboardModule {}
