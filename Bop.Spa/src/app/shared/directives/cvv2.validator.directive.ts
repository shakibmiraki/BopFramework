import { Directive, forwardRef } from '@angular/core';
import { NG_VALIDATORS, FormControl } from '@angular/forms';

function validateCvv2Factory() {
  return (c: FormControl) => {
    let Cvv2_REGEXP = /^[0-9]{3,4}$/gm;

    return Cvv2_REGEXP.test(c.value)
      ? null
      : {
          validateCvv2: {
            valid: false
          }
        };
  };
}

@Directive({
  selector: '[validateCvv2][ngModel],[validateCvv2][formControl]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => Cvv2Validator),
      multi: true
    }
  ]
})
export class Cvv2Validator {
  validator: Function;

  constructor() {
    this.validator = validateCvv2Factory();
  }

  validate(c: FormControl) {
    return this.validator(c);
  }
}
