import { CardStatus } from './../models/card-status';
import { CardAuth } from 'src/app/core/models/card-auth';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiConfigService } from './api-config.service';
import { APP_CONFIG, IAppConfig } from './app.config';
import { map, retry } from 'rxjs/operators';
import { ResponseModel, ResultType } from '../models/response';
import { CardStatusList } from '../models/card-status';

@Injectable({
  providedIn: 'root'
})
export class CardService {
  constructor(
    private http: HttpClient,
    @Inject(APP_CONFIG) private appConfig: IAppConfig,
    private apiConfigService: ApiConfigService
  ) {}

  authenticate(model: CardAuth): Observable<boolean> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http
      .post(
        `${this.appConfig.apiEndpoint}/${
          this.apiConfigService.configuration.cardAuthPath
        }`,
        model,
        { headers }
      )
      .pipe(
        retry(1),
        map((response: ResponseModel) => {
          if (response.result === 0) {
            return true;
          } else {
            return false;
          }
        })
      );
  }

  getCards(): Observable<CardStatus[]> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http
      .get(
        `${this.appConfig.apiEndpoint}/${
          this.apiConfigService.configuration.getCardsPath
        }`,
        { headers }
      )
      .pipe(
        retry(1),
        map((response: CardStatusList) => {
          if (response.result === ResultType.Success) {
            return response.cards;
          } else {
            return null;
          }
        })
      );
  }

  enableCard(pan: string): Observable<boolean> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http
      .post(
        `${this.appConfig.apiEndpoint}/${
          this.apiConfigService.configuration.enableCardPath
        }`,
        pan,
        { headers }
      )
      .pipe(
        retry(1),
        map((response: ResponseModel) => {
          console.log(response);
          if (response.result === ResultType.Success) {
            return true;
          } else {
            return false;
          }
        })
      );
  }

  disableCard(pan: string): Observable<boolean> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http
      .post(
        `${this.appConfig.apiEndpoint}/${
          this.apiConfigService.configuration.disableCardPath
        }`,
        pan,
        { headers }
      )
      .pipe(
        retry(1),
        map((response: ResponseModel) => {
          console.log(response);
          if (response.result === ResultType.Success) {
            return true;
          } else {
            return false;
          }
        })
      );
  }
}
