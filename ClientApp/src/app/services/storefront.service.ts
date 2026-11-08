import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environment';

export interface BlogPost {
  id: number;
  slug: string;
  title: string;
  excerpt: string | null;
  content: string;
  imageUrl: string | null;
  language: string;
  publishedAt: string;
}

export interface WishlistItem {
  productId: number;
  name: string;
  slug: string;
  price: number;
  imageUrl: string | null;
  addedAt: string;
}

export interface Account {
  id: number;
  username: string;
  email: string;
  name: string;
  surname: string;
  country: string | null;
  city: string | null;
  address: string | null;
  postalCode: string | null;
  phoneNumber: string | null;
}

export interface ContactPayload {
  name: string;
  email: string;
  phone: string | null;
  message: string;
  consentGiven: boolean;
}

@Injectable({ providedIn: 'root' })
export class StorefrontService {
  private readonly api = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getBlogPosts(lang: string): Observable<BlogPost[]> {
    return this.http.get<BlogPost[]>(`${this.api}/blog`, { params: { lang } });
  }

  getBlogPost(slug: string, lang: string): Observable<BlogPost> {
    return this.http.get<BlogPost>(`${this.api}/blog/${encodeURIComponent(slug)}`, {
      params: { lang },
    });
  }

  submitContact(payload: ContactPayload): Observable<unknown> {
    return this.http.post(`${this.api}/contact`, payload);
  }

  getWishlist(): Observable<WishlistItem[]> {
    return this.http.get<WishlistItem[]>(`${this.api}/wishlist`);
  }

  addToWishlist(productId: number): Observable<unknown> {
    return this.http.post(`${this.api}/wishlist/${productId}`, {});
  }

  removeFromWishlist(productId: number): Observable<unknown> {
    return this.http.delete(`${this.api}/wishlist/${productId}`);
  }

  getAccount(): Observable<Account> {
    return this.http.get<Account>(`${this.api}/account/me`);
  }

  updateAccount(account: Omit<Account, 'id' | 'username'>): Observable<Account> {
    return this.http.put<Account>(`${this.api}/account/me`, account);
  }

  changePassword(currentPassword: string, newPassword: string): Observable<unknown> {
    return this.http.post(`${this.api}/account/change-password`, { currentPassword, newPassword });
  }
}
