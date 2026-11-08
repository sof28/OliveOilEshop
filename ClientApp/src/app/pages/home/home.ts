import { Component, OnInit, PLATFORM_ID, inject, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { RouterLink } from '@angular/router';
import { SiteConfigService } from '../../config/site-config.service';
import { ProductService } from '../../services/product.service';
import { CartService } from '../../services/cart.service';
import { Product } from '../../models/product.model';
import { ProductCardComponent } from '../../components/product-card/product-card';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink, ProductCardComponent],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class HomeComponent implements OnInit {
  readonly site = inject(SiteConfigService);
  private readonly productService = inject(ProductService);
  private readonly cartService = inject(CartService);
  private readonly isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  readonly featured = signal<Product[]>([]);

  ngOnInit(): void {
    if (!this.isBrowser) {
      return;
    }
    this.productService.getProducts().subscribe({
      next: (products) =>
        this.featured.set(products.slice(0, this.site.config.featuredProducts.count)),
      error: () => this.featured.set([]),
    });
  }

  onAddToCart(product: Product): void {
    this.cartService.addToCart(product);
  }
}
