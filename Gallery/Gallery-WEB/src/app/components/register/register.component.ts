import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
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
    private formBuilder: FormBuilder, 
    private userService: UserService,
    private loadingService: LoadingService,
    private snackService: SnackService
  ) { }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      userName: '',
      firstName: '',
      lastName: '',
      email: '',
      pw: '',
      pwc: ''
    });
  }

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
      this.userService.registration(registerUserDTO).subscribe(_ => {
        this.loadingService.isLoading = false;
      }, error => {
        console.log(error);
        this.loadingService.isLoading = false;
        this.snackService.openSnackBar(error.statusText, "OK");
      });
      this.registerForm?.reset();
    }
  }

}
