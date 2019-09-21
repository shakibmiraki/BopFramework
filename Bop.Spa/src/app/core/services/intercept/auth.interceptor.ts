import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of, throwError } from 'rxjs';
import { catchError, delay, mergeMap, retryWhen, take } from 'rxjs/operators';
import { TokenStoreService } from '../token-store.service';
import { AuthTokenType } from '../../models/auth-token-type';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private delayBetweenRetriesMs = 1000;
  private numberOfRetries = 0;
  private authorizationHeader = 'Authorization';

  constructor(
    private tokenStoreService: TokenStoreService,
    private router: Router
  ) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const accessToken = this.tokenStoreService.getRawAuthToken(
      AuthTokenType.AccessToken
    );

    if (accessToken) {
      request = request.clone({
        headers: request.headers.set(
          this.authorizationHeader,
          `Bearer ${accessToken}`
        )
      });
      return next.handle(request).pipe(
        retryWhen(errors =>
          errors.pipe(
            mergeMap((error: HttpErrorResponse, retryAttempt: number) => {
              if (retryAttempt === this.numberOfRetries - 1) {
                return throwError(error); // no retry
              }

              switch (error.status) {
                case 400:
                case 404:
                  return throwError(error); // no retry
              }

              return of(error); // retry
            }),
            delay(this.delayBetweenRetriesMs),
            take(this.numberOfRetries)
          )
        ),
        catchError((error: any, caught: Observable<HttpEvent<any>>) => {
          console.log('auth.interceptor-->error-->');
          console.log(error);
          if (error.status === 401 || error.status === 403) {
            const newRequest = this.getNewAuthRequest(request);
            console.log('auth.interceptor-->newRequest-->');
            console.log(newRequest);
            if (newRequest) {
              return next.handle(newRequest);
            }
            this.router.navigate(['/login']);
          }
          return throwError(error);
        })
      );
    } else {
      // login page
      return next.handle(request);
    }
  }

  getNewAuthRequest(request: HttpRequest<any>): HttpRequest<any> | null {
    const newStoredToken = this.tokenStoreService.getRawAuthToken(
      AuthTokenType.AccessToken
    );
    const requestAccessTokenHeader = request.headers.get(
      this.authorizationHeader
    );
    if (!newStoredToken || !requestAccessTokenHeader) {
      return null;
    }
    const newAccessTokenHeader = `Bearer ${newStoredToken}`;
    if (requestAccessTokenHeader === newAccessTokenHeader) {
      return null;
    }
    return request.clone({
      headers: request.headers.set(
        this.authorizationHeader,
        newAccessTokenHeader
      )
    });
  }
}
