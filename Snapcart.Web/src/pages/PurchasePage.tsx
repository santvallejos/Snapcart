import { useState, useCallback, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { ShoppingBag, CheckCircle2, Store } from "lucide-react";
import { getProducts, completePurchase } from "../api/client";
import type { Product } from "../types";

export default function PurchasePage() {
  const navigate = useNavigate();
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [completing, setCompleting] = useState(false);
  const [supermarket, setSupermarket] = useState("");
  const [completed, setCompleted] = useState(false);
  const [finalTotal, setFinalTotal] = useState(0);
  const [finalProducts, setFinalProducts] = useState<Product[]>([]);

  const fetchProducts = useCallback(async () => {
    try {
      const data = await getProducts();
      setProducts(data);
    } catch {
      setProducts([]);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchProducts();
  }, [fetchProducts]);

  const total = products.reduce((sum, p) => sum + (p.price ?? 0) * (p.quantity ?? 1), 0);

  const handleComplete = async () => {
    setCompleting(true);
    try {
      await completePurchase({
        purchaseCompletedAt: new Date().toISOString(),
        supermarketName: supermarket.trim() || undefined,
      });
      setFinalTotal(total);
      setFinalProducts([...products]);
      setCompleted(true);
    } catch {
      alert("Error al finalizar la compra");
    } finally {
      setCompleting(false);
    }
  };

  if (completed) {
    return (
      <div className="space-y-6">
        <div className="text-center py-8 space-y-3">
          <CheckCircle2 size={56} className="mx-auto text-emerald-500" />
          <h1 className="text-2xl font-bold">¡Compra finalizada!</h1>
          <p className="text-gray-400 text-sm">Tu lista ha sido guardada y limpiada</p>
        </div>

        {/* Summary */}
        <div className="bg-white rounded-2xl p-5 shadow-sm border border-gray-100 space-y-4">
          <h2 className="font-semibold">Resumen de compra</h2>
          <div className="divide-y divide-gray-100">
            {finalProducts.map((p) => (
              <div key={p.id} className="flex items-center justify-between py-2.5 text-sm">
                <div>
                  <span className="font-medium">{p.name}</span>
                  {p.brand && <span className="text-gray-400 ml-1">· {p.brand}</span>}
                  {p.quantity && <span className="text-gray-400 ml-1">×{p.quantity}</span>}
                </div>
                <span className="font-medium text-gray-700">
                  {p.price != null ? `$${((p.price) * (p.quantity ?? 1)).toLocaleString()}` : "—"}
                </span>
              </div>
            ))}
          </div>
          <div className="border-t border-gray-100 pt-3 flex justify-between">
            <span className="font-semibold">Total</span>
            <span className="text-lg font-bold text-emerald-600">${finalTotal.toLocaleString()}</span>
          </div>
        </div>

        <button
          onClick={() => navigate("/")}
          className="w-full bg-emerald-600 hover:bg-emerald-700 text-white font-medium py-3 rounded-xl transition-colors text-sm"
        >
          Volver a mi lista
        </button>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold tracking-tight">Finalizar compra</h1>
        <p className="text-sm text-gray-400 mt-0.5">Revisá tu lista y completá la compra</p>
      </div>

      {loading ? (
        <div className="flex justify-center py-16">
          <div className="w-6 h-6 border-2 border-emerald-600 border-t-transparent rounded-full animate-spin" />
        </div>
      ) : products.length === 0 ? (
        <div className="text-center py-16 space-y-3">
          <ShoppingBag size={48} className="mx-auto text-gray-300" />
          <p className="text-gray-400 text-sm">No tenés productos en tu lista</p>
          <button
            onClick={() => navigate("/")}
            className="text-emerald-600 text-sm font-medium hover:underline"
          >
            Agregar productos
          </button>
        </div>
      ) : (
        <>
          {/* Product summary */}
          <div className="bg-white rounded-2xl p-4 shadow-sm border border-gray-100 space-y-2">
            {products.map((p) => (
              <div key={p.id} className="flex items-center justify-between py-2 text-sm">
                <div className="flex items-center gap-2">
                  <span className={`w-2 h-2 rounded-full ${p.isInCart ? "bg-emerald-400" : "bg-gray-300"}`} />
                  <span className={p.isInCart ? "text-gray-500" : "font-medium"}>{p.name}</span>
                  {p.quantity && <span className="text-gray-400">×{p.quantity}</span>}
                </div>
                <span className="font-medium text-gray-700">
                  {p.price != null ? `$${((p.price) * (p.quantity ?? 1)).toLocaleString()}` : "—"}
                </span>
              </div>
            ))}
            <div className="border-t border-gray-100 pt-3 flex justify-between mt-2">
              <span className="font-semibold">Total</span>
              <span className="text-lg font-bold text-emerald-600">${total.toLocaleString()}</span>
            </div>
          </div>

          {/* Supermarket */}
          <div className="flex items-center gap-3 bg-white rounded-2xl px-4 py-3 shadow-sm border border-gray-100">
            <Store size={20} className="text-gray-400 flex-shrink-0" />
            <input
              type="text"
              placeholder="Supermercado (opcional)"
              value={supermarket}
              onChange={(e) => setSupermarket(e.target.value)}
              className="flex-1 text-sm bg-transparent focus:outline-none"
            />
          </div>

          {/* Complete button */}
          <button
            onClick={handleComplete}
            disabled={completing}
            className="w-full flex items-center justify-center gap-2 bg-emerald-600 hover:bg-emerald-700 disabled:opacity-50 text-white font-medium py-3 rounded-xl transition-colors text-sm"
          >
            {completing ? (
              <div className="w-5 h-5 border-2 border-white border-t-transparent rounded-full animate-spin" />
            ) : (
              <CheckCircle2 size={20} />
            )}
            {completing ? "Finalizando..." : "Finalizar compra"}
          </button>
        </>
      )}
    </div>
  );
}
