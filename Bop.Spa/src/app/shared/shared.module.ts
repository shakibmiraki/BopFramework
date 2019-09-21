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
import { ButtonSpinnerComponent } from '../button-spinner/button-spinner.component';
import { PhoneValidator } from './directives/phone.validator.directive';
import { MustMatchValidator } from './directives/must-match.directive';
import { EmailValidator } from './directives/email.validator.directive';
import {
  IsAuthorizedDirective,
  ShowToAuthUserDirective
} from './directives/user.validator.directive';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { MatProgressButtonsModule } from 'mat-progress-buttons';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { UploadComponent } from '../upload/upload.component';
import { ConfirmationDialogComponent } from './dialog/confirmation.dialog.component';
import { CountdownModule } from 'ngx-countdown';
import { SpinnerComponent } from '../spinner/spinner.component';
import { InfiniteScrollComponent } from '../infinite-scroll/infinite-scroll.component';
import { toJalaaliPipe, formatJalaaliPipe } from './pipes/moment-jalaali.pipe';
import { formatTimePipe } from './pipes/time.pipe';
import { DpDatePickerModule } from 'ng2-jalali-date-picker';

// AoT requires an exported function for factories
export function HttpLoaderFactory(httpClient: HttpClient) {
  return new TranslateHttpLoader(httpClient);
}
@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    MaterialModule,
    MDBBootstrapModule,
    MatProgressButtonsModule,
    CountdownModule,
    DpDatePickerModule,
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
    InfiniteScrollComponent,
    ConfirmationDialogComponent,
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
    MDBBootstrapModule,
    MinValueDirective,
    MaxValueDirective,
    MatProgressButtonsModule,
    ButtonSpinnerComponent,
    SpinnerComponent,
    UploadComponent,
    ConfirmationDialogComponent,
    CountdownModule,
    InfiniteScrollComponent,
    toJalaaliPipe,
    formatJalaaliPipe,
    formatTimePipe,
    DpDatePickerModule,
    TranslateModule
  ],
  entryComponents: [ConfirmationDialogComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class SharedModule {
  static forRoot(): ModuleWithProviders {
    // Forcing the whole app to use the returned providers from the AppModule only.
    return {
      ngModule: SharedModule,
      providers: [
        /* All of your services here. It will hold the services needed by `itself`. */
      ]
    };
  }
}
