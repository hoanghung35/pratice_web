import { Injectable } from "@angular/core";
import { AuthService } from "../../core/services/auth.service";
import { Router } from "@angular/router";



@Injectable({ providedIn: 'root' })
export class AuthModule {
    constructor(private auth: AuthService, private router: Router) { }

    logout() {
        this.auth.logout().subscribe(() => {
            this.router.navigate(['/login']);
        });

    }
}