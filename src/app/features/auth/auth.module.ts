import { NgModule } from '@angular/core';
import { LoginComponent } from './login/login.component';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router'

@NgModule({
  imports: [
    CommonMoudle,
    RouterModule.forChild([
      {path: '', component: LoginComponent}
    ])
  ]
})

  export class AuthModule {}
