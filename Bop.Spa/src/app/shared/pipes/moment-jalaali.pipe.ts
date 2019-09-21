import { Pipe, PipeTransform } from '@angular/core';

import * as momentJalaali from 'moment-jalaali';

@Pipe({
  name: 'toJalaali'
})
export class toJalaaliPipe implements PipeTransform {
  transform(value: any, args?: any): any {
    return momentJalaali(value).format(args);
  }
}

@Pipe({
  name: 'formatJalaali'
})
export class formatJalaaliPipe implements PipeTransform {
  transform(value: any, currentFormat?: any, appliedFormat?: any): any {
    const parsedDate = momentJalaali(value, currentFormat);
    return parsedDate.format(appliedFormat);
  }
}
