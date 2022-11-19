import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginUserDTO, LoginUserResponse } from 'models';
import { timer } from 'rxjs';
import { LoadingService } from 'src/app/services/loading.service';
import { SnackService } from 'src/app/services/snack.service';
import { TokenService } from 'src/app/services/token.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup | undefined;

  constructor(
    private userService: UserService,
    private router: Router, 
    private tokenService: TokenService,
    private snackService: SnackService,
    private loadingService: LoadingService
  ) { }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      userName: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    });
  }

  /**
   * Submit login to the server
   */
  onSubmit(): void {
    if (this.loginForm) {
      let loginUserDTO: LoginUserDTO = { 
        username: this.loginForm.get('userName')?.value, 
        password: this.loginForm.get('password')?.value 
      };
      this.loadingService.isLoading = true;

      this.userService.login(loginUserDTO).subscribe({
        next: (response) => {
          this.setLocalStorage(loginUserDTO, response as LoginUserResponse);
          this.router.navigate(['profile']);
        },
        error: (err) => {
          console.log(err);
          this.snackService.openSnackBar(err.statusText, "OK");
          this.tokenService.deleteLocalStorage();
        }
      }).add(() => { this.loadingService.isLoading = false; });
      this.loginForm.reset();
    }
  }

  /**
   * Process and store login response in local storage
   * @param loginDto Login values sent
   * @param loginUserResponse Login response received
   */
  private setLocalStorage(loginDto: LoginUserDTO, loginUserResponse: LoginUserResponse){
    this.tokenService.setLocalStorage(loginUserResponse.access_token, loginUserResponse.refresh_token, loginDto.username);
    const refreshTimer = timer((loginUserResponse.expires_in*1000)-5000, (loginUserResponse.expires_in*1000)-5000); 
    
    refreshTimer.subscribe(() => {
      this.userService.refresh(this.tokenService.getRefreshToken()).subscribe({
        next: (res) => {
          const response = res as LoginUserResponse;
          this.tokenService.setAccessToken(response.access_token)
          this.tokenService.setRefreshToken(response.refresh_token);   
        },
        error: (err) => {
          console.log(err);
          this.tokenService.deleteLocalStorage();
        }
      });
    });
  }
}
