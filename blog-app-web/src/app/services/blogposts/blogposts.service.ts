import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BlogPost, BlogPostCreate, BlogPostListResponse } from 'src/app/models/blogposts.model';
import { EditBlogPostRequest } from 'src/app/models/edit-blog-post-request.model';
import { environment } from 'src/environments/environment.staging';

@Injectable({
  providedIn: 'root',
})
export class BlogpostsService {
  private apiUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getBlogPosts(page: number = 1): Observable<BlogPostListResponse> {
    return this.http.get<BlogPostListResponse>(
      `${this.apiUrl}/BlogPost?page=${page}`
    );
  }

  getBlogPost(id: string): Observable<BlogPost> {
    return this.http.get<BlogPost>(`${this.apiUrl}/BlogPost/${id}`);
  }

  getBlogPostByUrlHandle(urlHandle: string): Observable<BlogPost> {
    return this.http.get<BlogPost>(`${this.apiUrl}/BlogPost/${urlHandle}`);
  }

  createBlogPost(blogpost: BlogPostCreate): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/BlogPost`, blogpost);
  }

  editBlogPost(id: string, blogpost: EditBlogPostRequest): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/BlogPost/${id}`, blogpost);
  }

  deleteBlogPost(id: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/BlogPost/${id}`);
  }
}
