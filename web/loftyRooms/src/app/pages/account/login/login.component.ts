import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { GenericServiceService } from 'src/app/common/services/generic-service.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  error_message: string = '';
  constructor(private service: GenericServiceService, private router: Router) { }

  ngOnInit(): void {
    sessionStorage.setItem('token', '');
    sessionStorage.setItem('userid', '');
    sessionStorage.setItem('username', '');
  }

  hide = true;
  hide2 = true;
  loginModal = new FormGroup({
    UserName: new FormControl('', [Validators.required, Validators.minLength(3)]),
    Password: new FormControl('', [Validators.required, Validators.minLength(3)]),
  });

  get f(): { [key: string]: AbstractControl } {
    return this.loginModal.controls;
  }

  Login() {
    if (this.loginModal.invalid) {
      return;
    }
    this.service.post("Account/Login", this.loginModal.value).subscribe(res => {
      this.service.setLoading(false);
      if (res.successList.token != undefined) {
        sessionStorage.setItem('token', res.successList.token);
        sessionStorage.setItem('userid', res.successList.userId);
        sessionStorage.setItem('username', res.successList.userName);
        sessionStorage.setItem('claims', res.successList.claims);
        // window.location.href = '/home';
        window.location.href = '/';
      } else {
        this.error_message = 'Invalid Username Or Password';
      }
    });
  }
}
