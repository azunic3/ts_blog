import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { AddCategoryComponent } from './components/add-category/add-category.component';
import { CategoriesListComponent } from './components/categories-list/categories-list.component';
import { EditCategoryComponent } from './components/edit-category/edit-category.component';
import { BlogpostListComponent } from './components/blogpost-list/blogpost-list.component';
import { AddBlogpostComponent } from './components/add-blogpost/add-blogpost.component';
import { MarkdownModule } from 'ngx-markdown';
import { HomeComponent } from './components/home/home.component';
import { BlogpostDetailsComponent } from './components/blogpost-details/blogpost-details.component';
import { LoginComponent } from './login/login.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './interceptors/auth.service';
import { RegisterComponent } from './register/register.component';
import { EditBlogPostComponent } from './components/edit-blogpost/edit-blogpost.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    AddCategoryComponent,
    CategoriesListComponent,
    EditCategoryComponent,
    BlogpostListComponent,
    AddBlogpostComponent,
    HomeComponent,
    BlogpostDetailsComponent,
    LoginComponent,
    RegisterComponent,
    EditBlogPostComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    MarkdownModule.forRoot()
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
