
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BlogImagePost } from 'src/app/models/blog-image-post.model';
import { BlogImage } from 'src/app/models/blog-image.model';
import { environment } from 'src/environments/environment.staging';


const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
  }),
};
@Injectable({
  providedIn: 'root'
})
export class BlogImagesService {

  constructor(private http: HttpClient) { }

  saveBlogImage(blogImage: BlogImagePost): Observable<BlogImagePost> {
    return this.http.post<BlogImagePost>(`${environment.apiUrl}/BlogImages/Upload`, blogImage, httpOptions);
  }

  uploadImage(file: File, fileName: string, title: string): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('fileName', fileName);
    formData.append('title', title);

    return this.http.post(`${environment.apiUrl}/BlogImages/Upload`, formData);
  }

  getBlogImages(): Observable<BlogImage[]>{
    return this.http.get<BlogImage[]>(`${environment.apiUrl}/BlogImages`);
  }
}