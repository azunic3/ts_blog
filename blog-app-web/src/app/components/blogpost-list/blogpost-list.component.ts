import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { BlogPost } from 'src/app/models/blogposts.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { BlogpostsService } from 'src/app/services/blogposts/blogposts.service';

@Component({
  selector: 'app-blogpost-list',
  templateUrl: './blogpost-list.component.html',
  styleUrls: ['./blogpost-list.component.css'],
  providers: [ BlogpostsService, AuthService ]
})
export class BlogpostListComponent implements OnInit {
  blogPosts: BlogPost[] = [];
  pages: number = 1;
  page: number = 1;

  private destroy$ = new Subject<void>();

  constructor(private blogPostService: BlogpostsService, private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.getBlogPosts();
  }

  navigateToAddBlogPost(): void {
    this.router.navigate(['/admin/blogposts/add']);
  }

  getBlogPosts(page: number = 1): void {
    this.blogPostService
      .getBlogPosts(page)
      .subscribe((response: any) => {
        this.blogPosts = response.blogPosts;
        this.pages = response.page;
      });
  }

  deleteBlogPost(id: string): void {
    this.blogPostService.deleteBlogPost(id).subscribe(() => {
      this.getBlogPosts(this.page);
      alert('BlogPost deleted successfully');
    })
  }

  nextPage(): void {
    this.page++;
    this.getBlogPosts(this.page);
  }

  previousPage(): void {
    this.page--;
    this.getBlogPosts(this.page);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

}
