import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { map, observable, Observable } from 'rxjs';
import { ConfirmDiaalogComponent } from '../modal/confirm-diaalog/confirm-diaalog.component';


@Injectable({
  providedIn: 'root'
})
export class ConfirmService {
  bsModalRef?: BsModalRef<ConfirmDiaalogComponent>;

  constructor(private modalService: BsModalService) { }

  confirm(
    title = 'Confirmation',
    message = 'Are you sure you want to do this?',
    btnOkText = 'Ok',
    btnCanceltext = 'Cancel'): Observable<boolean>
  {
    const config = {
     initialState: {
      title,
      message,
      btnOkText,
      btnCanceltext
      }
    }
    this.bsModalRef = this.modalService.show(ConfirmDiaalogComponent, config);
    return new Observable<boolean>(this.getResult());
  }

  private getResult() {
    return (observer) => {
      const subscription = this.bsModalRef.onHidden.subscribe(() => {
        observer.next(this.bsModalRef.content.result);
        observer.complete();
      });

      return {
        unsubscribe() {
          subscription.unsubscribe();
        }
      }
    }
  }

}
