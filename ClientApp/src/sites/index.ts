import { SiteConfig } from '../app/config/site-config.model';
import sofianos from './sofianos/site.json';
import demo from './demo/site.json';

const sites: Record<string, SiteConfig> = {
  sofianos,
  demo,
};

export function resolveSite(siteId: string): SiteConfig {
  const site = sites[siteId];
  if (!site) {
    throw new Error(`Unknown siteId "${siteId}". Register it in src/sites/index.ts.`);
  }
  return site;
}
