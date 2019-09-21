import {
  ReceiptRequest,
  Receipt,
  ReceiptList
} from './../../core/models/receipt';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ReceiptService } from 'src/app/core/services/receipt.service';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { finalize, delay } from 'rxjs/operators';
import * as momentJalaali from 'moment-jalaali';
import * as AOS from 'aos';

@Component({
  selector: 'app-receipt-list',
  templateUrl: './receipt-list.component.html',
  styleUrls: ['./receipt-list.component.scss']
})
export class ReceiptListComponent implements OnInit, OnDestroy {
  private subscription: Subscription;
  errors = '';
  isRequesting: boolean;
  submitted = false;
  cardno = '';
  receiptlists: ReceiptList[] = [];
  counter = 0;
  allreceiptloade = false;
  filterApplied = false;
  fromDate = '';
  toDate = '';
  locationArray: Array<string> = [
    'تهران ، چهارراه گلوبندک ، خیابان خیام جنوبی',
    'تهران ، میدان قزوین ، خیابان قزوین ، نبش خیابان غفاری',
    'تهران ، خیابان طالقانی ، نرسیده به خیابان دکترشریعتی',
    'تهران ، میدان توحید ، میدان انقلاب ، ضلع شمال شرقی',
    'تهران ، خیابان پانزدهم خرداد شرقی ، سبزه میدان',
    'تهران ، میدان شهدا ، ابتدای خیابان هفدهم شهریور',
    'تهران ، خیابان امیرکبیر ، چهارراه سرچشمه ، پاساژ امیرکبیر',
    'تهران ، خیابان دکتر فاطمی ، میدان جهاد',
    'تهران ، خیابان جمهوری اسلامی ، بین خیابان ولیعصر و فلسطین',
    'تهران ، میدان شوش ، خیابان شهید صابونیان ، خیابان کاخ جوانان',
    'تهران ، خیابان دکتر شریعتی ، باغ صبا ، ایستگاه عوارضی'
  ];

  constructor(
    private receiptService: ReceiptService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    // subscribe to router event
    this.subscription = this.route.params.subscribe((param: any) => {
      this.cardno = param['cardno'];
    });

    AOS.init({
      duration: 900,
      delay: 100,
      once: true
    });

    this.loadReceipt();
  }

  public onSubmit() {
    if (this.fromDate && this.toDate) {
      this.filterApplied = true;
    }

    if (this.filterApplied) {
      this.submitted = true;
      this.isRequesting = true;
      this.errors = '';
      const NUMERIC_EXP = /\d+/g;
      const receiptRequest: ReceiptRequest = {
        cardno: this.cardno,
        startDate: this.fromDate.match(NUMERIC_EXP).join(''),
        endDate: this.toDate.match(NUMERIC_EXP).join('')
      };

      this.receiptService
        .getReceipts(receiptRequest)
        .pipe(delay(1000))
        .pipe(finalize(() => (this.isRequesting = false)))
        .subscribe((result: Receipt[]) => {
          const receipt: ReceiptList = {
            receipts: result,
            date: null
          };
          this.receiptlists = [];

          //TODO : delete after demo
          receipt.receipts.forEach(element => {
            element.sellerInfo.address = this.locationArray[
              Math.floor(Math.random() * this.locationArray.length)
            ];
            const remain = Math.floor((Math.random() * 9000000) + 3000000);
            const actual = remain + 50000;
            element.paymentInfo.availableBalance = remain;
            element.paymentInfo.actualBalance = actual;
          });
          //End TODO;
          this.receiptlists.push(receipt);
        });
    }
  }

  loadReceipt() {
    if (!this.allreceiptloade && !this.filterApplied) {
      this.isRequesting = true;
      const endDate = momentJalaali().subtract(this.counter, 'month');
      const startDate = momentJalaali().subtract(this.counter + 1, 'month');
      const receiptRequest: ReceiptRequest = {
        cardno: this.cardno,
        startDate: startDate.format('jYYYYjMMjDD'),
        endDate:
          this.counter === 0
            ? endDate.add(1, 'day').format('jYYYYjMMjDD')
            : endDate.format('jYYYYjMMjDD')
      };
      this.isRequesting = true;
      this.receiptService
        .getReceipts(receiptRequest)
        .pipe(delay(1000))
        .pipe(finalize(() => (this.isRequesting = false)))
        .subscribe((result: Receipt[]) => {
          if (!result) {
            this.allreceiptloade = true;
          } else {
            const receipt: ReceiptList = {
              receipts: result,
              date:
                this.counter === 0
                  ? endDate.clone().subtract(1, 'day')
                  : endDate.clone()
            };
            const isExist = this.receiptlists.find(
              a =>
                a.date.format('jYYYYjMMjDD') ===
                receipt.date.format('jYYYYjMMjDD')
            );
            if (!isExist) {
              //TODO : delete after demo
              receipt.receipts.forEach(element => {
                element.sellerInfo.address = this.locationArray[
                  Math.floor(Math.random() * this.locationArray.length)
                ];
                const remain = Math.floor((Math.random() * 9000000) + 3000000);
                const actual = remain + 50000;
                element.paymentInfo.availableBalance = remain;
                element.paymentInfo.actualBalance = actual;
              });
              //End TODO;
              this.receiptlists.push(receipt);
              this.counter = this.counter + 1;
            }
          }
        });
    }
  }

  show_source_cardno(paymentType: number): boolean {
    if (
      paymentType === 2 ||
      paymentType === 3 ||
      paymentType === 17 ||
      paymentType === 24 ||
      paymentType === 55
    ) {
      return true;
    }
    return false;
  }

  show_destination_cardno(paymentType: number): boolean {
    return false;
  }

  show_rrn(paymentType: number): boolean {
    if (
      paymentType === 2 ||
      paymentType === 3 ||
      paymentType === 12 ||
      paymentType === 17 ||
      paymentType === 24 ||
      paymentType === 55
    ) {
      return true;
    }
    return false;
  }

  show_source_account(paymentType: number): boolean {
    if (paymentType === 12 || paymentType === 55) {
      return true;
    }
    return false;
  }

  show_terminal_id(paymentType: number): boolean {
    if (
      paymentType === 2 ||
      paymentType === 3 ||
      paymentType === 12 ||
      paymentType === 17 ||
      paymentType === 24 ||
      paymentType === 5
    ) {
      return true;
    }
    return false;
  }

  show_actual_balance(paymentType: number): boolean {
    if (paymentType === 5 || paymentType === 12) {
      return true;
    }
    return false;
  }

  show_available_balance(paymentType: number): boolean {
    if (paymentType === 12) {
      return true;
    }
    return false;
  }

  show_amount(paymentType: number): boolean {
    if (paymentType === 3 || paymentType === 17) {
      return true;
    }
    return false;
  }

  show_customer_name(paymentType: number): boolean {
    if (paymentType === 3) {
      return true;
    }
    return false;
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
