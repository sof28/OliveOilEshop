import { Component, OnInit, PLATFORM_ID, inject, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { SiteConfigService } from '../../config/site-config.service';
import { AuthService } from '../../services/auth.service';
import { Account, StorefrontService } from '../../services/storefront.service';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [FormsModule, RouterLink],
  template: `
    <section class="page">
      <h1>{{ site.text('accountTitle', 'My account') }}</h1>
      @if (!auth.isLoggedIn()) {
        <p><a routerLink="/login">{{ site.text('loginToUseAccount', 'Log in to see your account.') }}</a></p>
      } @else if (account(); as a) {
        <form (ngSubmit)="save()">
          <input name="name" [(ngModel)]="a.name" required [placeholder]="site.text('fieldName', 'Name')" />
          <input name="surname" [(ngModel)]="a.surname" required [placeholder]="site.text('fieldSurname', 'Surname')" />
          <input name="email" type="email" [(ngModel)]="a.email" required [placeholder]="site.text('fieldEmail', 'Email')" />
          <input name="phone" [(ngModel)]="a.phoneNumber" [placeholder]="site.text('fieldPhone', 'Phone')" />
          <input name="country" [(ngModel)]="a.country" [placeholder]="site.text('fieldCountry', 'Country')" />
          <input name="city" [(ngModel)]="a.city" [placeholder]="site.text('fieldCity', 'City')" />
          <input name="address" [(ngModel)]="a.address" [placeholder]="site.text('fieldAddress', 'Address')" />
          <input name="postal" [(ngModel)]="a.postalCode" [placeholder]="site.text('fieldPostalCode', 'Postal code')" />
          <button type="submit">{{ site.text('save', 'Save') }}</button>
          @if (message()) {
            <p>{{ message() }}</p>
          }
        </form>

        <h2>{{ site.text('changePassword', 'Change password') }}</h2>
        <form (ngSubmit)="changePassword()">
          <input name="cur" type="password" [(ngModel)]="currentPassword" required [placeholder]="site.text('currentPassword', 'Current password')" />
          <input name="new" type="password" [(ngModel)]="newPassword" required minlength="8" [placeholder]="site.text('newPassword', 'New password (min 8 characters)')" />
          <button type="submit">{{ site.text('changePassword', 'Change password') }}</button>
          @if (passwordMessage()) {
            <p>{{ passwordMessage() }}</p>
          }
        </form>
      } @else {
        <p>{{ site.label('loading') }}</p>
      }
    </section>
  `,
  styles: `
    .page { max-width: 520px; margin: 2rem auto; padding: 0 1rem; }
    form { display: flex; flex-direction: column; gap: 0.75rem; margin-bottom: 2rem; }
    input { padding: 0.7rem; border: 1px solid var(--color-muted); border-radius: 6px; font: inherit; }
    button { padding: 0.8rem; background: var(--color-primary); color: #fff; border: 0; border-radius: 6px; cursor: pointer; }
    a { color: var(--color-primary); }
  `,
})
export class AccountComponent implements OnInit {
  readonly site = inject(SiteConfigService);
  readonly auth = inject(AuthService);
  private readonly api = inject(StorefrontService);
  private readonly isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  account = signal<Account | null>(null);
  message = signal('');
  passwordMessage = signal('');
  currentPassword = '';
  newPassword = '';

  ngOnInit(): void {
    if (!this.isBrowser || !this.auth.isLoggedIn()) return;
    this.api.getAccount().subscribe({
      next: (a) => this.account.set(a),
      error: () => this.auth.logout(),
    });
  }

  save(): void {
    const a = this.account();
    if (!a) return;
    const { id, username, ...payload } = a;
    this.api.updateAccount(payload).subscribe({
      next: (updated) => {
        this.account.set(updated);
        this.message.set(this.site.text('saved', 'Saved.'));
      },
      error: () => this.message.set(this.site.text('saveFailed', 'Could not save changes.')),
    });
  }

  changePassword(): void {
    if (this.newPassword.length < 8) return;
    this.api.changePassword(this.currentPassword, this.newPassword).subscribe({
      next: () => {
        this.currentPassword = '';
        this.newPassword = '';
        this.passwordMessage.set(this.site.text('passwordChanged', 'Password changed.'));
      },
      error: () =>
        this.passwordMessage.set(this.site.text('passwordFailed', 'Could not change password.')),
    });
  }
}
