import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  LoggedIn: boolean = false;

  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
  }

  login()
  {
    this.accountService.login(this.model).subscribe(response => 
      {
        console.log(response);       
      }, error => console.log(error));
  }

  Logout()
  {
    
    this.accountService.logout();
  }

}
