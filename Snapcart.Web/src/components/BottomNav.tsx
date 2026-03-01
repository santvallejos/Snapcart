import { NavLink } from "react-router-dom";
import { ShoppingCart, Camera, Receipt, Settings } from "lucide-react";

const links = [
  { to: "/", icon: ShoppingCart, label: "Lista" },
  { to: "/scan", icon: Camera, label: "Escanear" },
  { to: "/purchases", icon: Receipt, label: "Compras" },
  { to: "/settings", icon: Settings, label: "Ajustes" },
];

export default function BottomNav() {
  return (
    <nav className="fixed bottom-0 left-0 right-0 bg-white border-t border-gray-200 z-50">
      <div className="max-w-lg mx-auto flex justify-around items-center h-16">
        {links.map(({ to, icon: Icon, label }) => (
          <NavLink
            key={to}
            to={to}
            className={({ isActive }) =>
              `flex flex-col items-center gap-0.5 text-xs transition-colors ${
                isActive ? "text-emerald-600" : "text-gray-400 hover:text-gray-600"
              }`
            }
          >
            <Icon size={22} strokeWidth={1.8} />
            <span>{label}</span>
          </NavLink>
        ))}
      </div>
    </nav>
  );
}
