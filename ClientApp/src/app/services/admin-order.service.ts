import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environment';

export interface AdminOrder {
  id: number;
  orderDate: string;
  totalPrice: number;
  customerName: string;
  customerEmail: string;
  customerAddress: string;
  status: number;
  userId: number | null;
  orderItems: { productId: number; quantity: number; unitPrice: number }[];
}

export const ORDER_STATUSES = [
  { value: 0, label: 'In Progress' },
  { value: 1, label: 'Canceled' },
  { value: 2, label: 'Rejected' },
  { value: 3, label: 'Shipped' },
  { value: 4, label: 'Success' },
];

@Injectable({ providedIn: 'root' })
export class AdminOrderService {
  private readonly apiUrl = `${environment.apiUrl}/orders`;

  constructor(private http: HttpClient) {}

  getOrders(): Observable<AdminOrder[]> {
    return this.http.get<AdminOrder[]>(this.apiUrl);
  }

  updateOrder(id: number, data: Partial<AdminOrder>): Observable<AdminOrder> {
    return this.http.put<AdminOrder>(`${environment.apiUrl}/order/${id}`, data);
  }

  deleteOrder(id: number): Observable<unknown> {
    return this.http.delete(`${environment.apiUrl}/deleteOrder/${id}`);
  }
}

