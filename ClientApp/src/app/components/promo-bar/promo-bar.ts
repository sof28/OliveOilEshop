import { Component, inject } from '@angular/core';
import { SiteConfigService } from '../../config/site-config.service';

@Component({
  selector: 'app-promo-bar',
  standalone: true,
  templateUrl: './promo-bar.html',
  styleUrl: './promo-bar.scss',
})
export class PromoBarComponent {
  readonly site = inject(SiteConfigService);
}
