export interface IPagedRequest {
  currentPage: number | string,
  pageSize: number
}

export interface IPagedResult<T> {
  currentPage: number,
  pageSize: number,
  totalItems: number,
  totalPages: number,
  data: T[]
}