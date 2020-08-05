import { Language } from './../../core/models/localization-languages';
import { LocalizationService } from './../../core/services/localization.service';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { finalize } from 'rxjs/operators';
import { MatSelectChange } from '@angular/material/select';
import { MatOption } from '@angular/material/core';
import * as AOS from 'aos';

@Component({
  selector: 'app-localization',
  templateUrl: './localization.component.html',
  styleUrls: ['./localization.component.scss']
})
export class LocalizationComponent implements OnInit {
  errors = '';
  isRequesting: boolean;
  submitted = false;
  languages: Language[];
  selectedLanguageId: number;
  selectedLanguageName: string;

  constructor(
    private localizationService: LocalizationService,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit() {
    AOS.init({
      duration: 900,
      delay: 100,
      once: true
    });

    this.localizationService.getAllLanguages().subscribe(languageList => {
      this.languages = languageList;
    });
  }

  selected(event: MatSelectChange) {
    this.selectedLanguageId = event.source.value;
    console.log((event.source.selected as MatOption).viewValue.split('-'));
    const languageArray = (event.source.selected as MatOption).viewValue.split('-');
    this.selectedLanguageName = `${languageArray[1].trim().toLowerCase()}-${languageArray[2].trim().toUpperCase()}`;
    console.log(this.selectedLanguageName);
  }

  public onSubmit() {
    this.submitted = true;
    this.isRequesting = true;
    this.errors = '';

    this.localizationService
      .exportLanguage(this.selectedLanguageId)
      .pipe(finalize(() => (this.isRequesting = false)))
      .subscribe(result => {
        let json = JSON.stringify(result);
        const QOUTATION_REGEX = /\"(.+)\"/gm;
        const BACKSLASH_REGEX = /\\/g;
        json = json.replace(BACKSLASH_REGEX, '');
        json = json.replace(QOUTATION_REGEX, '$1');

        const link = document.createElement('a');
        link.download = `${this.selectedLanguageName}.json`;
        link.href =
          'data:application/json;charset=UTF-8,' + encodeURIComponent(json);
        link.click();
      });
  }
}
