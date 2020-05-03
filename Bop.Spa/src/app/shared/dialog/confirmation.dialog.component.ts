import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DialogData } from 'src/app/core/models/dialog.data';

@Component({
  selector: 'confirmation-dialog',
  templateUrl: './confirmation.dialog.component.html'
})
export class ConfirmationDialogComponent implements OnInit {
  title: string;
  body: string;

  constructor(
    private dialogRef: MatDialogRef<ConfirmationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: DialogData
  ) {
    this.title = data.title;
    this.body = data.body;
  }

  ngOnInit() {}
}
