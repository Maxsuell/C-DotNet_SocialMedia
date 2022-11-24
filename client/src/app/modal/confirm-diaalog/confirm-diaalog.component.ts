import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-confirm-diaalog',
  templateUrl: './confirm-diaalog.component.html',
  styleUrls: ['./confirm-diaalog.component.css']
})
export class ConfirmDiaalogComponent implements OnInit {
  title = '';
  message = '';
  btnOkText = '';
  btnCancelText = '';
  result = false;

  constructor(private bsModalRef: BsModalRef) { }

  ngOnInit(): void {
  }

  confirm()
  {
    this.result = true;
    this.bsModalRef.hide();
  }

  decline()
  {
    this.bsModalRef.hide();
  }

}
