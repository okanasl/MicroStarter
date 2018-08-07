import { Component, OnDestroy } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnDestroy {
    userData: any;
    isAuthorized: boolean;
  constructor(
        public oidcSecurityService: OidcSecurityService,
        public router: Router
    ) {
        this.oidcSecurityService.getUserData().subscribe(userdata=>{
            this.userData = userdata;
        })
        this.oidcSecurityService.getIsAuthorized().subscribe(isAuthorized=>{
            this.isAuthorized = isAuthorized;
        })
    }

    ngOnDestroy(): void {
        this.oidcSecurityService.onModuleSetup.unsubscribe();
        // this.oidcSecurityService.onAuthorizationResult.unsubscribe();
    }
    login() {
        this.oidcSecurityService.authorize((authUrl) => {
            if (window.addEventListener) {
                window.addEventListener('login_callback_message', this.popupCallbackLogic.bind(this), false);
            } else {
                (<any>window).attachEvent('login_callback_message', this.popupCallbackLogic.bind(this));
            }// handle the authorrization URL
            console.log(authUrl)
            this.popupCenter(authUrl, 'Login', 500, 540, null);
            // window.open(authUrl, '_blank', 'toolbar=1,location=111,menubar=0,left=,width=500,height=600');
        });
    }
    popupCenter(url, title, w, h, opts) {
        let _innerOpts = '';
        if (opts !== null && typeof opts === 'object' ) {
            for (const p in opts ) {
                if (opts.hasOwnProperty(p)) {
                    _innerOpts += p + '=' + opts[p] + ',';
                }
            }
        }
        const width = window.innerWidth ? window.innerWidth :
        document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
        const height = window.innerHeight ? window.innerHeight :
        document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

        const left = ((width / 2) - (w / 2)) ;
        const top = ((height / 2) - (h / 2)) ;
        const newWindow = window.open(url, title, _innerOpts + ' width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

        if (window.focus) {
            newWindow.focus();
        }
    }
    logout() {
        this.oidcSecurityService.logoff();
    }

    private popupCallbackLogic(event) {
        console.log(event)
        this.oidcSecurityService.authorizedCallback(event.detail);
    }
}