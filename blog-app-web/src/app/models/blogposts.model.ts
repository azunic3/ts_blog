import { Category } from "./categories.model";

export interface BlogPost {
  id: string;
  title: string;
  shortDescription: string;
  content: string;
  featureImageUrl: string;
  urlHandle: string;
  author: string;
  isVisible: boolean;
  publishDate: Date;
  categories: Category[];
}

export interface BlogPostCreate {
  title: string;
  shortDescription: string;
  content: string;
  featureImageUrl: string;
  urlHandle: string;
  author: string;
  isVisible: boolean;
  publishDate: Date;
  categories: string[];
}

export interface BlogPostUpdate {
  title: string;
  shortDescription: string;
  content: string;
  featureImageUrl: string;
  urlHandle: string;
  author: string;
  isVisible: boolean;
  publishDate: Date;

}

export interface BlogPostListResponse {
  blogPosts: BlogPost[];
  page: number;
}
