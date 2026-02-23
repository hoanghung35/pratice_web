import { Component } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  username: string = "";
  password: string = ""
  loginForm: FormGroup;
  user: Observable<string | null>
    //private readonly auth:AuthService
    | undefined

  //private readonly auth:AuthService
  constructor(private auth: AuthService, private router: Router, private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit() {

  }

  login() {
    this.auth.login(this.username, this.password)
      .subscribe({
        next: () => this.auth.getMe().subscribe(user => {
          this.user = user;
          this.router.navigate(['/items'])
        }),
        error: () => console.log('Login failed')
      });
  }
}
