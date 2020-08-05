import { RefreshTokenService } from "./core/services/refresh-token.service";
import { Component, HostListener } from "@angular/core";
import { fadeAnimation } from "./route-animation";
import { TranslateService } from "@ngx-translate/core";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"],
  animations: [fadeAnimation],
})
export class AppComponent {
  title = "Digital Receipt";

  constructor(
    private refreshTokenService: RefreshTokenService,
    translate: TranslateService
  ) {
    translate.addLangs(["en-US", "fa-IR"]);
    translate.setDefaultLang("fa-IR");
    const browserLang = translate.getBrowserLang();
    translate.use("fa-IR");
  }

  public getRouterOutletState(outlet) {
    return outlet.isActivated ? outlet.activatedRoute : "";
  }

  @HostListener("window:unload", ["$event"])
  unloadHandler() {
    // Invalidate current tab as active RefreshToken timer
    this.refreshTokenService.invalidateCurrentTabId();
  }

  @HostListener("window:beforeunload", ["$event"])
  beforeUnloadHander() {
    // ...
  }
}
