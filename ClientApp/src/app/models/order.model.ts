export interface CreateOrderRequest {
  price: number;
  customerName: string;
  customerEmail: string;
  customerAddress: string;
  orderItems: { productId: number; quantity: number; unitPrice: number }[];
  userId: number | null;
}

