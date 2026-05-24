import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { CartItem } from '../models/cart-item.model';
import { Product } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class CartService {
  private cartSubject = new BehaviorSubject<CartItem[]>(this.loadCart());
  cart$ = this.cartSubject.asObservable();
  itemCount$ = this.cart$.pipe(map((items) => items.reduce((total, i) => total + i.quantity, 0)));

  private loadCart(): CartItem[] {
    try {
      const saved = localStorage.getItem('cart');
      return saved ? JSON.parse(saved) : [];
    } catch {
      return [];
    }
  }

  private saveCart(items: CartItem[]): void {
    localStorage.setItem('cart', JSON.stringify(items));
    this.cartSubject.next(items);
  }

  addToCart(product: Product): void {
    const items = [...this.cartSubject.value];
    const existing = items.find((i) => i.product.id === product.id);
    if (existing) {
      existing.quantity++;
    } else {
      items.push({ product, quantity: 1 });
    }
    this.saveCart(items);
  }

  removeFromCart(productId: number): void {
    this.saveCart(this.cartSubject.value.filter((i) => i.product.id !== productId));
  }

  updateQuantity(productId: number, quantity: number): void {
    if (quantity < 1) {
      this.removeFromCart(productId);
      return;
    }
    const items = this.cartSubject.value.map((i) =>
      i.product.id === productId ? { ...i, quantity } : i,
    );
    this.saveCart(items);
  }

  clearCart(): void {
    this.saveCart([]);
  }

  getTotal(): number {
    return this.cartSubject.value.reduce((total, i) => total + i.product.price * i.quantity, 0);
  }

  getItems(): CartItem[] {
    return this.cartSubject.value;
  }
}

