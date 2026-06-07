import { Routes } from '@angular/router';
import { adminGuard } from './guards/admin.guard';

export const routes: Routes = [
  { path: '', loadComponent: () => import('./pages/home/home').then((m) => m.HomeComponent) },
  { path: 'about', loadComponent: () => import('./pages/about/about').then((m) => m.AboutComponent) },
  { path: 'products', loadComponent: () => import('./pages/products/products').then((m) => m.ProductsComponent) },
  { path: 'cart', loadComponent: () => import('./pages/cart/cart').then((m) => m.CartComponent) },
  { path: 'login', loadComponent: () => import('./pages/login/login').then((m) => m.LoginComponent) },
  {
    path: 'admin',
    canActivate: [adminGuard],
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./pages/admin/dashboard/dashboard').then((m) => m.AdminDashboardComponent),
      },
      {
        path: 'products',
        loadComponent: () =>
          import('./pages/admin/products/admin-products').then((m) => m.AdminProductsComponent),
      },
      {
        path: 'orders',
        loadComponent: () =>
          import('./pages/admin/orders/admin-orders').then((m) => m.AdminOrdersComponent),
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
