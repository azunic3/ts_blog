export interface Category {
    selected: boolean;
    id: string;
    name: string;
    urlHandle: string;
}

export interface CategoryCreate {
    name: string;
    urlHandle: string;
}

export interface CategoryUpdate {
    name: string;
    urlHandle: string;
}

export interface CategoriesListResponse {
    categories: Category[];
    page: number;
}