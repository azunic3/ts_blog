import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BlogPost } from 'src/app/models/blogposts.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { BlogpostsService } from 'src/app/services/blogposts/blogposts.service';

@Component({
  selector: 'app-blogpost-list',
  templateUrl: './blogpost-list.component.html',
  styleUrls: ['./blogpost-list.component.css']
})
export class BlogpostListComponent implements OnInit {
  blogPosts: BlogPost[] = [];
  pages = 1;     // -1 kad nema vise
  page = 1;
  loading = false;
  errorMessage = '';

  constructor(
    private blogPostService: BlogpostsService,
    public authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getBlogPosts(1);
  }

  navigateToAddBlogPost(): void {
    this.router.navigate(['/blogposts/add']);
  }

  getBlogPosts(page: number = 1): void {
    if (page < 1) page = 1;

    this.loading = true;
    this.errorMessage = '';

    const request$ = this.authService.isAdmin()
      ? this.blogPostService.getBlogPosts(page)     
      : this.blogPostService.getMyBlogPosts(page); 

    request$.subscribe({
      next: (response: any) => {
        this.blogPosts = response.blogPosts ?? [];
        this.pages = response.page ?? -1;
        this.page = page;
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;

        if (err?.status === 401) {
          this.errorMessage = 'You must be logged in.';
          this.router.navigate(['/login']);
          return;
        }

        if (err?.status === 403) {
          this.errorMessage = 'You do not have permission to view this.';
          this.router.navigate(['/']);
          return;
        }

        this.errorMessage = 'Failed to load blog posts.';
      }
    });
  }

  deleteBlogPost(id: string): void {
    if (!this.authService.isAdmin()) return;

    this.blogPostService.deleteBlogPost(id).subscribe({
      next: () => {
        this.getBlogPosts(this.page);
        alert('Blog post deleted successfully');
      },
      error: () => {
        alert('Delete failed.');
      }
    });
  }

  nextPage(): void {
    if (this.pages === -1) return;
    this.getBlogPosts(this.page + 1);
  }

  previousPage(): void {
    if (this.page <= 1) return;
    this.getBlogPosts(this.page - 1);
  }
}
