import { Component, Input, OnDestroy } from '@angular/core';
import { MatProgressButtonOptions } from 'mat-progress-buttons';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-button-spinner',
  templateUrl: './button-spinner.component.html',
  styleUrls: ['./button-spinner.component.scss']
})
export class ButtonSpinnerComponent implements OnDestroy {
  private currentTimeout: number;
  private isDelayedRunning = false;

  constructor(private translate: TranslateService) {}

  @Input()
  public set isRunning(value: boolean) {
    if (!value) {
      this.cancelTimeout();
      this.isDelayedRunning = false;
      this.btnOpts.active = false;
      return;
    }

    if (this.currentTimeout) {
      return;
    }

    // specify window to side-step conflict with node types: https://github.com/mgechev/angular2-seed/issues/901
    this.currentTimeout = window.setTimeout(() => {
      this.btnOpts.active = value;
      this.isDelayedRunning = value;
      this.cancelTimeout();
    }, this.delay);
  }

  @Input()
  public set inputValue(value: string) {
    this.translate
      .get(value)
      .subscribe((res: string) => (this.btnOpts.text = res));
  }

  @Input()
  public set inputDisabled(value: boolean) {
    this.btnOpts.disabled = value;
  }

  @Input()
  public delay = 150;

  // Button Options
  btnOpts: MatProgressButtonOptions = {
    active: false,
    text: '',
    spinnerSize: 19,
    raised: true,
    stroked: false,
    flat: false,
    fab: false,
    buttonColor: 'primary',
    spinnerColor: 'primary',
    fullWidth: true,
    disabled: false,
    mode: 'indeterminate',
    type: 'submit'
  };

  private cancelTimeout(): void {
    clearTimeout(this.currentTimeout);
    this.currentTimeout = undefined;
  }

  ngOnDestroy(): any {
    this.cancelTimeout();
  }
}
