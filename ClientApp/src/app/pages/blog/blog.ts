import { Component, OnInit, PLATFORM_ID, inject, signal } from '@angular/core';
import { DatePipe, isPlatformBrowser } from '@angular/common';
import { RouterLink } from '@angular/router';
import { SiteConfigService } from '../../config/site-config.service';
import { BlogPost, StorefrontService } from '../../services/storefront.service';

@Component({
  selector: 'app-blog',
  standalone: true,
  imports: [RouterLink, DatePipe],
  template: `
    <section class="page">
      <h1>{{ site.text('blogTitle', 'Blog') }}</h1>
      @if (loading()) {
        <p>{{ site.label('loading') }}</p>
      } @else if (posts().length === 0) {
        <p>{{ site.text('blogEmpty', 'No posts yet.') }}</p>
      } @else {
        <div class="grid">
          @for (p of posts(); track p.id) {
            <article class="card">
              @if (p.imageUrl) {
                <img [src]="p.imageUrl" [alt]="p.title" />
              }
              <h2>{{ p.title }}</h2>
              <small>{{ p.publishedAt | date: 'mediumDate' }}</small>
              <p>{{ p.excerpt }}</p>
              <a [routerLink]="['/blog', p.slug]">{{ site.text('readMore', 'Read more') }}</a>
            </article>
          }
        </div>
      }
    </section>
  `,
  styles: `
    .page { max-width: 1100px; margin: 2rem auto; padding: 0 1rem; }
    .grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: 1.5rem; }
    .card { background: var(--color-surface); padding: 1rem; border-radius: 8px; }
    .card img { width: 100%; border-radius: 6px; }
    a { color: var(--color-primary); }
  `,
})
export class BlogComponent implements OnInit {
  readonly site = inject(SiteConfigService);
  private readonly api = inject(StorefrontService);
  private readonly isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  posts = signal<BlogPost[]>([]);
  loading = signal(true);

  ngOnInit(): void {
    if (!this.isBrowser) return;
    this.api.getBlogPosts(this.site.lang()).subscribe({
      next: (p) => {
        this.posts.set(p);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }
}
