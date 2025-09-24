import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NotificationComponent } from './components/modals/notification/notification.component';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  bsModalRef?: BsModalRef; // Fixed typo: 'bsModelRef' → 'bsModalRef'

  constructor(private modalService: BsModalService) { }

  showNotification(isSuccess: boolean, title: string, message: string) {
    const initialState = { // Fixed: Added '=' and proper object syntax
      isSuccess,
      title,
      message
    };

    this.bsModalRef = this.modalService.show(NotificationComponent, { initialState }); // Fixed: Moved inside method
  }
}