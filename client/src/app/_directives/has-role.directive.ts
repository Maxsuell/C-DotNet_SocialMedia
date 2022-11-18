import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { take } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective {
  @Input() appHasRole: string[];
  user: User;

  constructor(private viewContainerRef: ViewContainerRef, private templateRef: TemplateRef<any>, private accountService: AccountService) 
  {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        this.user = user;
      }, error: error => error.error
    });
  }

  ngOnInit(): void{
    if(!this.user?.roles || this.user == null)
    {
      this.viewContainerRef.clear();
      return;
    }

    if(this.user.roles.some(r => this.appHasRole.includes(r)))
    {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    }else
    {
      this.viewContainerRef.clear();
    }
  }

}
