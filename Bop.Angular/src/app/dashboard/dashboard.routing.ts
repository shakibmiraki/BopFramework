import { Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { AuthGuard } from "../core/services/auth.guard";
import { AuthGuardPermission } from "../core/models/auth-guard-permission";

export const DashboardRoutes: Routes = [
  {
    path: "dashboard",
    canActivate: [AuthGuard],
    children: [
      {
        path: "",
        component: HomeComponent,
        data: {
          permission: {
            permittedRoles: ["Registered"],
          } as AuthGuardPermission,
        },
        canActivate: [AuthGuard],
      },
      {
        path: "home",
        component: HomeComponent,
        data: {
          permission: {
            permittedRoles: ["Registered"],
          } as AuthGuardPermission,
        },
        canActivate: [AuthGuard],
      }
    ],
  },
];
