import { Component, OnInit } from '@angular/core';
import { CardAuth } from 'src/app/core/models/card-auth';
import { CardService } from 'src/app/core/services/card.service.';
import { finalize } from 'rxjs/operators';
import { NgForm } from '@angular/forms';
import * as AOS from 'aos';

@Component({
  selector: 'app-card-auth',
  templateUrl: './card-auth.component.html',
  styleUrls: ['./card-auth.component.scss']
})
export class CardAuthComponent implements OnInit {
  model: CardAuth = {
    pan: '',
    pin2: '',
    cvv2: '',
    month: '',
    year: ''
  };
  errors = '';
  isRequesting: boolean;
  submitted = false;

  public panMask = [
    /[0-9]/,
    /\d/,
    /\d/,
    /\d/,
    '-',
    /\d/,
    /\d/,
    /\d/,
    /\d/,
    '-',
    /\d/,
    /\d/,
    /\d/,
    /\d/,
    '-',
    /\d/,
    /\d/,
    /\d/,
    /\d/
  ];

  public cvv2Mask = [/[0-9]/, /\d/, /\d/, /\d/];

  public monthMask = [/[0-9]/, /\d/];

  public yearMask = [/[1-9]/, /\d/, /\d/, /\d/];

  constructor(private cardService: CardService) {}

  ngOnInit() {
    AOS.init({
      duration: 900,
      delay: 100,
      once: true
    });
  }

  public onSubmit(f: NgForm) {
    this.submitted = true;
    this.isRequesting = true;
    this.errors = '';

    this.cardService
      .authenticate(this.model)
      .pipe(finalize(() => (this.isRequesting = false)))
      .subscribe(isAuthenticated => {
        if (isAuthenticated) {
          f.form.reset();
        }
      });
  }
}
