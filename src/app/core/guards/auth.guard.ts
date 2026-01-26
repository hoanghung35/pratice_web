import { Injectable } from "@angular/core";
import { AuthService } from "../services/auth.service";
import { CanActivate, Router } from "@angular/router";
import { catchError, map, Observable, of } from "rxjs";


@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(private auth: AuthService, private router: Router) { }

    canActivate(): Observable<boolean> {
        return this.auth.getMe().pipe(
            map(() => true),
            catchError(() => {
                this.router.navigate(['/login']);
                return of(false);
            })
        );
    }
}
