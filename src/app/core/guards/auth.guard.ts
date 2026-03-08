import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { AuthService } from "../services/auth.service";
import { tap } from "rxjs/operators";

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(private auth: AuthService, private router: Router) { }

    canActivate() {
        return this.auth.isAuthenticated().pipe(
            tap(ok => { if (!ok) this.router.navigate(['/login']); })
        );
    }
}
