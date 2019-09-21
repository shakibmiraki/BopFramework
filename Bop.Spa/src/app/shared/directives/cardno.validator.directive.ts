import { Directive, forwardRef } from '@angular/core';
import { NG_VALIDATORS, FormControl } from '@angular/forms';

function validatePanFactory() {
  return (c: FormControl) => {
    let PAN_REGEXP = /^[0-9]{4}-[0-9]{4}-[0-9]{4}-[0-9]{4}$/gm;

    return PAN_REGEXP.test(c.value)
      ? null
      : {
          validatePan: {
            valid: false
          }
        };
  };
}

@Directive({
  selector: '[validatePan][ngModel],[validatePan][formControl]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => PanValidator),
      multi: true
    }
  ]
})
export class PanValidator {
  validator: Function;

  constructor() {
    this.validator = validatePanFactory();
  }

  validate(c: FormControl) {
    return this.validator(c);
  }
}
