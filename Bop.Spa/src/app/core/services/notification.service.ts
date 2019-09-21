import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private messageQueue: Array<string> = Array<string>();
  private duration = 5000;

  constructor(
    public snackBar: MatSnackBar,
    private translate: TranslateService
  ) {}

  showMessage(messages: Array<string>): void {
    for (const message of messages) {
      const key = message.toLowerCase();
      this.translate.get(key).subscribe((res: string) => {
        this.messageQueue.push(res);
      });
    }

    this.messageQueue.forEach((message, index) => {
      setTimeout(() => {
        const shiftedMessage = this.messageQueue.shift();
        this.snackBar.open(shiftedMessage, 'X', {
          panelClass: ['error'],
          duration: this.duration
        });
      }, this.duration * index);
    });
  }
}
