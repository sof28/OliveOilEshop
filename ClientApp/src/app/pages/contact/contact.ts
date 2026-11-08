import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SiteConfigService } from '../../config/site-config.service';
import { StorefrontService } from '../../services/storefront.service';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [FormsModule],
  template: `
    <section class="page">
      <h1>{{ site.text('contactTitle', 'Contact') }}</h1>
      @if (status() === 'sent') {
        <p class="ok">{{ site.text('contactSent', 'Thank you! Your message was sent.') }}</p>
      } @else {
        <form #f="ngForm" (ngSubmit)="submit(f.valid === true)">
          <input name="name" [(ngModel)]="name" required [placeholder]="site.text('fieldName', 'Name')" />
          <input name="email" type="email" [(ngModel)]="email" required [placeholder]="site.text('fieldEmail', 'Email')" />
          <input name="phone" [(ngModel)]="phone" [placeholder]="site.text('fieldPhone', 'Phone')" />
          <textarea name="message" rows="5" [(ngModel)]="message" required [placeholder]="site.text('fieldMessage', 'Message')"></textarea>
          <label class="consent">
            <input type="checkbox" name="consent" [(ngModel)]="consent" required />
            {{ site.text('contactConsent', 'I agree to the processing of my data.') }}
          </label>
          @if (status() === 'error') {
            <p class="err">{{ site.text('contactError', 'Something went wrong. Please try again.') }}</p>
          }
          <button type="submit" [disabled]="status() === 'sending'">
            {{ site.text('send', 'Send') }}
          </button>
        </form>
      }
    </section>
  `,
  styles: `
    .page { max-width: 560px; margin: 2rem auto; padding: 0 1rem; }
    form { display: flex; flex-direction: column; gap: 0.75rem; }
    input, textarea { padding: 0.7rem; border: 1px solid var(--color-muted); border-radius: 6px; font: inherit; }
    .consent { display: flex; gap: 0.5rem; align-items: center; }
    .consent input { width: auto; }
    button { padding: 0.8rem; background: var(--color-primary); color: #fff; border: 0; border-radius: 6px; cursor: pointer; }
    .err { color: #b00020; }
    .ok { color: var(--color-primary); }
  `,
})
export class ContactComponent {
  readonly site = inject(SiteConfigService);
  private readonly api = inject(StorefrontService);

  name = '';
  email = '';
  phone = '';
  message = '';
  consent = false;
  status = signal<'idle' | 'sending' | 'sent' | 'error'>('idle');

  submit(valid: boolean): void {
    if (!valid || !this.consent) return;
    this.status.set('sending');
    this.api
      .submitContact({
        name: this.name,
        email: this.email,
        phone: this.phone || null,
        message: this.message,
        consentGiven: this.consent,
      })
      .subscribe({
        next: () => this.status.set('sent'),
        error: () => this.status.set('error'),
      });
  }
}
