import { Component, OnInit } from '@angular/core';
import { AdminUser } from 'src/app/models/admin-user.model';
import { AdminUsersService } from 'src/app/services/admin-users.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: AdminUser[] = [];
  loading = false;

  constructor(private usersService: AdminUsersService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading = true;
    this.usersService.getUsers().subscribe({
      next: (res) => {
        this.users = res ?? [];
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        alert(err?.error?.message ?? 'Failed to load users');
        this.loading = false;
      }
    });
  }

  deleteUser(id: string): void {
    if (!confirm('Delete this user?')) return;

    this.usersService.deleteUser(id).subscribe({
      next: () => {
        this.users = this.users.filter(u => u.id !== id);
      },
      error: (err) => {
        console.error(err);
        alert(err?.error?.message ?? 'Failed to delete user');
      }
    });
  }
}