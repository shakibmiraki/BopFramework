import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';

@Injectable()
export class CultureInterceptor implements HttpInterceptor {
  constructor(private translate: TranslateService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    let defaultLanguage = this.translate.getDefaultLang();
    if (!defaultLanguage) {
      defaultLanguage = 'fa-IR';
    }
    request = request.clone({
      headers: request.headers.set('Accept-Language', defaultLanguage)
    });

    return next.handle(request);
  }
}
