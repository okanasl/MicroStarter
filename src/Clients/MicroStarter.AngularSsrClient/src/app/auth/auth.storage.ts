import { Injectable } from '@angular/core';
import { OidcSecurityStorage } from 'angular-auth-oidc-client';
import { CookieService } from '../core/services/cookie.service';

@Injectable()
export class AuthStorage implements OidcSecurityStorage {
    constructor(private cookieService: CookieService){

    }
    public read(key: string): any {
        return this.cookieService.get(key);
    }

    public write(key: string, value: any): void {
        return this.cookieService.set(key,value);
    }
}