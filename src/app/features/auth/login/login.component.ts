import { Component, OnInit } from '@angular/core';
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
export class LoginComponent implements OnInit {

  loginForm!: FormGroup;
  user: Observable<string | null>
    //private readonly auth:AuthService
    | undefined

  constructor(private auth: AuthService, private router: Router, private fb: FormBuilder) {

  }

  ngOnInit() {
    this.loginForm = this.fb.group({
      usercode: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(3)]]
    });
  }

  login() {
    const { usercode, password } = this.loginForm.value;

    this.auth.login(usercode, password)
      .subscribe({
        next: () => this.auth.getMe().subscribe(res => {
          console.log(res);
          this.router.navigate(['/items'])
        }),
        error: () => console.log('Login failed')
      });
  }
}
