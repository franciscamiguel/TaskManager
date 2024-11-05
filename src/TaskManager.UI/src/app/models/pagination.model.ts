export interface PaginationFilter {
    pageNumber: number;
    pageSize: number;
  }
  
  export interface PagedResponse<T> {
    data: T[];
    pageNumber: number;
    pageSize: number;
    totalRecords: number;
  }
  