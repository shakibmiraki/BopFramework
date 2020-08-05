import { AccountActivate } from './../../core/models/account-activate';
import { Component, Inject, ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/core/services/user.service';
import { IAppConfig, APP_CONFIG } from 'src/app/core/services/app.config';
import { finalize } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { CountdownComponent } from 'ngx-countdown';

@Component({
  selector: 'app-activation',
  templateUrl: './activation-form.component.html'
})
export class ActivationFormComponent {

  @ViewChild(CountdownComponent) counter: CountdownComponent;
  
  model: AccountActivate = {
    phone: localStorage.getItem(this.appConfig.config.tokenKey),
    token: ''
  };

  errors: string;
  isRequesting: boolean;
  submitted = false;
  message = '';
  needResend = false;

  public tokenMask = [/[0-9]/, /\d/, /\d/, /\d/, /\d/];

  constructor(
    @Inject(APP_CONFIG) private appConfig: IAppConfig,
    private userService: UserService,
    private router: Router,
    private translate: TranslateService
  ) {}

  onSubmit() {
    this.submitted = true;
    this.isRequesting = true;
    this.errors = '';

    this.userService
      .activate(this.model)
      .pipe(finalize(() => (this.isRequesting = false)))
      .subscribe(
        result => {
          if (result) {
            this.router.navigate(['/login'], {
              queryParams: {
                isActivatedUser: true,
                phone: localStorage.getItem(this.appConfig.config.tokenKey)
              }
            });
          }
        },
        error => (this.errors = error)
      );
  }

  resend() {
    this.isRequesting = true;

    this.userService
      .resend(this.model)
      .pipe(
        finalize(() => {
          this.isRequesting = false;
          this.needResend = false;
          this.counter.restart();
        })
      )
      .subscribe(
        result => {
          // if (result) {
          //   this.router.navigate(['/login'], {
          //     queryParams: {
          //       isActivatedUser: true,
          //       phone: localStorage.getItem(this.appConfig.config.tokenKey)
          //     }
          //   });
          // }
        },
        error => (this.errors = error)
      );
  }

  onFinished() {
    this.needResend = true;
  }
}
