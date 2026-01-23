import { Component } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  username: string = "";
  password: string = ""
  //private readonly auth:AuthService
  constructor(private auth: AuthService, private router: Router) { }
  login() {
    this.auth.login(this.username, this.password)
      .subscribe({
        next: () => this.router.navigate(['/items']),
        error: () => console.log('Login failed')
      });
  }
}
