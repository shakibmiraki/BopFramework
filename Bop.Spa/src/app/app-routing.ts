import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { BlankComponent } from "./shared/layouts/blank/blank.component";
import { PageNotFoundComponent } from "./shared/components/page-not-found/page-not-found.component";
import { FullComponent } from "./shared/layouts/full/full.component";

export const AppRoutes: Routes = [
  {
    path: "",
    component: FullComponent,
    children: [
      {
        path: "",
        loadChildren: () =>
          import("./dashboard/dashboard.module").then((m) => m.DashboardModule),
      },
    ],
  },
  {
    path: "",
    component: BlankComponent,
    children: [
      {
        path: "",
        loadChildren: () =>
          import("./authentication/authentication.module").then(
            (m) => m.AuthenticationModule
          ),
      },
    ],
  },
  {
    path: "**",
    component: PageNotFoundComponent,
  },
];
