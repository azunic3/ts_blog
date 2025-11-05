import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Category, CategoryUpdate } from 'src/app/models/categories.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { CategoriesService } from 'src/app/services/categories/categories.service';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css'],
  providers: [CategoriesService, AuthService]
})
export class EditCategoryComponent implements OnInit {
  category: Category = {
    selected: false, // Dodajte ovo svojstvo
    id: '',
    name: '',
    urlHandle: ''
  };
  
  constructor(private categoriesService: CategoriesService, private route: ActivatedRoute, private router: Router, private authService: AuthService) {}

  ngOnInit(): void {

    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.getCategory(id);
      }
    });
  }

  getCategory(id: string): void {
    this.categoriesService.getCategory(id).subscribe((response: any) => {
      this.category.id = response.id;
      this.category.name = response.name;
      this.category.urlHandle = response.urlHandle;
      this.category.selected = false; // Postavite po defaultu
    });
  }

  editCategory(category: Category, e: Event): void {
    e.preventDefault();
    const categoryUpdate: CategoryUpdate = {
      name: category.name,
      urlHandle: category.urlHandle
    };
    this.categoriesService.editCategory(category.id, categoryUpdate).subscribe((response: any) => {
      alert('Category updated successfully');
      this.router.navigate(['/admin/categories']);
    });
  }
}
