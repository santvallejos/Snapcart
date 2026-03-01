import { useEffect, useState } from "react";
import { User as UserIcon, Mail, Phone, Calendar } from "lucide-react";
import { getUser } from "../api/client";
import type { User } from "../types";

export default function SettingsPage() {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getUser()
      .then(setUser)
      .catch(() => setUser(null))
      .finally(() => setLoading(false));
  }, []);

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold tracking-tight">Ajustes</h1>
        <p className="text-sm text-gray-400 mt-0.5">Tu perfil y configuración</p>
      </div>

      {loading ? (
        <div className="flex justify-center py-16">
          <div className="w-6 h-6 border-2 border-emerald-600 border-t-transparent rounded-full animate-spin" />
        </div>
      ) : user ? (
        <div className="bg-white rounded-2xl p-5 shadow-sm border border-gray-100 space-y-5">
          {/* Avatar */}
          <div className="flex items-center gap-4">
            <div className="w-14 h-14 rounded-full bg-emerald-100 flex items-center justify-center">
              <UserIcon size={28} className="text-emerald-600" />
            </div>
            <div>
              <p className="font-semibold text-lg">
                {user.name} {user.lastName}
              </p>
              <p className="text-sm text-gray-400">Cuenta activa</p>
            </div>
          </div>

          <div className="border-t border-gray-100" />

          {/* Info rows */}
          <div className="space-y-4">
            <div className="flex items-center gap-3">
              <Mail size={18} className="text-gray-400" />
              <div>
                <p className="text-xs text-gray-400">Correo electrónico</p>
                <p className="text-sm font-medium">{user.email}</p>
              </div>
            </div>
            {user.phone && (
              <div className="flex items-center gap-3">
                <Phone size={18} className="text-gray-400" />
                <div>
                  <p className="text-xs text-gray-400">Teléfono</p>
                  <p className="text-sm font-medium">{user.phone}</p>
                </div>
              </div>
            )}
            <div className="flex items-center gap-3">
              <Calendar size={18} className="text-gray-400" />
              <div>
                <p className="text-xs text-gray-400">Miembro desde</p>
                <p className="text-sm font-medium">
                  {new Date(user.createdAt).toLocaleDateString("es-AR", {
                    year: "numeric",
                    month: "long",
                    day: "numeric",
                  })}
                </p>
              </div>
            </div>
          </div>
        </div>
      ) : (
        <div className="text-center py-16">
          <p className="text-gray-400 text-sm">No se pudo cargar el usuario</p>
        </div>
      )}

      {/* App info */}
      <div className="text-center text-xs text-gray-300 space-y-1 pt-4">
        <p>Snapcart v1.0</p>
        <p>Gestiona tu carrito de compras con IA</p>
      </div>
    </div>
  );
}
