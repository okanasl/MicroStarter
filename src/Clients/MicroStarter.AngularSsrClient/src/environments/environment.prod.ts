export const environment = {
  production: true,
  //& region (authorization)
  authConfig : {
    stsServer: 'http://microstarter.identity.localhost',
    redirect_url: 'http://microstarter.angularssrclient.localhost/login-callback.html',
    client_id: 'MicroStarter.AngularSsrClient',
    response_type: 'id_token token',
    scope: 'openid email profile bar_api_fullaccess',
    post_logout_redirect_uri: 'http://microstarter.angularssrclient.localhost',
    start_checksession: true,
    silent_renew: true,
    silent_renew_url: 'http://microstarter.angularssrclient.localhost/silent-renew.html',
    startup_route: '/',
    forbidden_route: '/',
    unauthorized_route: '/',
    log_console_warning_active: true,
    log_console_debug_active: false,
    max_id_token_iat_offset_allowed_in_seconds: '10',
  }
  //& end (authorization)
};
