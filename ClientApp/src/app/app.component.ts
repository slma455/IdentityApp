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
    debugger;
    this.refreshUser();
  }

  private refreshUser(): void {
    debugger;
    const jwt = this.accountService.getJWT();
    
    if (jwt) {
      this.accountService.refreshUser(jwt).subscribe({
        next: () => {
          // User refreshed successfully
        },
        error: () => {
          this.accountService.logout();
        }
      });
    } else {
      // Explicitly set user to null when no JWT exists
      this.accountService.refreshUser(null).subscribe();
    }
  }
}