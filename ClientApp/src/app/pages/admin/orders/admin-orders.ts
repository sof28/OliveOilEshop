import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AdminOrderService, AdminOrder, ORDER_STATUSES } from '../../../services/admin-order.service';

@Component({
  selector: 'app-admin-orders',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './admin-orders.html',
  styleUrl: './admin-orders.scss',
})
export class AdminOrdersComponent implements OnInit {
  orders: AdminOrder[] = [];
  loading = true;
  error: string | null = null;
  successMessage: string | null = null;

  readonly statuses = ORDER_STATUSES;

  editingOrderId: number | null = null;
  editingStatus: number = 0;

  constructor(private adminOrderService: AdminOrderService) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.loading = true;
    this.adminOrderService.getOrders().subscribe({
      next: (orders) => {
        this.orders = orders;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load orders.';
        this.loading = false;
      },
    });
  }

  startEditStatus(order: AdminOrder): void {
    this.editingOrderId = order.id;
    this.editingStatus = order.status;
  }

  cancelEdit(): void {
    this.editingOrderId = null;
  }

  saveStatus(order: AdminOrder): void {
    this.adminOrderService
      .updateOrder(order.id, {
        ...order,
        status: this.editingStatus,
      })
      .subscribe({
        next: () => {
          order.status = this.editingStatus;
          this.editingOrderId = null;
          this.showSuccess('Order status updated.');
        },
        error: () => {
          this.error = 'Failed to update order.';
        },
      });
  }

  delete(id: number): void {
    if (!confirm('Delete this order?')) return;
    this.adminOrderService.deleteOrder(id).subscribe({
      next: () => {
        this.showSuccess('Order deleted.');
        this.loadOrders();
      },
      error: () => {
        this.error = 'Failed to delete order.';
      },
    });
  }

  getStatusLabel(value: number): string {
    return this.statuses.find((s) => s.value === value)?.label ?? 'Unknown';
  }

  getStatusClass(value: number): string {
    const map: Record<number, string> = {
      0: 'status-inprogress',
      1: 'status-canceled',
      2: 'status-rejected',
      3: 'status-shipped',
      4: 'status-success',
    };
    return map[value] ?? '';
  }

  private showSuccess(msg: string): void {
    this.successMessage = msg;
    setTimeout(() => (this.successMessage = null), 3000);
  }
}

