import { api } from "../api/api.ts";

export interface EquipmentDto {
  id: string;
  nome: string;
  numeroSerie: string;
  dataAquisicao: string;
  refSetor: string;
  refNumCertificado: string;
}
export interface InsertEquipmentCommand {
    nome: string;
    numeroSerie: string;
    dataAquisicao: string;
    refSetor: string;
    refNumCertificado: string;
}

export interface UpdateEquipmentCommand {
    nome: string;
    numeroSerie: string;
    dataAquisicao: string;
    refSetor: string;
    refNumCertificado: string;
}

export interface PagedResult<T> {
  data: T[];
  pageIndex: number;
  pageSize: number;
  totalCount: number;
}

export interface WebApiResponse<T> {
  data: T;
  success: boolean;
  message: string | null;
  errors: string[] | null;
  statusCode: number;
  timestamp: string;
}

const equipamentosService = {
  getAll: (pageIndex = 1, pageSize = 20) =>
    api.get<WebApiResponse<PagedResult<EquipmentDto>>>(
      `/equipamentos?pageIndex=${pageIndex}&pageSize=${pageSize}`
    ),

  getById: (id: string) =>
    api.get<WebApiResponse<EquipmentDto>>(`/equipamentos/${id}`),

  insert: (dto: InsertEquipmentCommand) =>
    api.post<WebApiResponse<EquipmentDto>>("/equipamentos", dto),

  update: (id: string, dto: UpdateEquipmentCommand) =>
    api.put<WebApiResponse<EquipmentDto>>(`/equipamentos/${id}`, dto),

  delete: (id: string) =>
    api.delete<WebApiResponse<boolean>>(`/equipamentos/${id}`),
};

export default equipamentosService;