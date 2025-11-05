import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { BlogImagePost } from 'src/app/models/blog-image-post.model';
import { BlogPost } from 'src/app/models/blogposts.model';
import {  Category } from 'src/app/models/categories.model';
import { EditBlogPostRequest } from 'src/app/models/edit-blog-post-request.model';
import { BlogImagesService } from 'src/app/services/blog-images/blog-images.service';
import { BlogpostsService } from 'src/app/services/blogposts/blogposts.service';
import { CategoriesService } from 'src/app/services/categories/categories.service';

@Component({
  selector: 'app-edit-blog-post',
  templateUrl: './edit-blogpost.component.html',
  styleUrls: ['./edit-blogpost.component.css']
})
export class EditBlogPostComponent implements OnInit, OnDestroy {
  id: string = '';
  blogPost?: BlogPost;
  categories: Category[] = []; 
  selectedCategories: string[] = []; 
  paramsSubscription?: Subscription;
  updateBlogPostSubscription?: Subscription;
  private uploadSubscription?: Subscription;
  markdownContent: string = '';
  blogImages$!: Observable<Array<BlogImagePost>>;
  selectedFile: File | null = null;
  fileName: string = '';
  title: string = '';

  @ViewChild('imageModal') imageModal!: ElementRef;

  constructor(
    private route: ActivatedRoute,
    private blogPostService: BlogpostsService,
    private categoryService: CategoriesService, 
    private blogImageService: BlogImagesService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.paramsSubscription = this.route.paramMap.subscribe(params => {
      this.id = params.get('id') ?? '';
      this.fetchBlogPost(this.id);
      this.loadCategories();
      
    });
    this.blogImages$ = this.blogImageService.getBlogImages();
  }

  private fetchBlogPost(id: string): void {
    this.blogPostService.getBlogPost(id).subscribe(res => {
      this.blogPost = res;
      this.markdownContent = this.blogPost.content;
      this.selectedCategories = this.blogPost.categories.map(cat => cat.id);
    });
  }

  private loadCategories(): void {
    this.categoryService.getCategories().subscribe((response: any) => {
      this.categories = response;
    });
  }

  onFormSubmit(): void {
    if (this.blogPost) {
      const request: EditBlogPostRequest = {
        title: this.blogPost.title,
        shortDescription: this.blogPost.shortDescription,
        content: this.blogPost.content,
        featureImageURL: this.blogPost.featureImageUrl,
        urlHandle: this.blogPost.urlHandle,
        publishDate: this.blogPost.publishDate,
        author: this.blogPost.author,
        isVisible: this.blogPost.isVisible,
        categories: this.selectedCategories
      };

      this.updateBlogPostSubscription = this.blogPostService.editBlogPost(this.id, request).subscribe(() => {
        this.router.navigateByUrl('/admin/blogposts');
      });
    }
  }

  ngOnDestroy(): void {
    this.paramsSubscription?.unsubscribe();
    this.updateBlogPostSubscription?.unsubscribe();
    this.uploadSubscription?.unsubscribe();
  }

  updateMarkdown(content: string): void {
    this.markdownContent = content;
  }

  SubmitImageForm(): void {
    if (this.selectedFile && this.fileName && this.title) {
      this.uploadSubscription = this.blogImageService.uploadImage(this.selectedFile, this.fileName, this.title)
        .subscribe({
          next: (response) => {
            if (response && response.filePath) {
              this.blogPost!.featureImageUrl = response.filePath;
              this.blogImages$ = this.blogImageService.getBlogImages();
              this.closeModal();
            } else {
              console.error('Upload response does not contain filePath:', response);
            }
          },
          error: (error) => {
            console.error('Error uploading image:', error);
            window.alert('Use an image with an extension that is either .png, .jpg, or .jpeg!');
          }
        });
    } else {
      window.alert('File, file name, or title is missing.');
    }
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  onImageClick(url: string | undefined): void {
    this.blogPost!.featureImageUrl = url ?? '';
    this.closeModal();
  }

  private closeModal(): void {
    if (this.imageModal && this.imageModal.nativeElement) {
      const modalElement = this.imageModal.nativeElement;
      modalElement.classList.remove('show');
      modalElement.setAttribute('aria-hidden', 'true');
      modalElement.style.display = 'none';

      // Remove the backdrop element
      const backdrop = document.querySelector('.modal-backdrop');
      if (backdrop) {
        backdrop.remove();
      }

      // Ensure the button is re-enabled and not affected by focus issues
      const uploadButton = document.querySelector('button[data-bs-target="#imageModal"]') as HTMLElement;
      if (uploadButton) {
        uploadButton.blur(); // Blurring to prevent requiring a double-click
      }

      // Remove the 'modal-open' class from the body to enable scrolling
      document.body.classList.remove('modal-open');
      document.documentElement.style.overflow = 'auto';
    }
  }
}

