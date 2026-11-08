import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SiteConfigService } from '../../config/site-config.service';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './footer.html',
  styleUrl: './footer.scss',
})
export class FooterComponent {
  readonly site = inject(SiteConfigService);
  readonly year = new Date().getFullYear();
}
