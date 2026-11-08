export interface Product {
  id: number;
  name: string;
  slug: string;
  description: string | null;
  price: number;
  imageUrl: string | null;
  sku: string | null;
  stockQuantity: number;
  categoryId: number | null;
  categoryName?: string | null;
  productTypeId: number | null;
  productTypeName?: string | null;
}
