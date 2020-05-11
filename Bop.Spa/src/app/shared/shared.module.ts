import { MaxValueDirective } from './directives/max-value.validator.directive';
import { MinValueDirective } from './directives/min-value.validator.directive';

import { MaterialModule } from './material-module';
import {
  NgModule,
  ModuleWithProviders,
  CUSTOM_ELEMENTS_SCHEMA
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient } from '@angular/common/http';

import { myFocus } from './directives/focus.directive';
import { ButtonSpinnerComponent } from './components/button-spinner/button-spinner.component';
import { PhoneValidator } from './directives/phone.validator.directive';
import { MustMatchValidator } from './directives/must-match.directive';
import { EmailValidator } from './directives/email.validator.directive';
import {
  IsAuthorizedDirective,
  ShowToAuthUserDirective
} from './directives/user.validator.directive';
import { MatProgressButtonsModule } from 'mat-progress-buttons';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { UploadComponent } from './components/upload/upload.component';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { toJalaaliPipe, formatJalaaliPipe } from './pipes/moment-jalaali.pipe';
import { formatTimePipe } from './pipes/time.pipe';
import { CountdownModule } from 'ngx-countdown';

// AoT requires an exported function for factories
export function HttpLoaderFactory(httpClient: HttpClient) {
  return new TranslateHttpLoader(httpClient);
}
@NgModule({
  imports: [

  CommonModule,
    HttpClientModule,
    MaterialModule,
    MatProgressButtonsModule,
    CountdownModule,
    TranslateModule.forChild({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  declarations: [
    myFocus,
    PhoneValidator,
    EmailValidator,
    MustMatchValidator,
    IsAuthorizedDirective,
    ShowToAuthUserDirective,
    MinValueDirective,
    MaxValueDirective,
    ButtonSpinnerComponent,
    SpinnerComponent,
    UploadComponent,
    toJalaaliPipe,
    formatJalaaliPipe,
    formatTimePipe
  ],
  exports: [
    myFocus,
    PhoneValidator,
    EmailValidator,
    MustMatchValidator,
    MaterialModule,
    IsAuthorizedDirective,
    ShowToAuthUserDirective,
    MinValueDirective,
    MaxValueDirective,
    MatProgressButtonsModule,
    ButtonSpinnerComponent,
    SpinnerComponent,
    UploadComponent,
    toJalaaliPipe,
    formatJalaaliPipe,
    formatTimePipe,
    CountdownModule,
    TranslateModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class SharedModule {
  static forRoot(): ModuleWithProviders<SharedModule> {
    // Forcing the whole app to use the returned providers from the AppModule only.
    return {
      ngModule: SharedModule,
      providers: [
        /* All of your services here. It will hold the services needed by `itself`. */
      ]
    };
  }
}
