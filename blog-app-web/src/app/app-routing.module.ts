import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CategoriesListComponent } from './components/categories-list/categories-list.component';
import { AddCategoryComponent } from './components/add-category/add-category.component';
import { EditCategoryComponent } from './components/edit-category/edit-category.component';

import { AddBlogpostComponent } from './components/add-blogpost/add-blogpost.component';
import { BlogpostListComponent } from './components/blogpost-list/blogpost-list.component';
import { EditBlogPostComponent } from './components/edit-blogpost/edit-blogpost.component';

import { HomeComponent } from './components/home/home.component';
import { BlogpostDetailsComponent } from './components/blogpost-details/blogpost-details.component';

import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';

import { ProfileComponent } from './components/profile/profile.component';
import { EditProfileComponent } from './components/profile/edit-profile/edit-profile.component';

import { UsersComponent } from './components/users/users.component';

import { AuthGuard } from './services/guard/auth.guard';
import { AdminGuard } from './services/guard/admin.guard';

export const routes: Routes = [

  { path: '', component: HomeComponent },
  { path: 'blog/:urlHandle', component: BlogpostDetailsComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'profile/edit',
    component: EditProfileComponent,
    canActivate: [AuthGuard]
  },

  {
    path: 'admin/categories',
    component: CategoriesListComponent,
    canActivate: [AdminGuard]
  },
  {
    path: 'admin/categories/add',
    component: AddCategoryComponent,
    canActivate: [AdminGuard]
  },
  {
    path: 'admin/categories/edit/:id',
    component: EditCategoryComponent,
    canActivate: [AdminGuard]
  },
  {
    path: 'blogposts',
    component: BlogpostListComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'blogposts/add',
    component: AddBlogpostComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'blogposts/edit/:id',
    component: EditBlogPostComponent,
    canActivate: [AuthGuard]
  },

  {
    path: 'admin/users',
    component: UsersComponent,
    canActivate: [AdminGuard]
  },

  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
