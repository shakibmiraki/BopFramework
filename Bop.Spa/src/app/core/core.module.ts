import { NgModule, APP_INITIALIZER, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ApiConfigService } from './services/api-config.service';
import { APP_CONFIG, AppConfig } from './services/app.config';
import { HttpErrorInterceptor } from './services/intercept/http-error.interceptor';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { NotificationService } from './services/notification.service';
import { AuthInterceptor } from './services/intercept/auth.interceptor';
import { CultureInterceptor } from './services/intercept/culture.interceptor';

@NgModule({
  imports: [CommonModule, HttpClientModule, MatSnackBarModule],
  declarations: [],
  exports: [],
  providers: [
    {
      provide: APP_CONFIG,
      useValue: AppConfig
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorInterceptor,
      multi: true,
      deps: [NotificationService]
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: CultureInterceptor,
      multi: true
    },
    // {
    //   provide: MAT_SNACK_BAR_DEFAULT_OPTIONS,
    //   useValue: {duration: 8000}
    // },
    {
      provide: APP_INITIALIZER,
      useFactory: (config: ApiConfigService) => () => config.loadApiConfig(),
      deps: [ApiConfigService],
      multi: true
    }
  ]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() core: CoreModule) {
    if (core) {
      throw new Error('CoreModule should be imported ONLY in AppModule.');
    }
  }
}
