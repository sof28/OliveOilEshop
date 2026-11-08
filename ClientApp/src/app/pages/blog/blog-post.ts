import { Component, OnInit, PLATFORM_ID, inject, signal } from '@angular/core';
import { DatePipe, isPlatformBrowser } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { SiteConfigService } from '../../config/site-config.service';
import { BlogPost, StorefrontService } from '../../services/storefront.service';

@Component({
  selector: 'app-blog-post',
  standalone: true,
  imports: [RouterLink, DatePipe],
  template: `
    <article class="page">
      <a routerLink="/blog">&larr; {{ site.text('blogTitle', 'Blog') }}</a>
      @if (post(); as p) {
        <h1>{{ p.title }}</h1>
        <small>{{ p.publishedAt | date: 'mediumDate' }}</small>
        @if (p.imageUrl) {
          <img [src]="p.imageUrl" [alt]="p.title" />
        }
        <div class="body">{{ p.content }}</div>
      } @else if (notFound()) {
        <p>{{ site.text('postNotFound', 'Post not found.') }}</p>
      } @else {
        <p>{{ site.label('loading') }}</p>
      }
    </article>
  `,
  styles: `
    .page { max-width: 760px; margin: 2rem auto; padding: 0 1rem; }
    img { width: 100%; border-radius: 8px; margin: 1rem 0; }
    .body { white-space: pre-line; line-height: 1.7; }
    a { color: var(--color-primary); }
  `,
})
export class BlogPostComponent implements OnInit {
  readonly site = inject(SiteConfigService);
  private readonly api = inject(StorefrontService);
  private readonly route = inject(ActivatedRoute);
  private readonly isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  post = signal<BlogPost | null>(null);
  notFound = signal(false);

  ngOnInit(): void {
    if (!this.isBrowser) return;
    const slug = this.route.snapshot.paramMap.get('slug') ?? '';
    this.api.getBlogPost(slug, this.site.lang()).subscribe({
      next: (p) => this.post.set(p),
      error: () => this.notFound.set(true),
    });
  }
}
