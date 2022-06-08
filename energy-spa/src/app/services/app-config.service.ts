import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AppConfig } from './app-config';

const configRoute = '/spa-config';

@Injectable({ providedIn: 'root' })
export class AppConfigService {
  private appConfig?: AppConfig;

  constructor(private http: HttpClient) {}

  loadAppConfig(): Observable<void> {
    const configUrl = `${environment.serverUri}${configRoute}`;
    const headers = {
      Accept: 'application/json',
    };
    return this.http.get<AppConfig>(configUrl, { headers }).pipe(
      map((data) => {
        this.appConfig = data;
        if (environment.debug) {
          console.log('Client configuration:', data);
        }
      }),
      catchError((error: HttpErrorResponse) => {
        console.error(
          'Unable to retrieve client configuration.',
          error.message
        );
        throw error;
      })
    );
  }

  getConfig(): AppConfig | undefined {
    return this.appConfig;
  }
}
