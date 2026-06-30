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
        title: 'Homepage',
        path: 'dashboard',
        loadChildrem: () => import('./features/dashboard/dashboard.module).then(m => m.DashBoardModule)
      },
      {
        title: 'ITEM',
        path: 'items',
        loadChildren: () => import('./features/item/item.module').then(m =>  m.ItemModule)
      },
      {
        title: 'REQUEST',
        path: 'request',
        loadChildren: () => import('./features/request/request.module').then(m => m.RequestModule),
        canActive: [RoleGuard],
        data: { roles : ['admin', 'dev'] }
      },
      {
        title: 'APPROVE',
        path: 'approve',
        loadChildren: () => import('./features/approve//approve.module').then(m => m.ApproveModule),
        canActive: [RoleGuard],
        data: { roles: ['manager', 'gm', 'dev', 'super'] }
      },
      {
        title: 'ACCOUNT',
        path: 'account',
        loadChildren: () => import('./features/account/account.module').then(m => m.AccountMoudle),
        canActive: [RoleGuard],
        data: { roles: ['dev'] }
      },
      {
        title: 'INVENTORY',
        path: 'inventory',
        loadChildren: () => import('./features/inventory/inventory.module').then(m => m.InventModule)
      },
      {
        title: 'HISTORY',
        path: 'history',
        loadChildren: () => import('./features/history/history.module').then(m => m.HistoryModule)
      },
      {
        title: 'CURRENCY',
        path: 'currency',
        loadChildren: () => import('./features/currency/currency.module').then(m => m.CurrencyModule)
      },
      {
        path: '',
        redirecTo: './dashboard',
        pathMatch: 'full'
      }
    ]
  },
  { 
    path: 'login',
    component: LoginComponent,
    children: [
      { path: 'reset-password', component: ForgetPasswordComponent }
    ],
    title: 'LOGIN'
  },
  {
    title: 'ERROR',
    component: PageErrorComponent,
    path: '**'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
