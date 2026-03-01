import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./components/Layout";
import ListPage from "./pages/ListPage";
import ScanPage from "./pages/ScanPage";
import PurchasePage from "./pages/PurchasePage";
import SettingsPage from "./pages/SettingsPage";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<Layout />}>
          <Route path="/" element={<ListPage />} />
          <Route path="/scan" element={<ScanPage />} />
          <Route path="/purchases" element={<PurchasePage />} />
          <Route path="/settings" element={<SettingsPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
