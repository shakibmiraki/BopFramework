import { ResponseModel } from './response';

export class Language {
  languageId: number;
  languageName: string;
  languageCulture: string;
  languageCode: string;
}

export class LanguageList extends ResponseModel {
  languages: Language[];
}
