import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { RoleGuard } from './core/guards/role.guard';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { LoginComponent } from './features/auth/login/login.component';

const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'items',
        loadChildren: () => import('./features/item/item.module')
          .then(m => m.ItemModule)
      },
      {
        path: 'approve',
        loadChildren: () => import('./features/approve/approve.module')
          .then(m => m.ApproveModule),
        canActivate: [RoleGuard],
        data: { roles: ['admin'] }
      },
      {
        path: 'orders',
        loadChildren: () => import('./features/order/order.module')
          .then(m => m.OrderModule)
      }
    ]
  },
  { path: 'login', component: LoginComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
