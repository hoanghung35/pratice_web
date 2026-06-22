import { HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuthService } from "../services/auth.service";
import { catchError, switchMap, throwError } from "rxjs";
import { Router } from "@angular/router";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private auth: AuthService, private router: Router) { }

    intercept(req: HttpRequest<any>, next: HttpHandler) {
        return next.handle(req).pipe(
            catchError(err => {

                if (err.status === 401) {
                    return this.auth.refreshToken().pipe(
                        switchMap(() => next.handle(req)),
                        catchError(() => {
                            this.router.navigate(['/login']);
                            return throwError(() => err);
                        })
                    );
                }

                if (err.status === 403 || err.status === 404) {
                    this.router.navigate(['/page-error']);
                }

                return throwError(() => err);
            })
        );
    }
}
