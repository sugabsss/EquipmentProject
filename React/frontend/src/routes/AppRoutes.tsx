import { BrowserRouter, Routes, Route } from "react-router-dom";
import ListEquipment from "../pages/Equipment/ListEquipment";
import CreateEquipment from "../pages/Equipment/CreateEquipment";
import EditEquipment from "../pages/Equipment/EditEquipment";

export default function AppRoutes() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<ListEquipment />} />
        <Route path="/equipamentos/criar" element={<CreateEquipment />} />
        <Route path="/equipamentos/editar/:id" element={<EditEquipment />} />
      </Routes>
    </BrowserRouter>
  );
}