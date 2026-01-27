import { NgModule } from "@angular/core";
import { AuthService } from "./services/auth.service";
import { UserService } from "./services/user.service";
import { ItemService } from "./services/item.service";
import { OrderService } from "./services/order.service";
import { ApproveService } from "./services/approve.service";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { JwtCookieInterceptor } from "./interceptors/jwt-cookie.interceptor";
import { ErrorInterceptor } from "./interceptors/error.interceptor";

@NgModule({
    providers: [
        AuthService,
        UserService,
        ItemService,
        OrderService,
        ApproveService,
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
