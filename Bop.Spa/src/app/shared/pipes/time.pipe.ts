import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatTime'
})
export class formatTimePipe implements PipeTransform {
  transform(value: any): any {
    const splitted = value.match(/.{1,2}/g);
    return `${splitted[0]}:${splitted[1]}:${splitted[2]}`;
  }
}