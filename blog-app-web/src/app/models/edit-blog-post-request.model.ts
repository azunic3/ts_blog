

export interface EditBlogPostRequest {
    title: string;
    shortDescription: string;
    content: string;
    featureImageURL: string;
    urlHandle: string;
    publishDate: Date;
    author: string;
    isVisible: boolean;
    categories: string[];
  }
  