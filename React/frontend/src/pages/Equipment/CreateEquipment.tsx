import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { useMutation } from "@tanstack/react-query";
import type { InsertEquipmentCommand } from "../../services/Equipment";
import equipamentosService from "../../services/Equipment";

type FormData = Omit<InsertEquipmentCommand, "id">;

export default function CreateEquipment() {
  const navigate = useNavigate();
  const { register, handleSubmit } = useForm<FormData>();

  const { mutate, isPending } = useMutation({
    mutationFn: (data: FormData) => equipamentosService.insert(data),
    onSuccess: () => navigate("/"),
  });

  return (
    <div style={{ padding: "2rem" }}>
      <h1>Novo Equipamento</h1>
      <form onSubmit={handleSubmit((data) => mutate(data))}>
        <div><label>Nome</label><br />
          <input {...register("nome")} /></div>
        <div><label>Número de Série</label><br />
          <input {...register("numeroSerie")} /></div>
        <div><label>Data de Aquisição</label><br />
          <input type="date" {...register("dataAquisicao")} /></div>
        <div><label>Setor</label><br />
          <input {...register("refSetor")} /></div>
        <div><label>Certificado</label><br />
          <input {...register("refNumCertificado")} /></div>
        <br />
        <button type="button" onClick={() => navigate("/")}>Cancelar</button>
        <button type="submit" disabled={isPending} style={{ marginLeft: "0.5rem" }}>
          {isPending ? "Salvando..." : "Salvar"}
        </button>
      </form>
    </div>
  );
}