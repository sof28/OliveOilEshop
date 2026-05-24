import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AsyncPipe, CurrencyPipe } from '@angular/common';
import { CartService } from '../../services/cart.service';
import { OrderService } from '../../services/order.service';
import { CartItem } from '../../models/cart-item.model';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [RouterLink, FormsModule, AsyncPipe, CurrencyPipe],
  templateUrl: './cart.html',
  styleUrl: './cart.scss',
})
export class CartComponent {
  customerName = '';
  customerEmail = '';
  customerAddress = '';
  orderSuccess = false;
  orderError: string | null = null;
  submitting = false;

  constructor(
    public cartService: CartService,
    private orderService: OrderService,
  ) {}

  removeItem(productId: number): void {
    this.cartService.removeFromCart(productId);
  }

  updateQty(item: CartItem, qty: number): void {
    this.cartService.updateQuantity(item.product.id, qty);
  }

  submitOrder(): void {
    const items = this.cartService.getItems();
    if (items.length === 0) return;

    this.submitting = true;
    this.orderError = null;

    const payload = {
      price: this.cartService.getTotal(),
      customerName: this.customerName,
      customerEmail: this.customerEmail,
      customerAddress: this.customerAddress,
      orderItems: items.map((i) => ({
        productId: i.product.id,
        quantity: i.quantity,
        unitPrice: i.product.price,
      })),
      userId: null,
    };

    this.orderService.createOrder(payload).subscribe({
      next: () => {
        this.orderSuccess = true;
        this.cartService.clearCart();
        this.submitting = false;
      },
      error: () => {
        this.orderError = 'Failed to place order. Please try again.';
        this.submitting = false;
      },
    });
  }
}

