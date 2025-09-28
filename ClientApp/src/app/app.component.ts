import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'ClientApp';

  constructor(private accountService: AccountService) {}

  ngOnInit() {
    this.refreshUser();
  }

  private refreshUser(): void {
    const jwt = this.accountService.getJWT();
    
    if (jwt) {
      this.accountService.refreshUser(jwt).subscribe({
        next: () => {
          // User refreshed successfully - optional callback
        },
        error: (error: any) => {
          console.error('Failed to refresh user:', error);
          this.accountService.logout();
        }
      });
    }
    // If no JWT exists, do nothing - user is not logged in
  }
}