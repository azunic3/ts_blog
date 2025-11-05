import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Category } from 'src/app/models/categories.model';
import { Subject } from 'rxjs';
import { CategoriesService } from 'src/app/services/categories/categories.service';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-categories-list',
  templateUrl: './categories-list.component.html',
  styleUrls: ['./categories-list.component.css'],
  providers: [CategoriesService, AuthService],
})
export class CategoriesListComponent implements OnInit {
onCategoryChange($event: Event) {
throw new Error('Method not implemented.');
}
  categories: Category[] = [];  
  private destroy$ = new Subject<void>();
selectedCategory: any;

  constructor(
    private categoriesService: CategoriesService,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.getCategories();
  }

  navigateToAddCategory(): void {
    this.router.navigate(['/admin/categories/add']);
  }

  navigateToEditCategory(id: string): void {
    this.router.navigate(['/admin/categories/edit', id]);
  }

  getCategories(): void {
    this.categoriesService
      .getCategories()
      .subscribe((response: any) => {
        this.categories = response;
      });
  }

  deleteCategory(id: string): void {
    this.categoriesService.deleteCategory(id).subscribe(() => {
      this.getCategories();
      alert('Category deleted successfully');
    })
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
