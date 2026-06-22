import { NgModule } from "@angular/core";
import { AuthService } from "./services/auth.service";
import { AccountService } from "./services/account.service";
import { ItemService } from "./services/item.service";
import { RequestService } from "./services/request.service";
import { ApproveService } from "./services/approve.service";
import { InventService } from "./services/invent.service";
import { LoadingService } from "./services/loading.service";
import { DialogService } from "./services/dialog.service";
import { CurrencyService } from "./services/currency.service";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { JwtCookieInterceptor } from "./interceptors/jwt-cookie.interceptor";
import { ErrorInterceptor } from "./interceptors/error.interceptor";

@NgModule({
    providers: [
        AuthService,
        AccountService,
        ItemService,
        RequestService,
        ApproveService,
        InventService,
        CurrencyService,
        DialogService,
        LoadingService,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: JwtCookieInterceptor,
            multi: true
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: ErrorInterceptor,
            multi: true
        }
    ]
})
export class CoreModule { }
