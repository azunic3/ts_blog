export interface BlogImagePost{
    id?: string;
    fileName: string;
    fileExtenstion?: string;
    title: string;
    url?: string;
    dateTime?: Date;
    file?: File | null;
}