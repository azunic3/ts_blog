import { Component } from '@angular/core';
import { AuthService } from '../services/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  providers: [AuthService],
})
export class RegisterComponent {
  username = '';
  email = '';
  password = '';

  fullName = '';
  bio = '';
  profileImageUrl = '';

  errorMessage = '';

  constructor(private authService: AuthService, private router: Router) {}

  register(): void {
    this.authService
      .register({
        username: this.username,
        email: this.email,
        password: this.password,
        fullName: this.fullName,
        bio: this.bio,
        profileImageUrl: this.profileImageUrl,
      })
      .subscribe({
        next: () => {
          alert('Registration successful!');
          this.router.navigate(['/login']);
        },
        error: (err) => {
          this.errorMessage = err?.error?.[0] ?? 'Error registering user';
        },
      });
  }
}
