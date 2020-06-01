import { MaxValueDirective } from "./directives/max-value.validator.directive";
import { MinValueDirective } from "./directives/min-value.validator.directive";
import { MaterialModule } from "./material-module";
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { myFocus } from "./directives/focus.directive";
import { ButtonSpinnerComponent } from "./components/button-spinner/button-spinner.component";
import { PhoneValidator } from "./directives/phone.validator.directive";
import { MustMatchValidator } from "./directives/must-match.directive";
import { EmailValidator } from "./directives/email.validator.directive";
import {
  IsAuthorizedDirective,
  ShowToAuthUserDirective,
} from "./directives/user.validator.directive";
import { MatProgressButtonsModule } from "mat-progress-buttons";
import { TranslateModule } from "@ngx-translate/core";
import { UploadComponent } from "./components/upload/upload.component";
import { SpinnerComponent } from "./components/spinner/spinner.component";
import { toJalaaliPipe, formatJalaaliPipe } from "./pipes/moment-jalaali.pipe";
import { formatTimePipe } from "./pipes/time.pipe";
import { CountdownModule } from "ngx-countdown";
import { TextMaskModule } from "angular2-text-mask";
import { FullComponent } from "./layouts/full/full.component";
import { BlankComponent } from "./layouts/blank/blank.component";
import { HeaderComponent } from "./components/header/header.component";
import { PageNotFoundComponent } from "./components/page-not-found/page-not-found.component";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { SidenavListComponent } from "./components/sidenav-list/sidenav-list.component";


@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    RouterModule,
    MaterialModule,
    MatProgressButtonsModule,
    TextMaskModule,
    CountdownModule,
    TranslateModule,
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
    formatTimePipe,
    FullComponent,
    BlankComponent,
    HeaderComponent,
    PageNotFoundComponent,
    SidenavListComponent,
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
    TextMaskModule,
    FullComponent,
    BlankComponent,
    HeaderComponent,
    TranslateModule,
    PageNotFoundComponent,
    FormsModule,
  ],
  // schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class SharedModule {}
