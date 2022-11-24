import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';
import { PresenceService } from '../_services/presence.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  title = "SemNomeAinda";
  model: any = {};
  user =  this.model;
  numMessage: number;


  constructor(public accountService: AccountService, 
    private router: Router, 
    private toastr : ToastrService) { }

  ngOnInit(): void {

  }

  login()
  {  
    this.accountService.login(this.user).subscribe({
      next: () => {
        this.router.navigateByUrl('/');                       
  }});
  }

  Logout()
  {
    
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }  

}
