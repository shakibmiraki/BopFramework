import { NgModule, APP_INITIALIZER, Optional, SkipSelf } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { APP_CONFIG, AppConfig } from "./services/app.config";
import { HttpErrorInterceptor } from "./services/intercept/http-error.interceptor";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { NotificationService } from "./services/notification.service";
import { AuthInterceptor } from "./services/intercept/auth.interceptor";
import { CultureInterceptor } from "./services/intercept/culture.interceptor";

@NgModule({
  imports: [CommonModule, MatSnackBarModule],
  declarations: [],
  exports: [],
  providers: [
    {
      provide: APP_CONFIG,
      useValue: AppConfig,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorInterceptor,
      multi: true,
      deps: [NotificationService],
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: CultureInterceptor,
      multi: true,
    },
  ],
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() core: CoreModule) {
    console.log('core loaded')
    if (core) {
      throw new Error("CoreModule should be imported ONLY in AppModule.");
    }
  }
}
