import { DashboardModule } from "./dashboard/dashboard.module";
import { AuthGuard } from "./core/services/auth.guard";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { CoreModule } from "./core/core.module";
import { SharedModule, HttpLoaderFactory } from "./shared/shared.module";
import { BlankComponent } from "./shared/layouts/blank/blank.component";
import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { HttpClientModule, HttpClient } from "@angular/common/http";
import { AppRoutes } from "./app-routing";
import { AppComponent } from "./app.component";
import { TranslateModule, TranslateLoader } from "@ngx-translate/core";
import { AuthenticationModule } from "./authentication/authentication.module";
import { HeaderComponent } from "./shared/components/header/header.component";
import { SidenavListComponent } from "./dashboard/navigation/sidenav-list/sidenav-list.component";
import { PageNotFoundComponent } from "./shared/components/page-not-found/page-not-found.component";

import { RouterModule } from "@angular/router";

@NgModule({
  declarations: [
    AppComponent,
    BlankComponent,
    HeaderComponent,
    SidenavListComponent,
    PageNotFoundComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    CoreModule,
    SharedModule.forRoot(),
    AuthenticationModule,
    DashboardModule,
    RouterModule.forRoot(AppRoutes),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
    }),
  ],
  providers: [AuthGuard],
  bootstrap: [AppComponent],
})
export class AppModule {}
