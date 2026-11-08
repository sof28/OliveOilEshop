export type Localized = Record<string, string>;

export interface Cta {
  label: Localized;
  route: string;
}

export interface SiteConfig {
  siteId: string;
  defaultLang: string;
  languages: string[];
  brand: {
    name: Localized;
    tagline: Localized;
    logo: string;
    icon: string;
  };
  theme: {
    primary: string;
    primaryDark: string;
    accent: string;
    background: string;
    surface: string;
    surfaceAlt: string;
    text: string;
    muted: string;
  };
  contact: {
    phone: string;
    email: string;
    address: Localized;
  };
  promoBar: {
    enabled: boolean;
    message: Localized;
    code: string;
  };
  nav: Cta[];
  hero: {
    badge: Localized;
    title: Localized;
    subtitle: Localized;
    image: string;
    primaryCta: Cta;
    secondaryCta: Cta;
  };
  story: {
    title: Localized;
    subtitle: Localized;
    body: Localized;
    image: string;
    cta: Cta;
  };
  features: { icon: string; title: Localized; text: Localized }[];
  featuredProducts: {
    title: Localized;
    subtitle: Localized;
    count: number;
    cta: Cta;
  };
  blogTeaser: {
    title: Localized;
    readMore: Localized;
    posts: { title: Localized; image: string; route: string }[];
  };
  contactSection: {
    title: Localized;
    text: Localized;
  };
  footer: {
    text: Localized;
  };
  labels: Record<string, Localized>;
  flags: {
    wishlist: boolean;
    blog: boolean;
    accounts: boolean;
    contactForm: boolean;
  };
}
