import { Outlet } from "react-router-dom";
import BottomNav from "./BottomNav";

export default function Layout() {
  return (
    <div className="min-h-screen bg-gray-50 text-gray-900">
      <main className="max-w-lg mx-auto pb-20 px-4 pt-6">
        <Outlet />
      </main>
      <BottomNav />
    </div>
  );
}
