import { Injectable, Inject } from "@angular/core";
import { HttpHeaders, HttpClient, HttpEvent } from "@angular/common/http";
import { IAppConfig, APP_CONFIG } from "./app.config";
import { retry } from "rxjs/operators";
import { Observable } from "rxjs/internal/Observable";

@Injectable({
  providedIn: "root",
})
export class FileService {
  constructor(
    private http: HttpClient,
    @Inject(APP_CONFIG) private appConfig: IAppConfig
  ) {}

  upload(file: FormData, languageId: string): Observable<HttpEvent<Object>> {
    return this.http
      .post(`${this.appConfig.paths.importLanguagePath}`, file, {
        reportProgress: true,
        observe: "events",
      })
      .pipe(retry(1));
  }
}
