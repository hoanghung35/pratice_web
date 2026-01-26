import { Injectable } from "@angular/core";
import { AuthService } from "../services/auth.service";
import { CanActivate, GuardResult, MaybeAsync, Router, RouterStateSnapshot } from "@angular/router";
import { ActivatedRouteSnapshot } from "@angular/router";
import { catchError, map, Observable, of } from "rxjs";

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
    constructor(private auth: AuthService, private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
        const roles = route.data['roles'] as string[];

        return this.auth.getMe().pipe(
            map(user => roles.includes(user.role)),
            catchError(() => of(false))
        );
    }
}
