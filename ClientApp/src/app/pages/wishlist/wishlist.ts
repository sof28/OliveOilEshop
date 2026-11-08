import { Component, OnInit, PLATFORM_ID, inject, signal } from '@angular/core';
import { CurrencyPipe, isPlatformBrowser } from '@angular/common';
import { RouterLink } from '@angular/router';
import { SiteConfigService } from '../../config/site-config.service';
import { AuthService } from '../../services/auth.service';
import { StorefrontService, WishlistItem } from '../../services/storefront.service';

@Component({
  selector: 'app-wishlist',
  standalone: true,
  imports: [RouterLink, CurrencyPipe],
  template: `
    <section class="page">
      <h1>{{ site.text('wishlistTitle', 'Wishlist') }}</h1>
      @if (!auth.isLoggedIn()) {
        <p>
          <a routerLink="/login">{{ site.text('loginToUseWishlist', 'Log in to use your wishlist.') }}</a>
        </p>
      } @else if (items().length === 0) {
        <p>{{ site.text('wishlistEmpty', 'Your wishlist is empty.') }}</p>
      } @else {
        <ul>
          @for (i of items(); track i.productId) {
            <li>
              @if (i.imageUrl) {
                <img [src]="i.imageUrl" [alt]="i.name" />
              }
              <a [routerLink]="['/products', i.slug]">{{ i.name }}</a>
              <span>{{ i.price | currency: 'EUR' }}</span>
              <button (click)="remove(i.productId)">{{ site.text('remove', 'Remove') }}</button>
            </li>
          }
        </ul>
      }
    </section>
  `,
  styles: `
    .page { max-width: 800px; margin: 2rem auto; padding: 0 1rem; }
    ul { list-style: none; padding: 0; }
    li { display: flex; align-items: center; gap: 1rem; padding: 0.75rem 0; border-bottom: 1px solid var(--color-surface-alt); }
    li img { width: 56px; height: 56px; object-fit: cover; border-radius: 6px; }
    li a { flex: 1; color: var(--color-primary); }
    button { background: none; border: 1px solid var(--color-muted); border-radius: 6px; padding: 0.4rem 0.8rem; cursor: pointer; }
  `,
})
export class WishlistComponent implements OnInit {
  readonly site = inject(SiteConfigService);
  readonly auth = inject(AuthService);
  private readonly api = inject(StorefrontService);
  private readonly isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  items = signal<WishlistItem[]>([]);

  ngOnInit(): void {
    if (!this.isBrowser || !this.auth.isLoggedIn()) return;
    this.api.getWishlist().subscribe((i) => this.items.set(i));
  }

  remove(productId: number): void {
    this.api
      .removeFromWishlist(productId)
      .subscribe(() => this.items.update((l) => l.filter((x) => x.productId !== productId)));
  }
}
