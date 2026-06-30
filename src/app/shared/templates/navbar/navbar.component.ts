import { Component, inject,  OnInit } from '@angular/core';


@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  private breakpointObserver = inject(BreakpointObserver);
  toggleDraw: boolean[]  = [false];
  userName: string = 'Admin';
  userRole: string = '';
  email: string= '';

  constructor(
    private authService: AuthService,
    private router: Router,
    private dialog: MatDialog
  ){ }

  ngOnInit(): void {
    this.userName = this.authService.name != null ? this.authService.name : 'Admin';
    this.userRole = this.authService.role ?? '';
    this.email = this.authService.email ?? '';
  }

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Beakpoints.Handset).pipe(
    map(res => res.matches),
    shareReplay()
  );

  showProfile() {
    this.dialog.open(EditProfileComponent, {
      maxWidth: '80rem',
      width: '40rem',
      panelClass: 'profile-container'
    });
  }

  logout() {
    this.authService.logout().subscibe({
      next: () => this.router.navigate(['/login']),
      error: () => console.log('Logout failed')
    });
  }
}
