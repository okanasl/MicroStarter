import {Component} from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
  <h1>Your Angular App (Universal)</h1><br/>
  <a routerLink="/">Home</a><br/>
  <a routerLink="/login">Login Page</a><br/>
  <a routerLink="/lazy">Lazy</a><br/>
  <a routerLink="/lazy/nested">Lazy_Nested</a><br/>
  <router-outlet></router-outlet>
  `,
  styles: []
})
export class AppComponent {

}
