const API_BASE = "http://localhost:5274/api";

export const USER_ID = "19d5f1ef-8642-4083-961e-f63b32778ab9";

// ── Products ──────────────────────────────────────────────

import type { Product, ProductDto, DetectResult, Buy, BuyDto, User } from "../types";

export async function getProducts(): Promise<Product[]> {
  const res = await fetch(`${API_BASE}/user/${USER_ID}/list/products`);
  if (!res.ok) throw new Error("Error al obtener productos");
  return res.json();
}

export async function getProduct(productId: string): Promise<Product> {
  const res = await fetch(`${API_BASE}/user/${USER_ID}/list/products/${productId}`);
  if (!res.ok) throw new Error("Producto no encontrado");
  return res.json();
}

export async function addProduct(dto: ProductDto): Promise<Product> {
  const res = await fetch(`${API_BASE}/user/${USER_ID}/list/products`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(dto),
  });
  if (!res.ok) throw new Error("Error al agregar producto");
  return res.json();
}

export async function updateProduct(productId: string, dto: ProductDto): Promise<Product> {
  const res = await fetch(`${API_BASE}/user/${USER_ID}/list/products/${productId}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(dto),
  });
  if (!res.ok) throw new Error("Error al actualizar producto");
  return res.json();
}

export async function deleteProduct(productId: string): Promise<void> {
  const res = await fetch(`${API_BASE}/user/${USER_ID}/list/products/${productId}`, {
    method: "DELETE",
  });
  if (!res.ok) throw new Error("Error al eliminar producto");
}

// ── Detect (AI) ───────────────────────────────────────────

export async function detectProduct(image: File): Promise<DetectResult> {
  const formData = new FormData();
  formData.append("image", image);
  const res = await fetch(`${API_BASE}/user/${USER_ID}/list/detect`, {
    method: "POST",
    body: formData,
  });
  if (!res.ok) throw new Error("Error al detectar producto");
  return res.json();
}

// ── Purchase ──────────────────────────────────────────────

export async function completePurchase(dto: BuyDto): Promise<Buy> {
  const res = await fetch(`${API_BASE}/user/${USER_ID}/list/complete`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(dto),
  });
  if (!res.ok) throw new Error("Error al finalizar compra");
  return res.json();
}

// ── User ──────────────────────────────────────────────────

export async function getUser(): Promise<User> {
  const res = await fetch(`${API_BASE}/user/${USER_ID}`);
  if (!res.ok) throw new Error("Usuario no encontrado");
  return res.json();
}
