import { Component, OnInit, computed, inject, signal, ChangeDetectorRef } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { CartService } from '../../services/cart.service';
import { Product } from '../../models/product.model';
import { ProductCardComponent } from '../../components/product-card/product-card';
import { SiteConfigService } from '../../config/site-config.service';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [ProductCardComponent],
  templateUrl: './products.html',
  styleUrl: './products.scss',
})
export class ProductsComponent implements OnInit {
  readonly site = inject(SiteConfigService);
  private readonly productService = inject(ProductService);
  private readonly cartService = inject(CartService);
  private readonly cdr = inject(ChangeDetectorRef);

  products = signal<Product[]>([]);
  loading = signal(true);
  error = signal(false);
  addedToCartId = signal<number | null>(null);
  selectedCategory = signal<string | null>(null);

  categories = computed(() => {
    const names = this.products()
      .map((p) => p.categoryName)
      .filter((n): n is string => !!n);
    return [...new Set(names)];
  });

  filtered = computed(() => {
    const category = this.selectedCategory();
    return category ? this.products().filter((p) => p.categoryName === category) : this.products();
  });

  ngOnInit(): void {
    this.productService.getProducts().subscribe({
      next: (products) => {
        this.products.set(products);
        this.loading.set(false);
        this.cdr.detectChanges();
      },
      error: () => {
        this.error.set(true);
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
