import { Component, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { ProductService } from '../../../services/product.service';
import { Product } from '../../../models/product.model';

@Component({
  selector: 'app-admin-products',
  standalone: true,
  imports: [CommonModule, FormsModule, CurrencyPipe, RouterLink, RouterLinkActive],
  templateUrl: './admin-products.html',
  styleUrl: './admin-products.scss',
})
export class AdminProductsComponent implements OnInit {
  products: Product[] = [];
  loading = true;
  saving = false;
  error: string | null = null;
  successMessage: string | null = null;

  editingProduct: Partial<Product> | null = null;
  isNew = false;

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading = true;
    this.productService.getProducts().subscribe({
      next: (products) => {
        this.products = products;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load products.';
        this.loading = false;
      },
    });
  }

  openNew(): void {
    this.editingProduct = {
      name: '',
      slug: '',
      description: '',
      price: 0,
      imageUrl: '',
      sku: '',
      stockQuantity: 0,
      categoryId: null,
      productTypeId: null,
    };
    this.isNew = true;
    this.error = null;
  }

  openEdit(p: Product): void {
    this.editingProduct = { ...p };
    this.isNew = false;
    this.error = null;
  }

  cancelEdit(): void {
    this.editingProduct = null;
  }

  save(): void {
    if (!this.editingProduct) return;
    this.saving = true;
    this.error = null;

    const obs = this.isNew
      ? this.productService.addProduct(this.editingProduct as Omit<Product, 'id'>)
      : this.productService.updateProduct(
          this.editingProduct.id!,
          this.editingProduct as Partial<Omit<Product, 'id'>>,
        );

    obs.subscribe({
      next: () => {
        this.editingProduct = null;
        this.saving = false;
        this.showSuccess(this.isNew ? 'Product added!' : 'Product updated!');
        this.loadProducts();
      },
      error: () => {
        this.error = 'Failed to save product.';
        this.saving = false;
      },
    });
  }

  delete(id: number): void {
    if (!confirm('Are you sure you want to delete this product?')) return;
    this.productService.deleteProduct(id).subscribe({
      next: () => {
        this.showSuccess('Product deleted.');
        this.loadProducts();
      },
      error: () => {
        this.error = 'Failed to delete product.';
      },
    });
  }

  private showSuccess(msg: string): void {
    this.successMessage = msg;
    setTimeout(() => (this.successMessage = null), 3000);
  }
}
