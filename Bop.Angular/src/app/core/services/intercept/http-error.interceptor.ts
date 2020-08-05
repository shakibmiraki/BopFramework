import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpResponse,
  HttpErrorResponse,
} from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError, tap } from "rxjs/operators";
import { NotificationService } from "../notification.service";
import { ResponseModel } from "../../models/response";
import { Injectable } from "@angular/core";

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  constructor(public notifier: NotificationService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    console.log(request);
    return next.handle(request).pipe(
      //retry(1),
      tap((evt) => {
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
