import { Component, OnInit, signal, ChangeDetectorRef } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { CartService } from '../../services/cart.service';
import { Product } from '../../models/product.model';
import { ProductCardComponent } from '../../components/product-card/product-card';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [ProductCardComponent],
  templateUrl: './products.html',
  styleUrl: './products.scss',
})
export class ProductsComponent implements OnInit {
  products = signal<Product[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);
  addedToCartId = signal<number | null>(null);

  constructor(
    private productService: ProductService,
    private cartService: CartService,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.productService.getProducts().subscribe({
      next: (products) => {
        this.products.set(products);
        this.loading.set(false);
        this.cdr.detectChanges();
      },
      error: () => {
        this.error.set('Failed to load products. Please try again.');
        this.loading.set(false);
        this.cdr.detectChanges();
      },
    });
  }

  onAddToCart(product: Product): void {
    this.cartService.addToCart(product);
    this.addedToCartId.set(product.id);
    setTimeout(() => this.addedToCartId.set(null), 2000);
  }
}



