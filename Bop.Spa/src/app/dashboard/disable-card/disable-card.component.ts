import { CardService } from 'src/app/core/services/card.service.';
import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from 'src/app/shared/dialog/confirmation.dialog.component';
import { DialogData } from 'src/app/core/models/dialog.data';
import { CardStatus } from 'src/app/core/models/card-status';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import * as AOS from 'aos';

let cardStatus: CardStatus[];

@Component({
  selector: 'app-disable-card',
  templateUrl: './disable-card.component.html',
  styleUrls: ['./disable-card.component.scss']
})
export class DisableCardComponent implements OnInit {
  errors = '';
  isRequesting: boolean;
  submitted = false;

  displayedColumns: string[] = ['operation', 'status', 'pan', 'image'];
  dataSource = cardStatus;

  constructor(
    private dialog: MatDialog,
    private cardService: CardService,
    private router: Router
  ) {}

  ngOnInit() {
    AOS.init({
      duration: 900,
      delay: 100,
      once: true
    });

    this.isRequesting = true;
    this.cardService
      .getCards()
      .pipe(finalize(() => (this.isRequesting = false)))
      .subscribe(cards => {
        cardStatus = cards;
        this.dataSource = cardStatus;
      });
  }

  public enableCard(pan: string) {
    const dialogConfig = new MatDialogConfig<DialogData>();
    dialogConfig.data = {
      title: 'فعال سازی کارت',
      body: 'آیا از فعال سازی کارت اطمینان دارید ؟'
    };

    const status = cardStatus.find(a => a.pan === pan).status.status;
    const dialogRef = this.dialog.open(
      ConfirmationDialogComponent,
      dialogConfig
    );

    dialogRef.afterClosed().subscribe(result => {
      if (result && !status) {
        this.cardService.enableCard(pan).subscribe(response => {
          if (response) {
            this.cardService.getCards().subscribe(cards => {
              cardStatus = cards;
              this.dataSource = cardStatus;
            });
          }
        });
      }
    });
  }

  public disableCard(pan: string) {
    const dialogConfig = new MatDialogConfig<DialogData>();
    dialogConfig.data = {
      title: 'غیر فعال سازی کارت',
      body: 'آیا از غیر فعال سازی کارت اطمینان دارید ؟'
    };

    const status = cardStatus.find(a => a.pan === pan).status.status;

    const dialogRef = this.dialog.open(
      ConfirmationDialogComponent,
      dialogConfig
    );

    dialogRef.afterClosed().subscribe(result => {
      if (result && status) {
        this.cardService.disableCard(pan).subscribe(response => {
          if (response) {
            this.cardService.getCards().subscribe(cards => {
              cardStatus = cards;
              this.dataSource = cardStatus;
            });
          }
        });
      }
    });
  }

  public viewReceipt(pan: string) {
    this.router.navigate(['/dashboard/receipt-list/', pan]);
  }
}
