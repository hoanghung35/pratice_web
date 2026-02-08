import { Component } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

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

  //private readonly auth:AuthService
  constructor(private auth: AuthService, private router: Router, private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      console.log('Dữ liệu đăng nhập:', this.loginForm.value);
      alert('Đăng nhập thành công (giả lập)!');
    }
  }

  login() {
    this.auth.login(this.username, this.password)
      .subscribe({
        next: () => this.router.navigate(['/items']),
        error: () => console.log('Login failed')
      });
  }
}
