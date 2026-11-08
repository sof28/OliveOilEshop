# Storefront Template Spec

Derived from the structure of sofianosoliveoil.gr (WooCommerce, EL/EN). Only structure and behaviour are reused. Brand text, logos and images come from per-site config.

## Layout (all pages)
- Promo bar: message, optional coupon code, phone, email
- Header: logo, nav, language switcher, account link, wishlist counter, mini-cart (count + total)
- Footer: brand, nav links, contact info (to be verified against the reference site)

## Pages
| Route | Sections |
|-------|----------|
| `/` | hero, story, featured products, blog teaser, contact form |
| `/about` | story page |
| `/products` | product grid, category filter |
| `/products/:id` | detail, variants, attributes |
| `/cart` | cart lines, coupon, totals |
| `/login` | login, lost password |
| `/wishlist` | saved products |
| `/blog`, `/blog/:slug` | post list, post |
| `/contact` | contact form |
| `/admin/**` | existing admin |

## Product card
image, title, category, price, add to wishlist, add to cart.

## Contact form
name, email, phone, message, consent, captcha.

## Per-site config (`assets/sites/<siteId>/site.json`)
brand, theme (color tokens), images, contact, nav, promo bar, hero, story, features flags, localized strings (`el`, `en`).

## Not yet verified on the reference site
checkout fields, footer content, my-account sub pages, blog list layout.
