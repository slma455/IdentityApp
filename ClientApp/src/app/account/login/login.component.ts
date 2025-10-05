import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { SharedService } from 'src/app/shared/shared.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({});
  submitted = false;
  returnUrl: string = '/';
  errorMessages: string[] = [];

  constructor(
    private accountService: AccountService,
    private sharedService: SharedService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
    });
  }

onSubmit() {
  this.submitted = true;
  this.errorMessages = [];

  if (this.loginForm.valid) {
    this.accountService.login(this.loginForm.value)
      .pipe(first())
      .subscribe({
        next: (user: any) => {
          debugger;

          if (user) {
            this.router.navigateByUrl('/');
          } else {
            this.activatedRoute.queryParams.subscribe({
              next: (params: any) => {
                this.returnUrl = params['returnUrl'] || '/';
              }
            });
          }

          // ✅ Move notification inside 'next' callback
          this.sharedService.showNotification(
            true, 
            user.title || 'Login Success', 
            user.message || 'Login successful'
          );

          this.router.navigate(['/']);
        },
        error: (error) => {
          // Handle login error - 3 parameters: isSuccess, title, message
          if (error.error?.errors) {
            this.errorMessages = error.error.errors;
          } else {
            this.errorMessages = ['Login failed. Please check your credentials.'];
          }

          this.sharedService.showNotification(
            false, 
            error.error?.title || 'Login Failed', 
            error.error?.message || 'Login failed. Please check your credentials.'
          );
        }
      });
  } else {
    // Form is invalid
    this.errorMessages = ['Please fix the validation errors.'];

    this.sharedService.showNotification(
      false, 
      'Form Validation Error', 
      'Please fix the validation errors before submitting.'
    );
  }
}

}