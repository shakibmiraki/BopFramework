import { Inject, OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../core/services/user.service';
import { RegistrationModel } from '../../core/models/account-register';
import { finalize } from 'rxjs/operators';
import { IAppConfig, APP_CONFIG } from 'src/app/core/services/app.config';
import * as AOS from 'aos';

@Component({
  selector: 'registeration-form',
  templateUrl: './registration-form.component.html'
})
export class RegistrationFormComponent implements OnInit {

  model: RegistrationModel = { phone: '', password: '', confirmPassword: '' };
  errors: string;
  isRequesting: boolean;
  submitted = false;

  constructor(
    @Inject(APP_CONFIG) private appConfig: IAppConfig,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    AOS.init({
      duration: 900,
      delay: 100,
      once: true
    });
  }


  onSubmit() {
    this.submitted = true;
    this.isRequesting = true;
    this.errors = '';

    this.userService
      .register(this.model)
      .pipe(finalize(() => (this.isRequesting = false)))
      .subscribe(
        result => {
          if (result) {
            localStorage.setItem(this.appConfig.config.tokenKey, this.model.phone);
            this.router.navigate(['/activate']);
          }
        },
        error => (this.errors = error)
      );
  }
}
