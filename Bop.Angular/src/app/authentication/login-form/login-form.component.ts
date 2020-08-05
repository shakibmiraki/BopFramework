import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Credentials } from '../../core/models/credentials';
import { UserService } from '../../core/services/user.service';
import { ActivatedRoute, Router } from '@angular/router';
import * as AOS from 'aos';

@Component({
  selector: 'login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.scss']
})
export class LoginFormComponent implements OnInit, OnDestroy {
  constructor(
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute
  ) {}
  private subscription: Subscription;

  model: Credentials = { phone: '', password: '', rememberMe: false };
  errors = '';
  returnUrl: string | null = null;
  isRequesting: boolean;
  submitted = false;
  isActivatedUser: boolean;

  ngOnInit() {

    console.log('login component loaded');
    
    AOS.init({
      duration: 900,
      delay: 100,
      once: true
    });

    // reset the login status
    this.userService.logout(false);

    // get the return url from route parameters
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'];

    // subscribe to router event
    this.subscription = this.route.queryParams.subscribe((param: any) => {
      this.isActivatedUser = param['isActivatedUser'];
      this.model.phone = param['phone'];
    });

  }

  public onSubmit() {
    this.submitted = true;
    this.isRequesting = true;
    this.errors = '';

    this.userService
      .login(this.model)
      .pipe(finalize(() => (this.isRequesting = false)))
      .subscribe(isLoggedIn => {
        if (isLoggedIn) {
          window.location.href = '/dashboard/home';
        }
      });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
