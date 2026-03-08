import { Injectable } from "@angular/core";
import { CanActivate, ActivatedRouteSnapshot, Router } from "@angular/router";
import { AuthService, UserInfo } from "../services/auth.service";
import { map, tap } from "rxjs/operators";

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
    constructor(private auth: AuthService, private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot) {
        const allowed: string[] = route.data['roles'] ?? [];
        return this.auth.getMe().pipe(
            map((u: UserInfo | null) => !!u && allowed.includes(u.role)),
            tap(ok => { if (!ok) this.router.navigate(['/']); })
        );
    }
}
