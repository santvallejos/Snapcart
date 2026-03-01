import { useEffect, useState, useCallback } from "react";
import { Plus, ShoppingBasket } from "lucide-react";
import type { Product, ProductDto } from "../types";
import { getProducts, addProduct, updateProduct, deleteProduct } from "../api/client";
import ProductItem from "../components/ProductItem";
import ProductForm from "../components/ProductForm";

export default function ListPage() {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [showForm, setShowForm] = useState(false);
  const [editingProduct, setEditingProduct] = useState<Product | undefined>();

  const fetchProducts = useCallback(async () => {
    try {
      const data = await getProducts();
      setProducts(data);
    } catch {
      /* empty list on error */
      setProducts([]);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchProducts();
  }, [fetchProducts]);

  const handleAdd = async (dto: ProductDto) => {
    await addProduct(dto);
    setShowForm(false);
    await fetchProducts();
  };

  const handleUpdate = async (dto: ProductDto) => {
    if (!editingProduct) return;
    await updateProduct(editingProduct.id, dto);
    setEditingProduct(undefined);
    setShowForm(false);
    await fetchProducts();
  };

  const handleDelete = async (id: string) => {
    await deleteProduct(id);
    await fetchProducts();
  };

  const handleEdit = (product: Product) => {
    setEditingProduct(product);
    setShowForm(true);
  };

  const handleCancel = () => {
    setShowForm(false);
    setEditingProduct(undefined);
  };

  const total = products.reduce((sum, p) => sum + (p.price ?? 0) * (p.quantity ?? 1), 0);
  const inCartCount = products.filter((p) => p.isInCart).length;

  return (
    <div className="space-y-5">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold tracking-tight">Mi Lista</h1>
          <p className="text-sm text-gray-400 mt-0.5">
            {products.length} producto{products.length !== 1 && "s"}
            {inCartCount > 0 && ` · ${inCartCount} en carrito`}
          </p>
        </div>
        {!showForm && (
          <button
            onClick={() => {
              setEditingProduct(undefined);
              setShowForm(true);
            }}
            className="flex items-center gap-1.5 bg-emerald-600 hover:bg-emerald-700 text-white text-sm font-medium px-4 py-2 rounded-xl transition-colors"
          >
            <Plus size={18} />
            Agregar
          </button>
        )}
      </div>

      {/* Form */}
      {showForm && (
        <ProductForm
          product={editingProduct}
          onSave={editingProduct ? handleUpdate : handleAdd}
          onCancel={handleCancel}
        />
      )}

      {/* Product list */}
      {loading ? (
        <div className="flex justify-center py-16">
          <div className="w-6 h-6 border-2 border-emerald-600 border-t-transparent rounded-full animate-spin" />
        </div>
      ) : products.length === 0 ? (
        <div className="text-center py-16 space-y-3">
          <ShoppingBasket size={48} className="mx-auto text-gray-300" />
          <p className="text-gray-400 text-sm">Tu lista está vacía</p>
          <p className="text-gray-300 text-xs">Agrega productos para comenzar</p>
        </div>
      ) : (
        <div className="space-y-2">
          {products.map((product) => (
            <ProductItem
              key={product.id}
              product={product}
              onEdit={handleEdit}
              onDelete={handleDelete}
            />
          ))}
        </div>
      )}

      {/* Total */}
      {products.length > 0 && (
        <div className="bg-white rounded-2xl p-4 shadow-sm border border-gray-100 flex items-center justify-between">
          <span className="text-sm text-gray-500">Total estimado</span>
          <span className="text-lg font-bold text-emerald-600">${total.toLocaleString()}</span>
        </div>
      )}
    </div>
  );
}
