import { DOCUMENT } from '@angular/common';
import { Injectable, inject, signal } from '@angular/core';
import { environment } from '../../environment';
import { resolveSite } from '../../sites';
import { Localized, SiteConfig } from './site-config.model';

@Injectable({ providedIn: 'root' })
export class SiteConfigService {
  private readonly document = inject(DOCUMENT);

  readonly config: SiteConfig = resolveSite(environment.siteId);
  readonly lang = signal<string>(this.config.defaultLang);

  constructor() {
    this.applyTheme();
    this.document.documentElement.lang = this.lang();
  }

  /** Returns the text for the active language, falling back to the default language. */
  t(value: Localized | undefined): string {
    if (!value) {
      return '';
    }
    return value[this.lang()] ?? value[this.config.defaultLang] ?? '';
  }

  label(key: string): string {
    return this.t(this.config.labels[key]);
  }

  /** Label from site config, or the given fallback when the site does not define it. */
  text(key: string, fallback: string): string {
    return this.label(key) || fallback;
  }

  setLang(lang: string): void {
    if (!this.config.languages.includes(lang)) {
      return;
    }
    this.lang.set(lang);
    this.document.documentElement.lang = lang;
  }

  private applyTheme(): void {
    const t = this.config.theme;
    const style = this.document.documentElement.style;
    style.setProperty('--color-primary', t.primary);
    style.setProperty('--color-primary-dark', t.primaryDark);
    style.setProperty('--color-accent', t.accent);
    style.setProperty('--color-background', t.background);
    style.setProperty('--color-surface', t.surface);
    style.setProperty('--color-surface-alt', t.surfaceAlt);
    style.setProperty('--color-text', t.text);
    style.setProperty('--color-muted', t.muted);
  }
}
