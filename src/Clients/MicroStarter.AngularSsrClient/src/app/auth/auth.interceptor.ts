import { Injectable } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(
        private oidcSecurityService: OidcSecurityService ) { }
    intercept( request: HttpRequest<any>, next: HttpHandler ): Observable<HttpEvent<any>> {
        const token = this.oidcSecurityService.getToken();
        request = request.clone( {
            setHeaders: {
                Authorization: `Bearer ${token}`
            }
        } );

        return next.handle( request ).pipe(
            tap(( event: HttpEvent<any> ) => {
                if ( event instanceof HttpResponse ) {
                    // do stuff with response if you want
                }
            }, ( err: any ) => {
                if ( err instanceof HttpErrorResponse ) {
                    if ( err.status === 401 ) {
                        // this.router.navigate( ['/unauthorized'] );
                        // this.oidcSecurityService.authorize();
                    }
                }
             } )
        );
    }
}