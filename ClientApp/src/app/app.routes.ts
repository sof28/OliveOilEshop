import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', loadComponent: () => import('./pages/home/home').then((m) => m.HomeComponent) },
  { path: 'about', loadComponent: () => import('./pages/about/about').then((m) => m.AboutComponent) },
  { path: 'products', loadComponent: () => import('./pages/products/products').then((m) => m.ProductsComponent) },
  { path: 'cart', loadComponent: () => import('./pages/cart/cart').then((m) => m.CartComponent) },
  { path: '**', redirectTo: '' },
];
