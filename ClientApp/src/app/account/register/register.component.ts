import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { SharedService } from 'src/app/shared/shared.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
    registerForm: FormGroup = new FormGroup({});
    submitted = false;
    errorMessages: string[] = [];

    constructor(
      private accountService: AccountService,
      private SharedService: SharedService,
      private router:Router,
      private formBuilder: FormBuilder
    ) {}

    ngOnInit(): void {
      this.initializeForm();
    }

    initializeForm() {
      this.registerForm = this.formBuilder.group({
        firstName: ['', [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(15)
        ]],
        lastName: ['', [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(15)
        ]],
        email: ['', [
          Validators.required,
          Validators.pattern("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$")
        ]],
        password: ['', [
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(15)
        ]],
      });
    }

   register() {
    this.submitted = true;
    this.errorMessages = [];
    
    // Mark all controls as touched to show validation messages
    this.registerForm.markAllAsTouched();

   this.accountService.register(this.registerForm.value).subscribe({
  next: (response: any) => {
    console.log('Backend response:', response); // Debug
    this.SharedService.showNotification(
      response.success ?? true, // Fallback to `true` if undefined
      response.title || "Success",
      response.message || "Registration successful"
    );
    this.router.navigateByUrl('/account/login');
  },
  error: (error) => {
    this.SharedService.showNotification(
      false,
      "Error",
      error.error?.message || "Registration failed"
    );
  }
});
    
}
}