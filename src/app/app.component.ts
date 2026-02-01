import { Component } from '@angular/core';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'FE_Web';

  constructor(private auth: AuthService) { }

  ngOnInit() {
    this.auth.getMe(true).subscribe();
  }
}
