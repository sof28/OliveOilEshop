import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { CartService } from '../../services/cart.service';
import { Product } from '../../models/product.model';
import { SiteConfigService } from '../../config/site-config.service';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [RouterLink, CurrencyPipe],
  templateUrl: './product-detail.html',
  styleUrl: './product-detail.scss',
})
export class ProductDetailComponent implements OnInit {
  readonly site = inject(SiteConfigService);
  private readonly route = inject(ActivatedRoute);
  private readonly productService = inject(ProductService);
  private readonly cartService = inject(CartService);

  readonly product = signal<Product | null>(null);
  readonly loading = signal(true);
  readonly added = signal(false);
  readonly inStock = computed(() => (this.product()?.stockQuantity ?? 0) > 0);

  ngOnInit(): void {
    const key = this.route.snapshot.paramMap.get('slug') ?? '';
    this.productService.getProducts().subscribe({
      next: (products) => {
        this.product.set(products.find((p) => p.slug === key || String(p.id) === key) ?? null);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }

  addToCart(): void {
    const product = this.product();
    if (!product) {
      return;
    }
    this.cartService.addToCart(product);
    this.added.set(true);
    setTimeout(() => this.added.set(false), 2000);
  }
}
