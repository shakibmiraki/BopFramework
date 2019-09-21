import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpResponse,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError, tap } from 'rxjs/operators';
import { NotificationService } from '../notification.service';
import { ResponseModel } from '../../models/response';

export class HttpErrorInterceptor implements HttpInterceptor {
  constructor(public notifier: NotificationService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      //retry(1),
      tap(evt => {
        if (evt instanceof HttpResponse) {
          const response: ResponseModel = evt.body;

          if (response && response.messages) {
            this.notifier.showMessage(response.messages);
          }
        }
      }),
      catchError((error: HttpErrorResponse) => {
        const response: ResponseModel = error.error;

        if (response && response.messages) {
          this.notifier.showMessage(response.messages);
        }
        return throwError(error);
      })
    );
  }
}
