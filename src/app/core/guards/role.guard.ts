import { Injectable } from "@angular/core";
import { AuthService } from "../services/auth.service";
import { CanActivate, Router } from "@angular/router";
import { ActivatedRouteSnapshot } from "@angular/router";

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
    constructor(private auth: AuthService, private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot): boolean {
        const roles = route.data['roles'] as string[];

        if (!roles.includes(this.auth.role!)) {
            this.router.navigate(['/403']);
            return false;
        }
        return true;
    }
}
