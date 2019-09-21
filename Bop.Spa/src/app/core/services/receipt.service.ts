import { ReceiptRequest, ReceiptResponse, Receipt } from './../models/receipt';
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
export class ReceiptService {
  constructor(
    private http: HttpClient,
    @Inject(APP_CONFIG) private appConfig: IAppConfig,
    private apiConfigService: ApiConfigService
  ) {}

  getReceipts(model: ReceiptRequest): Observable<Receipt[]> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http
      .post(
        `${this.appConfig.apiEndpoint}/${
          this.apiConfigService.configuration.getReceiptsPath
        }`,
        model,
        { headers }
      )
      .pipe(
        retry(1),
        map((response: ReceiptResponse) => {
          if (response.result === ResultType.Success) {
            return response.receipts;
          } else {
            return null;
          }
        })
      );
  }
}
