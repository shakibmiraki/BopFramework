import { Language, LanguageList } from './../models/localization-languages';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiConfigService } from './api-config.service';
import { APP_CONFIG, IAppConfig } from './app.config';
import { map, retry } from 'rxjs/operators';
import { ResultType } from '../models/response';

@Injectable({
  providedIn: 'root'
})
export class LocalizationService {
  constructor(
    private http: HttpClient,
    @Inject(APP_CONFIG) private appConfig: IAppConfig,
    private apiConfigService: ApiConfigService
  ) {}

  exportLanguage(languageId: number): Observable<string> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http
      .post(
        `${this.appConfig.apiEndpoint}/${
          this.apiConfigService.configuration.exportLanguagePath
        }`,
        languageId,
        { headers }
      )
      .pipe(
        retry(1),
        map((response: any) => {
          if (response.result === 0) {
            return response.jsonFile;
          } else {
            return 'nothing';
          }
        })
      );
  }
  getAllLanguages(): Observable<Language[]> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http
      .post(
        `${this.appConfig.apiEndpoint}/${
          this.apiConfigService.configuration.getAllLanguagePath
        }`,
        null,
        { headers }
      )
      .pipe(
        retry(1),
        map((response: LanguageList) => {
          if (response.result === ResultType.Success) {
            return response.languages;
          } else {
            return null;
          }
        })
      );
  }
}
