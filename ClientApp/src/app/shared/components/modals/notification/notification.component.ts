import { Component } from '@angular/core';
import {BsModalRef} from 'ngx-bootstrap/modal'

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})

export class NotificationComponent {
   isSuccess!: boolean; // Renamed from `isSuccessful` to `isSuccess`
   title!: string;
   message!: string;

  constructor(public bsModalRef: BsModalRef) {}
}
