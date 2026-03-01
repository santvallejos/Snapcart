export interface Product {
  id: string;
  name: string;
  brand?: string;
  quantity?: number;
  price?: number;
  category?: string;
  isInCart: boolean;
}

export interface ProductDto {
  name: string;
  brand?: string;
  quantity?: number;
  price?: number;
  category?: string;
}

export interface User {
  id: string;
  name: string;
  lastName: string;
  email: string;
  phone?: string;
  createdAt: string;
}

export interface Buy {
  id: string;
  userId: string;
  listId: string;
  purchaseCompletedAt: string;
  supermarketName?: string;
}

export interface BuyDto {
  purchaseCompletedAt: string;
  supermarketName?: string;
}

export interface DetectResult {
  matched: boolean;
  message?: string;
  product?: Product;
}
