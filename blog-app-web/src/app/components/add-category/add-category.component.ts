import { Component, OnInit } from '@angular/core';
import { takeUntil } from 'rxjs';
import { Subject } from 'rxjs';
import { CategoryCreate } from 'src/app/models/categories.model';
import { Router } from '@angular/router';
import { CategoriesService } from 'src/app/services/categories/categories.service';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css'],
  providers: [ CategoriesService, AuthService ]
})
export class AddCategoryComponent implements OnInit {
  category: CategoryCreate = {
    name: '',
    urlHandle: ''
  };
  private destroy$ = new Subject<void>();
selectedCategory: any;
categories: any;

  constructor(private categoryService: CategoriesService, private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    if (!this.authService.isLoggedIn() || !this.authService.isAdmin()) {
      this.router.navigate(['/']);
    }
  }

  createCategory(e: Event): void {
    e.preventDefault();
    this.categoryService.createCategory(this.category)
      .pipe(takeUntil(this.destroy$))
      .subscribe(response => {
        if (response.statusCode === 201) {
          alert('Category created successfully');
          this.category = {
            name: '',
            urlHandle: ''
          }
          this.router.navigate(['/admin/categories']);
          
        } else {
          alert('Failed to create category');
        }
      }, error => {
        alert('Error creating category');
        console.error('Error:', error);
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  
}
