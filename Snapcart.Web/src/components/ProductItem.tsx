import type { Product } from "../types";
import { Pencil, Trash2, ShoppingCart, Circle } from "lucide-react";

interface Props {
  product: Product;
  onEdit: (product: Product) => void;
  onDelete: (id: string) => void;
}

export default function ProductItem({ product, onEdit, onDelete }: Props) {
  return (
    <div
      className={`flex items-center gap-3 bg-white rounded-2xl px-4 py-3 shadow-sm border transition-all ${
        product.isInCart
          ? "border-emerald-200 bg-emerald-50/50"
          : "border-gray-100"
      }`}
    >
      {/* Cart indicator */}
      <div className="flex-shrink-0">
        {product.isInCart ? (
          <ShoppingCart size={18} className="text-emerald-500" />
        ) : (
          <Circle size={18} className="text-gray-300" />
        )}
      </div>

      {/* Product info */}
      <div className="flex-1 min-w-0">
        <p className={`font-medium text-sm truncate ${product.isInCart ? "line-through text-gray-400" : ""}`}>
          {product.name}
        </p>
        <div className="flex items-center gap-2 text-xs text-gray-400 mt-0.5">
          {product.brand && <span>{product.brand}</span>}
          {product.category && (
            <>
              {product.brand && <span>·</span>}
              <span>{product.category}</span>
            </>
          )}
          {product.quantity && (
            <>
              <span>·</span>
              <span>×{product.quantity}</span>
            </>
          )}
        </div>
      </div>

      {/* Price */}
      {product.price != null && (
        <span className="text-sm font-semibold text-gray-700 whitespace-nowrap">
          ${product.price.toLocaleString()}
        </span>
      )}

      {/* Actions */}
      <div className="flex items-center gap-1 flex-shrink-0">
        <button
          onClick={() => onEdit(product)}
          className="p-1.5 rounded-lg text-gray-400 hover:text-emerald-600 hover:bg-emerald-50 transition-colors"
        >
          <Pencil size={15} />
        </button>
        <button
          onClick={() => onDelete(product.id)}
          className="p-1.5 rounded-lg text-gray-400 hover:text-red-500 hover:bg-red-50 transition-colors"
        >
          <Trash2 size={15} />
        </button>
      </div>
    </div>
  );
}
