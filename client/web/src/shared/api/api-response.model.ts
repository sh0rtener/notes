export interface ApiResponse<T> {
    Message: string;
    Data: T | null
}