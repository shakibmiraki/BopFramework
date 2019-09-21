import { Pipe, PipeTransform } from "@angular/core";


@Pipe({
  name: "cardno"
})
export class CardnoPipe implements PipeTransform {
  transform(value: any): any {
    const splitted = value.match(/.{1,4}/g);
    return `${splitted[0]}-${splitted[1]}-${splitted[2]}-${splitted[3]}`;

  }
}