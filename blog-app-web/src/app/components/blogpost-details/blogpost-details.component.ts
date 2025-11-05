import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BlogPost } from 'src/app/models/blogposts.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { BlogpostsService } from 'src/app/services/blogposts/blogposts.service';

@Component({
  selector: 'app-blogpost-details',
  templateUrl: './blogpost-details.component.html',
  styleUrls: ['./blogpost-details.component.css'],
  providers: [BlogpostsService,]
})
export class BlogpostDetailsComponent implements OnInit {
  constructor(private blogPostService: BlogpostsService
    , private route: ActivatedRoute, private router: Router) {}
  blogPost: BlogPost = {
    id: '',
    title: '',
    content: '',
    shortDescription: '',
    featureImageUrl: '',
    author: '',
    isVisible: true,
    urlHandle: '',
    publishDate: new Date(),
    categories: []
  };

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const urlHandle = params.get('urlHandle');
      if (urlHandle) {
        this.getBlogPost(urlHandle);
        
      }
    })
  }

  getBlogPost(urlHandle: string) {
    this.blogPostService.getBlogPostByUrlHandle(urlHandle).subscribe((response: any) => {
      this.blogPost = response;
    })
  }
}
