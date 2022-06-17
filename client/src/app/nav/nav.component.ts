import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  title = "SemNomeAinda";
  model: any = {};
  user: any = {
      "username": "maxx",
      "password": "Password"
  }


  constructor(public accountService: AccountService, private router: Router, private toastr : ToastrService) { }

  ngOnInit(): void {

  }

  login()
  {
  console.log(this.user);
    this.accountService.login(this.user).subscribe({
      next: response => {
        this.router.navigateByUrl('/messages');
        console.log(response);
               
  }});
  }

  Logout()
  {
    
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }


}
