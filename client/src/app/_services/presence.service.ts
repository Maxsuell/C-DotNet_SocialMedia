import { Injectable } from '@angular/core';
import { Route, Router } from '@angular/router';
import { HubConnection } from '@microsoft/signalr';
import { HubConnectionBuilder } from '@microsoft/signalr/dist/esm/HubConnectionBuilder';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  hubUrl = 'https://localhost:5001/hubs/presence';
  private hubConnection: HubConnection;
  private onlineUsersSource = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this.onlineUsersSource.asObservable();
  private newMessageSource = new BehaviorSubject<number[]>([]);
  newMessage$ = this.newMessageSource.asObservable();

  constructor(private toastr: ToastrService, private router: Router) { }

  createHubConnection(user: User)
  {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl, {
        accessTokenFactory: () => user.token
      }).withAutomaticReconnect().build();

      this.hubConnection
        .start().catch(error => console.log(error));
      
      this.hubConnection.on('UserIsOnline', username => {
        this.onlineUsers$.pipe(take(1)).subscribe(usernames => 
          {
            this.onlineUsersSource.next([...usernames, username]);
          })
      });
      

      this.hubConnection.on('UserIsOffline', username => {
        this.onlineUsers$.pipe(take(1)).subscribe(usernames => 
          {
            this.onlineUsersSource.next([...usernames.filter(x => x !== username)]);
          })
      });

      this.hubConnection.on('GetOnlineUsers', (usernames: string[]) => {
        this.onlineUsersSource.next(usernames);
      });

      this.hubConnection.on('NewMessageReceived', ({username,knownAs}) => 
      {
        this.newMessageSource.pipe(take(1)).subscribe( x => x[0] = 3 );
        console.log(this.newMessageSource);
        this.toastr.info(knownAs + ' has sent you a new message!')
          .onTap
          .pipe(take(1))
          .subscribe(() => this.router.navigateByUrl('/members/'+ username + '?tab=3'));
      });
  }

  stopHubConnection()
  {
    this.hubConnection.stop().catch(error => console.log(error));
  }
}

