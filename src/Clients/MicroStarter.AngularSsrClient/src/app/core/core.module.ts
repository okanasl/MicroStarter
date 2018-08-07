import { PlatformService } from "./services/platform.service";
import { CookieService } from "./services/cookie.service";
import { NgModule } from "@angular/core";

const SERVICES = [
    CookieService,
    PlatformService,
]
@NgModule({
    imports:[],
    providers:[SERVICES],
    declarations: [],
    exports: [],
})
export class CoreModule {
    static forRoot() {
        return {
          ngModule: CoreModule,
          providers: [ SERVICES ]
        }
      }
}