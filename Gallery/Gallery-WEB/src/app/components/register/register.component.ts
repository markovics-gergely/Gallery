import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterUserDTO } from 'models';
import { LoadingService } from 'src/app/services/loading.service';
import { SnackService } from 'src/app/services/snack.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup | undefined;

  constructor(
    private userService: UserService,
    private loadingService: LoadingService,
    private snackService: SnackService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      userName: new FormControl('', Validators.required),
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      pw: new FormControl('', Validators.required),
      pwc: new FormControl('', Validators.required)
    });
  }

  /**
   * Submit registration to the server
   */
  onSubmit(): void {
    if (this.registerForm) {
      this.loadingService.isLoading = true;
      let registerUserDTO: RegisterUserDTO = { 
        userName: this.registerForm.get('userName')?.value, 
        firstName: this.registerForm.get('firstName')?.value, 
        lastName: this.registerForm.get('lastName')?.value, 
        email: this.registerForm.get('email')?.value, 
        password: this.registerForm.get('pw')?.value,
        confirmedPassword: this.registerForm.get('pwc')?.value
      };
      this.userService.registration(registerUserDTO).subscribe({
        next: () => { this.router.navigate(['login']); },
        error: (err) => { this.snackService.openSnackBar(err.statusText, "OK"); }
      }).add(() => { this.loadingService.isLoading = false; });
      this.registerForm?.reset();
    }
  }

  /**
   * Flag for register form valid status
   */
  get validForm(): boolean { return this.registerForm?.valid || false; }
}
