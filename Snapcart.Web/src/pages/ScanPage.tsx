import { useRef, useState } from "react";
import { Camera, Upload, Loader2, CheckCircle2, XCircle } from "lucide-react";
import { detectProduct } from "../api/client";
import type { DetectResult } from "../types";

export default function ScanPage() {
  const fileInputRef = useRef<HTMLInputElement>(null);
  const cameraInputRef = useRef<HTMLInputElement>(null);
  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState<DetectResult | null>(null);
  const [preview, setPreview] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);

  const handleFile = async (file: File) => {
    setError(null);
    setResult(null);
    setPreview(URL.createObjectURL(file));
    setLoading(true);
    try {
      const data = await detectProduct(file);
      setResult(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Error desconocido");
    } finally {
      setLoading(false);
    }
  };

  const onFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) handleFile(file);
    e.target.value = "";
  };

  const reset = () => {
    setResult(null);
    setPreview(null);
    setError(null);
  };

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold tracking-tight">Escanear producto</h1>
        <p className="text-sm text-gray-400 mt-0.5">
          Tomá una foto o subí una imagen para detectar un producto de tu lista
        </p>
      </div>

      {/* Preview */}
      {preview && (
        <div className="relative rounded-2xl overflow-hidden border border-gray-200">
          <img src={preview} alt="Preview" className="w-full h-64 object-cover" />
          {loading && (
            <div className="absolute inset-0 bg-black/40 flex items-center justify-center">
              <Loader2 size={36} className="text-white animate-spin" />
            </div>
          )}
        </div>
      )}

      {/* Result */}
      {result && (
        <div
          className={`rounded-2xl p-5 border ${
            result.matched
              ? "bg-emerald-50 border-emerald-200"
              : "bg-amber-50 border-amber-200"
          }`}
        >
          {result.matched ? (
            <div className="flex items-start gap-3">
              <CheckCircle2 size={24} className="text-emerald-500 flex-shrink-0 mt-0.5" />
              <div>
                <p className="font-semibold text-emerald-800">¡Producto detectado!</p>
                <p className="text-sm text-emerald-700 mt-1">
                  <span className="font-medium">{result.product?.name}</span>
                  {result.product?.brand && ` · ${result.product.brand}`}
                </p>
                <p className="text-xs text-emerald-600 mt-1">Marcado como "en carrito"</p>
              </div>
            </div>
          ) : (
            <div className="flex items-start gap-3">
              <XCircle size={24} className="text-amber-500 flex-shrink-0 mt-0.5" />
              <div>
                <p className="font-semibold text-amber-800">No se encontró coincidencia</p>
                <p className="text-sm text-amber-700 mt-1">
                  {result.message || "El producto no coincide con ninguno de tu lista."}
                </p>
              </div>
            </div>
          )}
        </div>
      )}

      {/* Error */}
      {error && (
        <div className="rounded-2xl p-4 bg-red-50 border border-red-200 text-sm text-red-700">
          {error}
        </div>
      )}

      {/* Actions */}
      <div className="grid grid-cols-2 gap-3">
        <button
          onClick={() => cameraInputRef.current?.click()}
          disabled={loading}
          className="flex flex-col items-center gap-2 bg-emerald-600 hover:bg-emerald-700 disabled:opacity-50 text-white rounded-2xl py-6 transition-colors"
        >
          <Camera size={28} />
          <span className="text-sm font-medium">Tomar foto</span>
        </button>
        <button
          onClick={() => fileInputRef.current?.click()}
          disabled={loading}
          className="flex flex-col items-center gap-2 bg-white hover:bg-gray-50 disabled:opacity-50 text-gray-700 rounded-2xl py-6 border border-gray-200 transition-colors"
        >
          <Upload size={28} />
          <span className="text-sm font-medium">Subir archivo</span>
        </button>
      </div>

      {(result || error) && (
        <button
          onClick={reset}
          className="w-full text-center text-sm text-gray-400 hover:text-gray-600 py-2 transition-colors"
        >
          Escanear otro producto
        </button>
      )}

      {/* Hidden inputs */}
      <input
        ref={cameraInputRef}
        type="file"
        accept="image/*"
        capture="environment"
        className="hidden"
        onChange={onFileChange}
      />
      <input
        ref={fileInputRef}
        type="file"
        accept="image/*"
        className="hidden"
        onChange={onFileChange}
      />
    </div>
  );
}
