import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { useNavigate, useParams } from "react-router-dom";
import { useMutation, useQuery } from "@tanstack/react-query";
import equipamentosService, { type EquipmentDto } from "../../services/Equipment";

export default function EditEquipment() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { register, handleSubmit, reset } = useForm<EquipmentDto>();

  const { data, isLoading } = useQuery({
    queryKey: ["equipamento", id],
    queryFn: () => equipamentosService.getById(id!),
    enabled: !!id,
  });

  // Preenche o form quando os dados chegam
  useEffect(() => {
    if (data?.data.data) {
      reset(data.data.data);
    }
  }, [data, reset]);

  const { mutate, isPending } = useMutation({
    mutationFn: (formData: EquipmentDto) =>
      equipamentosService.update(id!, formData),
    onSuccess: () => navigate("/"),
  });

  if (isLoading) return <p>Carregando...</p>;

  return (
    <div style={{ padding: "2rem" }}>
      <h1>Editar Equipamento</h1>
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