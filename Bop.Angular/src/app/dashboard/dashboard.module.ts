import { AuthGuard } from "../core/services/auth.guard";
import { SharedModule } from "./../shared/shared.module";
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HomeComponent } from "./home/home.component";
import { DashboardRoutes } from "./dashboard.routing";
import { RouterModule } from "@angular/router";

@NgModule({
  imports: [CommonModule, SharedModule, RouterModule.forChild(DashboardRoutes)],
  declarations: [HomeComponent],
  exports: [],
  providers: [AuthGuard],
})
export class DashboardModule {}
