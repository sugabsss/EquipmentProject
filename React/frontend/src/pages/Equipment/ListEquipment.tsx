import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";
import equipamentosService from "../../services/Equipment";

export default function ListEquipment() {
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const [pageIndex, setPageIndex] = useState(1);
  const pageSize = 20;

  const { data, isLoading, isError } = useQuery({
    queryKey: ["equipamentos", pageIndex],
    queryFn: () => equipamentosService.getAll(pageIndex, pageSize),
  });

  const { mutate: deleteEquipment, isPending: isDeleting } = useMutation({
    mutationFn: (id: string) => equipamentosService.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["equipamentos"] });
    },
    onError: () => {
      alert("Erro ao deletar equipamento.");
    },
  });

  const handleDelete = (id: string, nome: string) => {
    if (confirm(`Deseja realmente deletar "${nome}"?`)) {
      deleteEquipment(id);
    }
  };

  const paged = data?.data.data;

  if (isLoading) return <p>Carregando...</p>;
  if (isError) return <p>Erro ao carregar equipamentos.</p>;

  return (
    <div style={{ padding: "2rem" }}>
      <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "1rem" }}>
        <h1>Equipamentos</h1>
        <button onClick={() => navigate("/equipamentos/criar")}>+ Novo</button>
      </div>
      <table border={1} cellPadding={8} width="100%">
        <thead>
          <tr>
            <th>Nome</th>
            <th>Número de Série</th>
            <th>Data Aquisição</th>
            <th>Setor</th>
            <th>Certificado</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {!paged?.data?.length ? (
            <tr>
              <td colSpan={6} style={{ textAlign: "center", padding: "2rem", color: "#888" }}>
                Nenhum equipamento cadastrado.
              </td>
            </tr>
          ) : (
            paged.data.map((eq: any) => (
              <tr key={eq.id}>
                <td>{eq.nome}</td>
                <td>{eq.numeroSerie}</td>
                <td>{new Date(eq.dataAquisicao).toLocaleDateString("pt-BR")}</td>
                <td>{eq.refSetor}</td>
                <td>{eq.refNumCertificado}</td>
                <td>
                  <button onClick={() => navigate(`/equipamentos/editar/${eq.id}`)}>
                    Editar
                  </button>
                  <button
                    style={{ marginLeft: "0.5rem", color: "red" }}
                    disabled={isDeleting}
                    onClick={() => handleDelete(eq.id, eq.nome)}
                  >
                    Deletar
                  </button>
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
      <div style={{ marginTop: "1rem", display: "flex", gap: "1rem" }}>
        <button disabled={pageIndex === 1} onClick={() => setPageIndex((p) => p - 1)}>
          Anterior
        </button>
        <span>Página {pageIndex} de {Math.ceil((paged?.totalCount ?? 0) / pageSize)}</span>
        <button
          disabled={pageIndex >= Math.ceil((paged?.totalCount ?? 0) / pageSize)}
          onClick={() => setPageIndex((p) => p + 1)}
        >
          Próxima
        </button>
      </div>
    </div>
  );
}