import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { TokenService } from '../services/token.service';
import { SnackService } from '../services/snack.service';
import { Router } from '@angular/router';
import { map } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(
    private token: TokenService,
    private snackService: SnackService,
    private router: Router
  ) {}

  /**
   * Set bearer header from jwt and check errors
   * @param request Request to intercept
   * @param next Next request handler function
   * @returns 
   */
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // Get the auth token from the service.
    const authToken = this.token.getAccessToken();

    // Clone the request and replace the original headers with
    // cloned headers, updated with the authorization.
    const authReq = request.clone({
      setHeaders: { Authorization: 'Bearer ' + authToken }
    });
    
    // send cloned request with header to the next handler.
    return next.handle(authReq).pipe(map((event: HttpEvent<any>) => {
      if (event instanceof HttpResponse) {
        if(event.status == 401) {
          this.router.navigate(["/login"]);
        }
        if (event.status >= 400) {
          this.snackService.openSnackBar(event.statusText, "OK");
        }
      }
      return event;
    }));
  }
}
