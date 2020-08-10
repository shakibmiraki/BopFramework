import i18next from "i18next";
import { languages } from "./../constants/languages";
import { localStorageService } from "./localStorage";
import { storage_key } from "../constants/constant";

const isLTR = () => {
  const language = getActiveLanguage();
  return languages.filter((a) => a.code === language)[0].ltr;
};

const getActiveLanguage = () => localStorageService.getKey(storage_key.i18nextLng) || i18next.language;

const getLocaleClassName = (ltr) => {
  return ltr ? "left" : "right";
};

const getDefaultLocale = () => {
  const language = getActiveLanguage();
  return languages.filter((a) => a.code === language)[0].locale;
};

const getLocalDirectionName = (ltr) => {
  return ltr ? "ltr" : "rtl";
};

const getLanguageInitialized = () => {
  return localStorageService.getKey(storage_key.language_initialize);
};

const setDirElement = () => {
  let direction;
  isLTR() ? (direction = "ltr") : (direction = "rtl");
  document.body.setAttribute("dir", direction);
};
const setLangElement = () => {
  const default_language = getDefaultLocale();
  document.documentElement.setAttribute("lang", default_language);
};

const setLanguageInitialized = () => {
  localStorageService.setKey(storage_key.language_initialize, true);
};

const changeLanguage = (i18n, languageCode) => {
  i18n.changeLanguage(languageCode);
};

export const localeService = {
  isLTR,
  getActiveLanguage,
  getLocaleClassName,
  getDefaultLocale,
  getLocalDirectionName,
  getLanguageInitialized,
  setDirElement,
  setLangElement,
  setLanguageInitialized,
  changeLanguage,
};
