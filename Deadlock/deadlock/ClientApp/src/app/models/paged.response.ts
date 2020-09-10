export interface PagedResponse<T> {
    data?: T[];
    count?: number;
    total?: number;
}