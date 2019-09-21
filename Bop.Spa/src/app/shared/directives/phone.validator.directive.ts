import { Directive, forwardRef } from '@angular/core';
import { NG_VALIDATORS, FormControl } from '@angular/forms';

function validatePhoneFactory() {
  return (c: FormControl) => {
    let Phone_REGEXP = /^(09)[13][0-9]\d{7}$/i;

    return Phone_REGEXP.test(c.value) ? null : {
      validatePhone: {
        valid: false
      }
    };
  };
}

@Directive({
  selector: '[validatePhone][ngModel],[validatePhone][formControl]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => PhoneValidator), multi: true }
  ]
})
export class PhoneValidator {

  validator: Function;

  constructor() {
    this.validator = validatePhoneFactory();
  }

  validate(c: FormControl) {
    return this.validator(c);
  }
}