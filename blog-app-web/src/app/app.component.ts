import { Component } from '@angular/core';
import { AuthService } from './services/auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title(title: any) {
    throw new Error('Method not implemented.');
  }
  constructor(public authService: AuthService) {}

  logout() {
    this.authService.logout();
    // dodaj navigaciju na login stranicu ili početnu stranicu nakon logout-a
  }
}
