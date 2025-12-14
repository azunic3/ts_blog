import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommentGetDto } from 'src/app/models/comment.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { CommentService } from 'src/app/services/comments.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css'],
})
export class CommentsComponent implements OnChanges {
  @Input() blogPostId: string | null = null;

  comments: CommentGetDto[] = [];
  loading = false;
  error?: string;

  form: FormGroup;
  saving = false;

  constructor(
    private commentService: CommentService,
    private fb: FormBuilder,
    private authService: AuthService
  ) {
    this.form = this.fb.group({
      content: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(1000)]],
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['blogPostId']) {
      const id = this.blogPostId?.trim();
      if (!id) return;          // ✅ ne šalji /blog/ bez ID-a
      this.load(id);
    }
  }

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  get isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  private load(id: string): void {
    this.loading = true;
    this.error = undefined;

    this.commentService.getByBlog(id).subscribe({
      next: (data) => {
        this.comments = data ?? [];
        this.loading = false;
      },
      error: () => {
        this.error = 'Unable to load comments.';
        this.loading = false;
      },
    });
  }

  save(): void {
    if (!this.isLoggedIn) {
      this.error = 'You must be logged in to comment.';
      return;
    }

    const id = this.blogPostId?.trim();
    if (!id) return;

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.error = undefined;

    this.commentService.add({
      blogPostId: id,
      content: this.form.value.content,
    }).subscribe({
      next: () => {
        this.form.reset();
        this.saving = false;
        this.load(id);
      },
      error: () => {
        this.error = 'Failed to add comment.';
        this.saving = false;
      },
    });
  }

  deleteComment(id: string): void {
    if (!this.isAdmin) return;

    const blogId = this.blogPostId?.trim();
    if (!blogId) return;

    this.commentService.delete(id).subscribe({
      next: () => this.load(blogId),
      error: () => (this.error = 'Failed to delete comment.'),
    });
  }

  formatDate(iso: string): string {
    return new Date(iso).toLocaleString();
  }
}
