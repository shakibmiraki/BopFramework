import { PermissionRecord } from './../../core/models/permission-record';
import {
  Directive,
  OnInit,
  TemplateRef,
  ViewContainerRef,
  Input
} from '@angular/core';

import { UserService } from 'src/app/core/services/user.service';
import { Observable } from 'rxjs';

@Directive({ selector: '[IsAuthorized]' })
export class IsAuthorizedDirective implements OnInit {
  subscription;
  @Input('IsAuthorized') roleName;

  model: PermissionRecord = { PermissionSystemName: '' };

  isLoggedIn$ = this.userService.authStatus$;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.isLoggedIn$.subscribe(isauth => {
      if (isauth) {
        const isAuthorized = this.userService.isAuthUserInRole(this.roleName);
        if (isAuthorized) {
          this.viewContainer.clear();
          this.viewContainer.createEmbeddedView(this.templateRef);
        } else {
          this.viewContainer.clear();
        }
      }
    });
  }
}

@Directive({ selector: '[ShowToAuthUser]' })
export class ShowToAuthUserDirective implements OnInit {
  subscription;
  @Input('ShowToAuthUser') renderTemplate;

  isLoggedIn$ = this.userService.authStatus$;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.isLoggedIn$.subscribe(isauth => {
      if (isauth) {
        if (this.renderTemplate) {
          this.viewContainer.createEmbeddedView(this.templateRef);
        } else {
          this.viewContainer.clear();
        }
      } else {
        if (this.renderTemplate) {
          this.viewContainer.clear();
        } else {
          this.viewContainer.clear();
          this.viewContainer.createEmbeddedView(this.templateRef);
        }
      }
    });
  }
}
