"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var platform_browser_dynamic_1 = require("@angular/platform-browser-dynamic");
var appModule = require("./app/app.module");
var environment = require("./environments/environment");
if (environment.environment.production) {
    core_1.enableProdMode();
}
platform_browser_dynamic_1.platformBrowserDynamic().bootstrapModule(appModule.AppModule)
    .catch(function (err) { return console.error(err); });
//# sourceMappingURL=main.js.map