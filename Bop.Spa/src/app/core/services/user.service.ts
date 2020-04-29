import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable, throwError } from "rxjs";
import { catchError, finalize, map, retry } from "rxjs/operators";
import { AuthTokenType } from "./../models/auth-token-type";
import { AuthUser } from "./../models/auth-user";
import { Credentials } from "./../models/credentials";
import { APP_CONFIG, IAppConfig } from "./app.config";
import { RefreshTokenService } from "./refresh-token.service";
import { TokenStoreService } from "./token-store.service";
import { RegistrationModel } from "../models/account-register";
import { AccountActivate } from "../models/account-activate";

@Injectable({
  providedIn: "root",
})
export class UserService {
  private authStatusSource = new BehaviorSubject<boolean>(false);
  authStatus$ = this.authStatusSource.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router,
    @Inject(APP_CONFIG) private appConfig: IAppConfig,
    private tokenStoreService: TokenStoreService,
    private refreshTokenService: RefreshTokenService
  ) {
    this.updateStatusOnPageRefresh();
    this.refreshTokenService.scheduleRefreshToken(
      this.isAuthUserLoggedIn(),
      false
    );
  }

  login(model: Credentials): Observable<boolean> {
    const headers = new HttpHeaders({ "Content-Type": "application/json" });

    return this.http
      .post(`${this.appConfig.paths.loginPath}`, model, { headers })
      .pipe(
        map((response: any) => {
          this.tokenStoreService.setRememberMe(model.rememberMe);
          if (!response) {
            this.authStatusSource.next(false);
            return false;
          }
          this.tokenStoreService.storeLoginSession(response);
          this.refreshTokenService.scheduleRefreshToken(true, true);
          this.authStatusSource.next(true);
          return true;
        }),
        catchError((error: HttpErrorResponse) => throwError(error))
      );
  }

  register(model: RegistrationModel): Observable<boolean> {
    const headers = new HttpHeaders({ "Content-Type": "application/json" });

    return this.http
      .post(`${this.appConfig.paths.registerPath}`, model, { headers })
      .pipe(map((response: any) => true));
  }

  activate(model: AccountActivate): Observable<boolean> {
    const headers = new HttpHeaders({ "Content-Type": "application/json" });

    return this.http
      .post(`${this.appConfig.paths.activatePath}`, model, { headers })
      .pipe(map((response: any) => true));
  }

  resend(model: AccountActivate): Observable<boolean> {
    const headers = new HttpHeaders({ "Content-Type": "application/json" });

    return this.http
      .post(`${this.appConfig.paths.resendPath}`, model, { headers })
      .pipe(map((response: any) => true));
  }

  getBearerAuthHeader(): HttpHeaders {
    return new HttpHeaders({
      "Content-Type": "application/json",
      Authorization: `Bearer ${this.tokenStoreService.getRawAuthToken(
        AuthTokenType.AccessToken
      )}`,
    });
  }

  logout(navigateToHome: boolean): void {
    const headers = new HttpHeaders({ "Content-Type": "application/json" });
    const refreshToken = encodeURIComponent(
      this.tokenStoreService.getRawAuthToken(AuthTokenType.RefreshToken)
    );
    this.http
      .get(`${this.appConfig.paths.logoutPath}?refreshToken=${refreshToken}`, {
        headers,
      })
      .pipe(
        map((response) => response || {}),
        catchError((error: HttpErrorResponse) => throwError(error)),
        finalize(() => {
          this.tokenStoreService.deleteAuthTokens();
          this.refreshTokenService.unscheduleRefreshToken(true);
          this.authStatusSource.next(false);
          if (navigateToHome) {
            this.router.navigate(["/"]);
            location.href = "/";
          }
        })
      )
      .subscribe((result) => {});
  }

  isAuthUserLoggedIn(): boolean {
    return (
      this.tokenStoreService.hasStoredAccessAndRefreshTokens() &&
      !this.tokenStoreService.isAccessTokenTokenExpired()
    );
  }

  getAuthUser(): AuthUser | null {
    if (!this.isAuthUserLoggedIn()) {
      return null;
    }

    const decodedToken = this.tokenStoreService.getDecodedAccessToken();
    const roles = this.tokenStoreService.getDecodedTokenRoles();
    return Object.freeze({
      userId:
        decodedToken[
          "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
        ],
      userName:
        decodedToken[
          "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
        ],
      displayName: decodedToken["DisplayName"],
      roles: roles,
    });
  }

  isAuthUserInRoles(requiredRoles: string[]): boolean {
    const user = this.getAuthUser();

    if (!user || !user.roles) {
      return false;
    }

    if (
      user.roles.indexOf(this.appConfig.config.adminRoleName.toLowerCase()) >= 0
    ) {
      return true; // The `Admin` role has full access to every pages.
    }

    return requiredRoles.some((requiredRole) => {
      if (user.roles) {
        return user.roles.indexOf(requiredRole.toLowerCase()) >= 0;
      } else {
        return false;
      }
    });
  }

  isAuthUserInRole(requiredRole: string): boolean {
    return this.isAuthUserInRoles([requiredRole]);
  }

  // IsAuthorize(permission: PermissionRecord): Observable<boolean> {
  //   const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

  //   return this.http
  //     .post(
  //       `${this.appConfig.apiEndpoint}/${
  //         this.apiConfigService.configuration.verifyAccessPath
  //       }`,
  //       permission,
  //       { headers }
  //     )
  //     .pipe(
  //       map((response: ResponseModel) => {
  //         if (response.result === ResultType.Success) {
  //           return true;
  //         } else {
  //           return false;
  //         }
  //       }),
  //       catchError((error: HttpErrorResponse) => throwError(error))
  //     );
  // }

  private updateStatusOnPageRefresh(): void {
    this.authStatusSource.next(this.isAuthUserLoggedIn());
  }
}
