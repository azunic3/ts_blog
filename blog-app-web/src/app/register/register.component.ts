import { Component } from '@angular/core';
import { AuthService } from '../services/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  providers: [AuthService]
})
export class RegisterComponent {
  username: string = '';
  password: string = '';
  email: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  register(): void {
    this.authService
      .register(this.username, this.email, this.password)
      .subscribe(() => {
        this.username = '';
        this.email = '';
        this.password = '';
        alert('Registration successful!');
        this.router.navigate(['/login']);
      });
  }
}
