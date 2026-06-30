import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-page-error',
  templateUrl: './page-error.component.html',
  styleUrl: './page-error.component.scss'
})
export class PageErrorComponent {
  constructor(private router: Router) {}

  GoToHome() {
    this.router.navigate(['/dashboard']);
  }
}
