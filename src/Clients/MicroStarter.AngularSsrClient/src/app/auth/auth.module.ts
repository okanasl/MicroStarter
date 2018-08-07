import { NgModule, APP_INITIALIZER } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './components/login/login.component';
import { AuthGuard } from './auth.guard';
import { OidcConfigService, AuthModule, OpenIDImplicitFlowConfiguration, OidcSecurityService, AuthWellKnownEndpoints } from 'angular-auth-oidc-client';
import { environment } from '../../environments/environment';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './auth.interceptor';
import {AuthStorage} from './auth.storage';

export function loadConfig(oidcConfigService: OidcConfigService) {
    console.log('GET AUTH CONFIG FROM ENVIRONMENT');
    // You can fetch it from your api
    return () => environment.authConfig;
  }

@NgModule({
    declarations: [  LoginComponent ],
    imports: [
        CommonModule,
        HttpClientModule,
        AuthModule.forRoot({ storage: AuthStorage })
    ],
    providers: [
        AuthGuard,
        {
            provide: APP_INITIALIZER,
            useFactory: loadConfig,
            deps: [OidcConfigService],
            multi: true
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthInterceptor,
            multi: true
        }
    ],
})

export class AuthClModule {
    constructor(
        private oidcSecurityService: OidcSecurityService
    ) {
            const config = environment.authConfig;
            const openIDImplicitFlowConfiguration = new OpenIDImplicitFlowConfiguration();
            openIDImplicitFlowConfiguration.stsServer = config.stsServer;
            openIDImplicitFlowConfiguration.redirect_url = config.redirect_url;
            openIDImplicitFlowConfiguration.trigger_authorization_result_event = true;
            // The Client MUST validate that the aud (audience) Claim contains its client_id value registered at the Issuer
            // identified by the iss (issuer) Claim as an audience.
            // The ID Token MUST be rejected if the ID Token does not list the Client as a valid audience,
            // or if it contains additional audiences not trusted by the Client.
            openIDImplicitFlowConfiguration.client_id = config.client_id;
            openIDImplicitFlowConfiguration.response_type = config.response_type;
            openIDImplicitFlowConfiguration.scope = config.scope;
            openIDImplicitFlowConfiguration.post_logout_redirect_uri = config.post_logout_redirect_uri;
            openIDImplicitFlowConfiguration.start_checksession = typeof window !== 'undefined' ? config.start_checksession : false;
            openIDImplicitFlowConfiguration.silent_renew = typeof window !== 'undefined' ? config.silent_renew : false;
            openIDImplicitFlowConfiguration.silent_renew_url = config.silent_renew_url;
            openIDImplicitFlowConfiguration.post_login_route = config.startup_route;
            // HTTP 403
            openIDImplicitFlowConfiguration.forbidden_route = config.forbidden_route;
            // HTTP 401
            openIDImplicitFlowConfiguration.unauthorized_route = config.unauthorized_route;
            openIDImplicitFlowConfiguration.log_console_warning_active = config.log_console_warning_active;
            openIDImplicitFlowConfiguration.log_console_debug_active = config.log_console_debug_active;
            // id_token C8: The iat Claim can be used to reject tokens that were issued too far away from the current time,
            // limiting the amount of time that nonces need to be stored to prevent attacks.The acceptable range is Client specific.
            openIDImplicitFlowConfiguration.max_id_token_iat_offset_allowed_in_seconds = 20;
            openIDImplicitFlowConfiguration.storage = AuthStorage;
    
            const authWellKnownEndpoints = new AuthWellKnownEndpoints();
            authWellKnownEndpoints.issuer = config.stsServer;
            authWellKnownEndpoints.jwks_uri = `${config.stsServer}/.well-known/openid-configuration/jwks`;
            authWellKnownEndpoints.authorization_endpoint = `${config.stsServer}/connect/authorize`;
            authWellKnownEndpoints.token_endpoint = `${config.stsServer}/connect/token`;
            authWellKnownEndpoints.userinfo_endpoint = `${config.stsServer}/connect/userinfo`;
            authWellKnownEndpoints.end_session_endpoint = `${config.stsServer}/connect/endsession`;
            authWellKnownEndpoints.check_session_iframe = `${config.stsServer}/connect/checksession`;
            authWellKnownEndpoints.revocation_endpoint = `${config.stsServer}/connect/revocation`;
            authWellKnownEndpoints.introspection_endpoint = `${config.stsServer}/connect/introspect`;
            this.oidcSecurityService.setupModule(openIDImplicitFlowConfiguration, authWellKnownEndpoints);
    
      }
}